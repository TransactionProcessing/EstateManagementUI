using NUnit.Framework;

// Configure NUnit for parallel execution at the Children level
// This allows scenarios to run in parallel, which is ideal for browser-based tests
[assembly: Parallelizable(ParallelScope.Children)]
[assembly: LevelOfParallelism(4)]
