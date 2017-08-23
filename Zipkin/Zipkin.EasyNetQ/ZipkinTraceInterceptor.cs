using System.Collections.Generic;
using EasyNetQ.Interception;
using Zipkin.XB3Propagation;

namespace Zipkin.EasyNetQ
{
	public class ZipkinTraceInterceptor : IProduceConsumeInterceptor
	{
		public RawMessage OnProduce(RawMessage rawMessage)
		{
			var stringHeaders = new Dictionary<string, string>();
			stringHeaders.WriteX3BHeaders();

			var messageHeaders = rawMessage.Properties.Headers;
			foreach (var stringHeader in stringHeaders)
			{
				messageHeaders.Add(stringHeader.Key, stringHeader.Value);
			}
			
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