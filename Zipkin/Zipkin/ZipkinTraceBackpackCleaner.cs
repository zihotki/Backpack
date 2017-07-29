using System;
using System.Linq;
using BackpackCore;
using Zipkin.Constants;
using Zipkin.Model;
using Zipkin.Tracers;
using Zipkin.Utils;

namespace Zipkin
{
	public class ZipkinTraceBackpackCleaner : IBackpackCleaner
	{
		public void Cleanup(BackpackScope scope, Exception e)
		{
			// IsDebug should be also set somewhere at the root
			var isDebug = scope.Get(BackpackConstants.IsDebug, false);
			// sampling should be also set at the root from parent scope or decided if it's a root scope
			var isSampled = scope.Get(BackpackConstants.IsSampled, false);

			if (isSampled || isDebug)
			{
				var span = new Span
				{
					Id = scope.GetLocal(BackpackConstants.SpanId, default(long)),
					Name = scope.GetLocal(BackpackConstants.SpanName, string.Empty),

					TraceId = scope.Get(BackpackConstants.TraceId, Guid.Empty),

					ParentId = scope.GetLocal(BackpackConstants.ParentSpanId, default(long)),
					SpanType = (SpanType) scope.GetLocal(BackpackConstants.SpanType, default(byte))
				};

				if (!span.IsValid)
				{
					return;
				}

				var startTicks = scope.GetLocal(BackpackConstants.SpanStartInTicks, default(long));
				var durationInMs = TickClock.GetDuration(startTicks);

				span.DurationInMicroseconds = durationInMs;
				span.TimestampInUnixMicroseconds = scope.GetLocal(BackpackConstants.SpanStartInUnixTimeMicro, default(long));

				span.IsDebug = isDebug;

				span.BinaryAnnotations = scope.GetUnique().ToArray();

				ZipkinConfig.Record(span);
			}
		}
	}
}
