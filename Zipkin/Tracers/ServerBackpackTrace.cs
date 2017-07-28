using System;
using BackpackCore;
using Zipkin.Constants;
using Zipkin.Utils;

namespace Zipkin.Tracers
{
	public sealed class ServerBackpackTrace : BaseBackpackTrace
	{
		public ServerBackpackTrace(string name) : base(name)
		{
			var parentSpanId = Backpack.Get(BackpackItemNames.SpanId, default(long));

			// isdebug should be also set somewhere at the root
			var isDebug = Scope.Get(BackpackItemNames.IsDebug, false);
			// sampling should be also set at the root from parent scope or decided if it's a root scope
			var isSampled = Scope.Get(BackpackItemNames.IsSampled, false);

			if (isSampled || isDebug)
			{
				Scope.Add(BackpackItemNames.SpanName, name);
				// for child traces it should come from backpack itself
				// for root it should be set by infrastructure
				// Backpack.Add(ZipkinItems.TraceId, "");
				// span id is always unique
				Scope.Add(BackpackItemNames.SpanId, RandomHelper.NewId());

				if (parentSpanId != default(long))
				{
					Scope.Add(BackpackItemNames.ParentSpanId, parentSpanId);
				}

				Scope.Add(BackpackItemNames.SpanStartInUnixTimeMicro, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
					isHidden: true);
				Scope.Add(BackpackItemNames.SpanStartInTicks, TickClock.Start(),
					isHidden: true);

				// ReSharper disable once VirtualMemberCallInConstructor
				// The function should be used only to populate data for the span into Backpack
				Scope.Add(BackpackItemNames.SpanType, (byte)SpanType.Server);
			}
		}

		public ServerBackpackTrace(string name, TraceInfo traceInfo) : base(name)
		{
			var parentSpanId = Backpack.Get(BackpackItemNames.SpanId, default(long));

			// isdebug should be also set somewhere at the root
			var isDebug = Scope.Get(BackpackItemNames.IsDebug, false);
			// sampling should be also set at the root from parent scope or decided if it's a root scope
			var isSampled = Scope.Get(BackpackItemNames.IsSampled, false);

			if (isSampled || isDebug)
			{
				Scope.Add(BackpackItemNames.SpanName, name);
				// for child traces it should come from backpack itself
				// for root it should be set by infrastructure
				// Backpack.Add(ZipkinItems.TraceId, "");
				// span id is always unique
				Scope.Add(BackpackItemNames.SpanId, RandomHelper.NewId());

				if (parentSpanId != default(long))
				{
					Scope.Add(BackpackItemNames.ParentSpanId, parentSpanId);
				}

				Scope.Add(BackpackItemNames.SpanStartInUnixTimeMicro, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
					isHidden: true);
				Scope.Add(BackpackItemNames.SpanStartInTicks, TickClock.Start(),
					isHidden: true);

				// ReSharper disable once VirtualMemberCallInConstructor
				// The function should be used only to populate data for the span into Backpack
				Scope.Add(BackpackItemNames.SpanType, (byte)SpanType.Server);
			}
		}

		protected override void AppendDataForSampledSpan()
		{
			
		}
	}
}