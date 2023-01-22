using System.Text;
using CliFx;
using CliFx.Infrastructure;

namespace DependencyGraph.CLI.Tests;

public class DependencyGraphSolverTests : IDisposable
{
    private string inputFileDirectory;
    private string outputFilePath;
    private string outputDirectory;
    private const string correct_input = "input.csv";
    private const string empty_input = "empty_input.csv";
    private const string cyclic_inoput = "cyclic_input.csv";
    private const string inputFilesFolder = "TestInput";
    private const string outputFolder = "TestOutput";
    private const string outputFile = "output.csv";

    public DependencyGraphSolverTests()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var baseDirectory = currentDirectory.Substring(0, currentDirectory.IndexOf("DependencyGraph.CLI.Tests"));
        inputFileDirectory = Path.Combine(baseDirectory, inputFilesFolder);
        outputDirectory = Path.Combine(baseDirectory, outputFolder);
        outputFilePath = Path.Combine(outputDirectory, outputFile);
    }

    [Fact]
    public async Task Command_WithEmptyInput_CreatesEmptyOutputFile()
    {
        using var console = new FakeInMemoryConsole();
        var command = new DependencyGraphSolverCommand
        {
            InputFile = Path.Combine(inputFileDirectory, empty_input),
            OutputFile = outputFilePath
        };

        await command.ExecuteAsync(console);

        Assert.Empty(File.ReadLines(outputFilePath).Where(line => line != "").Select(x => x.Split(',')).ToArray());
    }

    // End to end Command Test
    [Fact]
    public async Task Command_WithCorrectInput_CreatesOutputFile()
    {
        using var console = new FakeInMemoryConsole();

        var app = new CliApplicationBuilder()
            .AddCommand<DependencyGraphSolverCommand>()
            .UseConsole(console)
            .Build();

        var args = new[] { "--file", Path.Combine(inputFileDirectory, correct_input), "--output", outputFilePath };

        await app.RunAsync(args);

        var stdOut = console.ReadOutputString();

        string[][] actualOutput = File.ReadLines(outputFilePath).Where(line => line != "").Select(x => x.Split(',')).ToArray();
        string[][] expectedOuput = new string[][]
        {
            new string[]{ "left sock", "right sock", "t-shirt" },
            new string[]{ "dress shirt" },
            new string[]{ "pants", "tie" },
            new string[]{ "belt" },
            new string[]{ "suit jacket" },
            new string[]{ "left shoe", "right shoe", "sun glasses" },
            new string[]{ "overcoat" }
        };
        var expectedConsoleOutput = string.Join(Environment.NewLine,
            expectedOuput.Select(line => string.Join(',', line)))
            + Environment.NewLine;

        Assert.Equal(expectedOuput, actualOutput);
        Assert.Equal(expectedConsoleOutput, stdOut);
    }

    [Fact]
    public async Task Command_WithCyclicInput_DoesNotCreateOutputFile()
    {
        using var console = new FakeInMemoryConsole();

        var app = new CliApplicationBuilder()
            .AddCommand<DependencyGraphSolverCommand>()
            .UseConsole(console)
            .Build();

        var args = new[] { "--file", Path.Combine(inputFileDirectory, cyclic_inoput), "--output", outputFilePath };

        await app.RunAsync(args);

        var stdOut = console.ReadOutputString();

        Assert.False(Directory.Exists(outputDirectory));
        Assert.Contains("The input provided does not seem to form a directed acyclic graph, please check your input", stdOut);
    }

    public void Dispose()
    {
        //clear output directory
        if(Directory.Exists(outputDirectory))
            Directory.Delete(outputDirectory, true);
    }
}
