using System;
using BackpackCore;

namespace Zipkin.Tracers
{
	public abstract class BaseBackpackTrace 
    {
		protected readonly BackpackScope Scope;

	    public BackpackScope TraceScope => Scope;

	    protected BaseBackpackTrace(string name)
	    {
		    Scope = Backpack.CreateScope(name);
	    }

		public void Finish(Exception e = null)
		{
			Scope.Clear(e);
	    }
    }
}
