using NUnit.Framework;

// Enable parallel test execution at the assembly level
[assembly: Parallelizable(ParallelScope.Fixtures)]

// Set the number of workers for parallel execution
// This can be overridden by command line arguments or test runner configuration
[assembly: LevelOfParallelism(3)]
