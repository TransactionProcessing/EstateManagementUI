// File download utility
function downloadFile(filename, base64Content) {
    const binaryString = window.atob(base64Content);
    const bytes = new Uint8Array(binaryString.length);
    for (let i = 0; i < binaryString.length; i++) {
        bytes[i] = binaryString.charCodeAt(i);
    }
    const blob = new Blob([bytes], { type: 'application/octet-stream' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
}

window.elementExists = function (id) {
    try { return document.getElementById(id) !== null; } catch { return false; }
};

window.updateOrCreateChartElement = function (canvasElement, type, labels, datasets, title) {
    try {
        if (!canvasElement) return;
        // existing chart logic but operate on canvasElement (not by id)
        // e.g., const ctx = canvasElement.getContext('2d'); ...
    } catch (e) {
        console.error(e);
    }
};