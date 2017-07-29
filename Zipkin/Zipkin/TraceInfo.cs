using System;

namespace Zipkin
{
	public sealed class TraceInfo
	{
		public Guid TraceId { get; }
		public long SpanId { get; }
		public long? ParentSpanId { get; }
		public bool? IsSampled { get; }
		public bool? IsDebug { get; }

		public TraceInfo(Guid traceId, long spanId, long? parentSpanId,  bool? isSampled, bool? isDebug)
		{
			TraceId = traceId;
			ParentSpanId = parentSpanId;
			SpanId = spanId;
			IsSampled = isSampled;
			IsDebug = isDebug;
		}
	}
}