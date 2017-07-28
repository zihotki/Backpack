using System.Threading;

namespace Zipkin.EasyNetQ
{
	public class TraceInfoUtil
	{
		private static readonly AsyncLocal<TraceInfo> TraceInfoLocal = new AsyncLocal<TraceInfo>();

		public static TraceInfo TraceInfo
		{
			get => TraceInfoLocal.Value;
			set => TraceInfoLocal.Value = value;
		}
	}
}