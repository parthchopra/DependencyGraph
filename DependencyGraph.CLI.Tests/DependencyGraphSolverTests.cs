using CliFx.Infrastructure;

namespace DependencyGraph.CLI.Tests;

public class DependencyGraphSolverTests
{
    [Fact]
    public async Task Command_WithEmptyInput_CreatesEmptyOutputFile()
    {
        using var console = new FakeInMemoryConsole();
        var command = new DependencyGraphSolverCommand
        {
            InputFile = ""
        };

        await command.ExecuteAsync(console);
    }
}
