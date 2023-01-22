using System;
namespace DependencyGraph
{
	public class SimpleLogger : ILogger
	{

        public void Write(string message) => Console.WriteLine(message);

        public void WriteError(string message) => Write(message);

        public void WriteSuccess(string message) => Write(message);
    }
}

