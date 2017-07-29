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
			var parentSpanId = Backpack.Get(BackpackConstants.SpanId, default(long));

			// isdebug should be also set somewhere at the root
			var isDebug = Scope.Get(BackpackConstants.IsDebug, false);
			// sampling should be also set at the root from parent scope or decided if it's a root scope
			var isSampled = Scope.Get(BackpackConstants.IsSampled, false);

			if (isSampled || isDebug)
			{
				Scope.Add(BackpackConstants.SpanName, name);
				// for child traces it should come from backpack itself
				// for root it should be set by infrastructure
				// Backpack.Add(ZipkinItems.TraceId, "");
				// span id is always unique
				Scope.Add(BackpackConstants.SpanId, RandomHelper.NewId());

				if (parentSpanId != default(long))
				{
					Scope.Add(BackpackConstants.ParentSpanId, parentSpanId);
				}

				Scope.Add(BackpackConstants.SpanStartInUnixTimeMicro, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
					isHidden: true);
				Scope.Add(BackpackConstants.SpanStartInTicks, TickClock.Start(),
					isHidden: true);

				// The function should be used only to populate data for the span into Backpack
				Scope.Add(BackpackConstants.SpanType, (byte)SpanType.Local);
			}
		}
	}
}
