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
			if (traceId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(traceId), "Argument can not be empty");
			}

			if (spanId == default(long))
			{
				throw new ArgumentNullException(nameof(spanId), "Argument can not be empty");
			}

			TraceId = traceId;
			SpanId = spanId;
			ParentSpanId = parentSpanId;
			IsSampled = isSampled;
			IsDebug = isDebug;
		}
	}
}