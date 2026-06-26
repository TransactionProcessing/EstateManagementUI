import crypto from 'node:crypto';
import fs from 'node:fs/promises';

const repo = process.env.GITHUB_REPOSITORY;
const token = process.env.GITHUB_TOKEN;
const threshold = normalizeSeverity(process.env.SEMGREP_MIN_SEVERITY || 'warning');
const runUrl = process.env.GITHUB_RUN_ID && repo
  ? `https://github.com/${repo}/actions/runs/${process.env.GITHUB_RUN_ID}`
  : '';

if (!repo) {
  throw new Error('GITHUB_REPOSITORY is required.');
}

if (!token) {
  throw new Error('GITHUB_TOKEN is required.');
}

const inputPath = process.argv[2] || 'semgrep.json';
const raw = await fs.readFile(inputPath, 'utf8').catch(() => '');
const parsed = raw.trim() ? JSON.parse(raw) : {};
const results = Array.isArray(parsed.results) ? parsed.results : [];
const findings = results
  .map(normalizeFinding)
  .filter((finding) => finding && severityRank(finding.severity) >= threshold.rank);

const seenMarkers = new Set();
const existingIssues = await searchIssues(`repo:${repo} is:issue "semgrep-finding-id:" in:body`);

for (const finding of findings) {
  const marker = finding.marker;
  seenMarkers.add(marker);

  const matches = await searchIssues(`repo:${repo} is:issue "${marker}" in:body`);
  const canonical = chooseCanonicalIssue(matches);

  if (matches.length > 1) {
    for (const duplicate of matches) {
      if (canonical && duplicate.id === canonical.id) {
        continue;
      }
      await closeIssue(duplicate.number, `Consolidated into #${canonical?.number ?? 'N/A'} for the same Semgrep finding marker.`);
    }
  }

  if (canonical) {
    await upsertIssue(canonical, finding, marker);
  } else {
    await createIssue(finding, marker);
  }
}

for (const issue of existingIssues) {
  const marker = extractMarker(issue.body || '');
  if (!marker || seenMarkers.has(marker)) {
    continue;
  }

  if (issue.state === 'open') {
    await closeIssue(issue.number, 'Resolved by a later Semgrep scan.');
  }
}

function normalizeFinding(result) {
  const path = result?.path || result?.location?.path || '';
  const ruleId = result?.check_id || result?.rule_id || result?.ruleId || result?.id || 'unknown-rule';
  const message = normalizeText(result?.extra?.message || result?.message || '');
  const severity = normalizeSeverity(result?.extra?.severity || result?.severity || 'warning').name;
  const startLine = result?.start?.line || result?.location?.start_line || result?.start_line || null;
  const endLine = result?.end?.line || result?.location?.end_line || result?.end_line || startLine;
  const snippet = normalizeText(result?.extra?.lines || result?.lines || '');

  if (!path) {
    return null;
  }

  const markerSource = [
    ruleId,
    cleanPath(path),
    message,
    snippet,
  ].join('|');

  return {
    path: cleanPath(path),
    ruleId,
    message,
    severity,
    startLine,
    endLine,
    snippet,
    marker: sha256(markerSource),
  };
}

function chooseCanonicalIssue(issues) {
  if (!issues.length) {
    return null;
  }

  const openIssues = issues.filter((issue) => issue.state === 'open');
  const pool = openIssues.length ? openIssues : issues;

  return pool
    .slice()
    .sort((left, right) => new Date(right.updated_at) - new Date(left.updated_at))[0];
}

async function upsertIssue(issue, finding, marker) {
  const title = buildTitle(finding);
  const body = buildBody(finding, marker);

  if (issue.state !== 'open') {
    await api(`/repos/${repo}/issues/${issue.number}`, 'PATCH', { state: 'open' });
  }

  if (issue.title !== title || issue.body !== body) {
    await api(`/repos/${repo}/issues/${issue.number}`, 'PATCH', { title, body });
  }
}

async function createIssue(finding, marker) {
  const title = buildTitle(finding);
  const body = buildBody(finding, marker);

  await api(`/repos/${repo}/issues`, 'POST', { title, body });
}

async function closeIssue(number, comment) {
  if (comment) {
    await api(`/repos/${repo}/issues/${number}/comments`, 'POST', { body: comment });
  }

  await api(`/repos/${repo}/issues/${number}`, 'PATCH', { state: 'closed' });
}

function buildTitle(finding) {
  const linePart = finding.startLine ? `:${finding.startLine}` : '';
  const title = `[Semgrep][${finding.severity.toUpperCase()}] ${finding.ruleId} ${finding.path}${linePart}`;
  return title.length > 240 ? title.slice(0, 240) : title;
}

function buildBody(finding, marker) {
  const lines = finding.startLine && finding.endLine && finding.startLine !== finding.endLine
    ? `${finding.startLine}-${finding.endLine}`
    : (finding.startLine ? `${finding.startLine}` : 'unknown');

  const parts = [
    `<!-- semgrep-finding-id: ${marker} -->`,
    `<!-- semgrep-rule-id: ${finding.ruleId} -->`,
    `<!-- semgrep-path: ${finding.path} -->`,
    `<!-- semgrep-severity: ${finding.severity} -->`,
    '',
    `Semgrep reported a ${finding.severity} finding in \`${finding.path}\`.`,
    '',
    `- Rule: \`${finding.ruleId}\``,
    `- Path: \`${finding.path}\``,
    `- Lines: \`${lines}\``,
    `- Severity: \`${finding.severity}\``,
  ];

  if (runUrl) {
    parts.push(`- Scan: ${runUrl}`);
  }

  if (finding.message) {
    parts.push('', '### Message', finding.message);
  }

  if (finding.snippet) {
    parts.push('', '### Match', '```text', finding.snippet, '```');
  }

  return parts.join('\n');
}

function extractMarker(body) {
  const match = body.match(/<!--\s*semgrep-finding-id:\s*([a-f0-9]{64})\s*-->/i);
  return match ? match[1] : null;
}

function normalizeSeverity(value) {
  const normalized = String(value || 'warning').trim().toLowerCase();

  if (normalized === 'info' || normalized === 'information') {
    return { name: 'info', rank: 1 };
  }

  if (normalized === 'warning' || normalized === 'warn') {
    return { name: 'warning', rank: 2 };
  }

  if (normalized === 'error' || normalized === 'high' || normalized === 'critical') {
    return { name: 'error', rank: 3 };
  }

  return { name: 'warning', rank: 2 };
}

function severityRank(value) {
  return normalizeSeverity(value).rank;
}

function normalizeText(value) {
  return String(value || '').replace(/\s+/g, ' ').trim();
}

function cleanPath(value) {
  return String(value || '').replace(/\\/g, '/');
}

function sha256(value) {
  return crypto.createHash('sha256').update(value).digest('hex');
}

async function searchIssues(query) {
  const items = [];
  for (let page = 1; page < 10; page += 1) {
    const result = await api(`/search/issues?q=${encodeURIComponent(query)}&per_page=100&page=${page}`);
    if (!Array.isArray(result.items) || result.items.length === 0) {
      break;
    }

    items.push(...result.items.filter((item) => !item.pull_request));

    if (result.items.length < 100) {
      break;
    }
  }

  return items;
}

async function api(path, method = 'GET', body) {
  const response = await fetch(`https://api.github.com${path}`, {
    method,
    headers: {
      Authorization: `Bearer ${token}`,
      Accept: 'application/vnd.github+json',
      'X-GitHub-Api-Version': '2022-11-28',
      ...(body ? { 'Content-Type': 'application/json' } : {}),
    },
    body: body ? JSON.stringify(body) : undefined,
  });

  if (!response.ok) {
    const text = await response.text();
    throw new Error(`GitHub API request failed for ${method} ${path}: ${response.status} ${response.statusText}\n${text}`);
  }

  if (response.status === 204) {
    return null;
  }

  return response.json();
}
