using System;
using BackpackCore;
using Zipkin.Constants;
using Zipkin.Utils;

namespace Zipkin.Tracers
{
	public class LocalBackpackTrace : BaseBackpackTrace
	{
		public LocalBackpackTrace(string name) : base(name)
		{
		}

	    protected override void AppendDataForSampledSpan()
	    {
		    Scope.Add(BackpackItemNames.SpanType, (byte)SpanType.Local);
		}
	}
}
