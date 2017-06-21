using System;

namespace BackpackCore
{
	public class BackpackScopeDisposedException : Exception
	{
		public BackpackScopeDisposedException()
		{
		}

		public BackpackScopeDisposedException(string message) : base(message)
		{
		}

		public BackpackScopeDisposedException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}