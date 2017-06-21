using BackpackCore;
using Zipkin.Constants;

namespace Zipkin.Tracers
{
	public sealed class ClientBackpackTrace : BaseBackpackTrace
	{
		public ClientBackpackTrace(string name) : base(name)
		{
		}

		protected override void AppendDataForSampledSpan()
		{
			Backpack.Add(BackpackItemNames.SpanType, (byte) SpanType.Client);
		}
	}
}