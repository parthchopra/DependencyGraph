using System.Linq;
using System.Text;

namespace DependencyGraph;
public class DependencyGraphSolver
{
    private readonly string[,] input;
    private readonly ILogger logger;

    public DependencyGraphSolver(string[,] input, ILogger? logger)
    {
        this.input = input;
        this.logger = logger ?? new SimpleLogger();
    }

    public List<List<string>> Solve()
    {
        Dictionary<string, HashSet<string>> adjacencyList = ConstructAdjacencyList();
        Queue<(string key, int level)> nodesWithNoDependency = CreateInitialNodesWithNoDependencies(adjacencyList);

        var output = new List<List<string>>();
        var visitedNodes = 0;
        var currentLevelNodes = new List<string>();
        var previousLevel = 0;
        while (nodesWithNoDependency.Count > 0)
        {
            var (currentItem, currentLevel) = nodesWithNoDependency.Dequeue();
            visitedNodes++;

            if (currentLevel > previousLevel)
            {
                AlphabeticallyOrderAndAddCurrentLevelNodesToOutput(currentLevelNodes, output);
                currentLevelNodes = new List<string>();
                previousLevel = currentLevel;
            }
            currentLevelNodes.Add(currentItem);

            var nodesDependingOnCurrentItem = adjacencyList.Where(i => i.Value.Contains(currentItem)).Select(i => i.Key).ToList();
            foreach (var node in nodesDependingOnCurrentItem)
            {
                adjacencyList[node].Remove(currentItem);
                if (adjacencyList[node].Count == 0)
                {
                    nodesWithNoDependency.Enqueue((node, currentLevel + 1));
                }
            }
        }

        AlphabeticallyOrderAndAddCurrentLevelNodesToOutput(currentLevelNodes, output);

        if (visitedNodes != adjacencyList.Keys.Count)
        {
            throw new InvalidDAGException("The input provided does not seem to form a directed acyclic graph, please check your input");
        }
        Console.WriteLine(output);

        return output;
    }

    private Dictionary<string, HashSet<string>> ConstructAdjacencyList()
    {
        var adjacencyList = new Dictionary<string, HashSet<string>>();
        for (var row = 0; row < input.GetLength(0); row++)
        {
            var dependency = input[row, 0];
            var item = input[row, 1];

            if (!adjacencyList.ContainsKey(dependency))
            {
                adjacencyList[dependency] = new HashSet<string>();
            }

            if (adjacencyList.ContainsKey(item))
            {
                adjacencyList[item].Add(dependency);
            }
            else
            {
                adjacencyList[item] = new HashSet<string>() { dependency };
            }
        }

        return adjacencyList;
    }

    private Queue<(string key, int level)> CreateInitialNodesWithNoDependencies(Dictionary<string, HashSet<string>> adjacencyList)
    {
        var nodesWithNoDependency = new Queue<(string key, int level)>();

        foreach (var key in adjacencyList.Keys)
        {
            var level = 0;
            if (adjacencyList[key].Count == 0)
                nodesWithNoDependency.Enqueue((key, level));
        }

        return nodesWithNoDependency;
    }

    private void AlphabeticallyOrderAndAddCurrentLevelNodesToOutput(List<string> currentLevelNodes, List<List<string>> output)
    {
        if(currentLevelNodes.Count > 0)
        {
            var orderedNodes = currentLevelNodes.OrderBy(x => x).ToList();
            logger.WriteSuccess(string.Join(',', orderedNodes));
            output.Add(orderedNodes);
        }
    }
}

