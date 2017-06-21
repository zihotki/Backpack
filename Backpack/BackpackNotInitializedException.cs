using System;

namespace BackpackCore
{
	public class BackpackNotInitializedException : Exception
	{
		public BackpackNotInitializedException()
		{
		}

		public BackpackNotInitializedException(string message) : base(message)
		{
		}

		public BackpackNotInitializedException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}