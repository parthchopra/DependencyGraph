using System;
namespace DependencyGraph
{
	public interface ILogger
	{
        public void Write(string message);
        public void WriteSuccess(string message);
        public void WriteError(string message);
    }
}

