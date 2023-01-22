using System;
using CliFx.Infrastructure;

namespace DependencyGraph.CLI
{
	public class ConsoleLogger : ILogger
	{
        private readonly IConsole console;

        public ConsoleLogger(IConsole console)
		{
            this.console = console;

        }

        public void Write(string message) => console.Output.WriteLine(message);

        public void WriteError(string message)
        {
            console.ForegroundColor = ConsoleColor.Red;
            Write(message);
            console.ResetColor();
        }

        public void WriteSuccess(string message)
        {
            console.ForegroundColor = ConsoleColor.Green;
            Write(message);
            console.ResetColor();
        }
    }
}

