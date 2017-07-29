using EasyNetQ.Interception;
using Zipkin.XB3Propagation;

namespace Zipkin.EasyNetQ
{
	public class ZipkinTraceInterceptor : IProduceConsumeInterceptor
	{
		public RawMessage OnProduce(RawMessage rawMessage)
		{
			rawMessage.Properties.Headers.WriteX3BHeaders();
			
			return rawMessage;
		}

		public RawMessage OnConsume(RawMessage rawMessage)
		{
			var traceInfo = rawMessage.Properties.Headers.LoadTraceInfo();
			TraceInfoUtil.TraceInfo = traceInfo;

			return rawMessage;
		}
	}
}