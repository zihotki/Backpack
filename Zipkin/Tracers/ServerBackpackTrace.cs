using BackpackCore;
using Zipkin.Constants;

namespace Zipkin.Tracers
{
	public sealed class ServerBackpackTrace : BaseBackpackTrace
	{
		public ServerBackpackTrace(string name) : base(name)
		{
		}

		protected override void AppendDataForSampledSpan()
		{
			Scope.Add(BackpackItemNames.SpanType, (byte)SpanType.Server);
		}
	}
}