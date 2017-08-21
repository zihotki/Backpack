using System;
using BackpackCore;
using Zipkin.Constants;
using Zipkin.Utils;

namespace Zipkin.Tracers
{
	public sealed class ClientBackpackTrace : BaseBackpackTrace
	{
		public ClientBackpackTrace(string name) : base(name)
		{
			Scope.Add(BackpackConstants.SpanName, name);
			Scope.Add(BackpackConstants.SpanType, (byte)SpanType.Client);
			
			// sampling should be also set at the root from parent scope or decided if it's a root scope
			var isSampled = Scope.Get(BackpackConstants.IsSampled);
			if (isSampled == null || isSampled.BoolValue.HasValue == false)
			{
				Scope.Add(BackpackConstants.IsSampled, ZipkinConfig.ShouldSample(), isHidden: true);
			}

			// We need to get parent span id before generating and adding a new one
			var parentSpanId = Backpack.Get(BackpackConstants.SpanId, default(long));

			Scope.Add(BackpackConstants.SpanId, RandomHelper.NewId());

			if (parentSpanId != default(long))
			{
				Scope.Add(BackpackConstants.ParentSpanId, parentSpanId);
			}

			// if TraceId is not set we need to set it with a new value
			var traceId = Backpack.Get(BackpackConstants.TraceId);
			if (traceId == null || traceId.GuidValue.HasValue == false)
			{
				Scope.Add(BackpackConstants.TraceId, Guid.NewGuid());
			}

			Scope.Add(BackpackConstants.SpanStartInUnixTimeMicro, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
				isHidden: true);
			Scope.Add(BackpackConstants.SpanStartInTicks, TickClock.Start(),
				isHidden: true);
		}
	}
}