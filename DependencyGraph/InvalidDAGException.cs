using System;
namespace DependencyGraph
{
	public class InvalidDAGException : Exception
	{
		public InvalidDAGException()
		{
		}

		public InvalidDAGException(string message): base(message)
		{

		}

        public InvalidDAGException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}

