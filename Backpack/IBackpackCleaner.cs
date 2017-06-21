using System;

namespace BackpackCore
{
	public interface IBackpackCleaner
	{
		void Cleanup(BackpackScope scope, Exception e);
	}
}