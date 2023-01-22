using System;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Microsoft.CSharp;

namespace DependencyGraph.CLI
{
    [Command]
	public class DependencyGraphSolverCommand : ICommand
	{
        [CommandOption(shortName: 'f', name: "file", Description = "Input file specifying clothing dependencies")]
        public string InputFile { get; init; } = default!;

        [CommandOption(shortName: 'o', name: "output", Description = "Output file containing the results of dependency graph resolver")]
        public string OutputFile { get; set; } = "output.csv";

        public DependencyGraphSolverCommand()
		{
		}


        public ValueTask ExecuteAsync(IConsole console)
        {
            var logger = new ConsoleLogger(console);
            try
            {
                StreamReader sr = new StreamReader(InputFile);
                string[,] input = File.ReadLines(InputFile).Where(line => line != "").Select(x => x.Split(',')).ToArray().To2D();
                var dependencyGraphResult = new DependencyGraphSolver(input, logger).Solve();

                // ensure directory exists
                var outputDirectory = new FileInfo(OutputFile).Directory?.FullName;
                if(outputDirectory != null && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                using (TextWriter tw = new StreamWriter(OutputFile))
                {
                    foreach (var line in dependencyGraphResult)
                        tw.WriteLine(string.Join(',', line));
                }

            }
            catch(Exception e) when (e is IOException || e is InvalidOperationException)
            {
                logger.WriteError($"Unable to read input file at ${InputFile}. | ${e.Message}");
            }
            catch(InvalidDAGException e)
            {
                logger.WriteError(e.Message);
            }
            catch(Exception e)
            {
                logger.WriteError($"Unknown exception. | ${e.Message}");
            }

            return default;
        }
        
    }
}

