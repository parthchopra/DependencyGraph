namespace DependencyGraph.Tests;

public class DependencyGraphSolverTests
{
    public DependencyGraphSolverTests()
    {

    }

    [Fact]
    public void Solve_WithCorrectlyFormedGraph_ReturnsTopologicallySortedAndAlphabeticallyOrderedList()
    {

        var input = new string[,]
        {
            {"t-shirt", "dress shirt"},
            {"dress shirt", "pants"},
            {"dress shirt", "suit jacket"},
            {"tie", "suit jacket"},
            {"pants", "suit jacket"},
            {"belt", "suit jacket"},
            {"suit jacket", "overcoat"},
            {"dress shirt", "tie"},
            {"suit jacket", "sun glasses"},
            {"sun glasses", "overcoat"},
            {"left sock", "pants"},
            {"pants", "belt"},
            {"suit jacket", "left shoe"},
            {"suit jacket", "right shoe"},
            {"left shoe", "overcoat"},
            {"right sock", "pants"},
            {"right shoe", "overcoat"},
            {"t-shirt", "suit jacket"}
        };

        var expectedList = new List<List<string>>
        {
            new List<string>{ "left sock", "right sock", "t-shirt" },
            new List<string>{ "dress shirt" },
            new List<string>{ "pants", "tie" },
            new List<string>{ "belt" },
            new List<string>{ "suit jacket" },
            new List<string>{ "left shoe", "right shoe", "sun glasses" },
            new List<string>{ "overcoat" }
        };

        var sut = new DependencyGraphSolver(input, null);
        var actualResult = sut.Solve();

        Assert.Equal(expectedList, actualResult);
    }

    [Fact]
    public void Solve_WithCyclicalGraph_ThrowsException()
    {
        var input = new String[,]
        {
            { "A", "B" },
            { "B", "C" },
            { "D", "A" },
            { "E", "D" },
            { "D", "F" },
            { "F", "E" }
        };

        var sut = new DependencyGraphSolver(input, null);

        Assert.Throws<InvalidDAGException>(() => sut.Solve());
    }

    [Fact]
    public void Solve_WithEmptyInput_ReturnsEmptyOutput()
    {
        var input = new string[,] { };
        var sut = new DependencyGraphSolver(input, null);
        var actualResult = sut.Solve();

        Assert.Empty(actualResult);
    }

    [Fact]
    public void Solve_With2Inputs_ReturnsOutputInSeparateLines()
    {
        var input = new string[,] { {"A", "B" } };
        var expectedList = new List<List<string>>
        {
            new List<string> {"A"},
            new List<string> {"B"}
        };

        var sut = new DependencyGraphSolver(input, null);
        var actualResult = sut.Solve();

        Assert.Equal(expectedList, actualResult);
    }

}
