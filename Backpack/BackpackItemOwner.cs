using System;

namespace BackpackCore
{ 
	[Flags]
	public enum BackpackItemOwner
	{
		Shared = 0,
		Zipkin = 1,
		Serilog = 2
	}
}