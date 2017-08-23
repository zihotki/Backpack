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
			CreateNewServerTrace(name);
		}

		public ServerBackpackTrace(string name, TraceInfo traceInfo) : base(name)
		{
			if (traceInfo == null)
			{
				CreateNewServerTrace(name);
			}
			else
			{
				CreateNewServerTrace(name, traceInfo);
			}
		}

		private void CreateNewServerTrace(string name)
		{
			// According to the Open Zipkin docs, server trace should use SpanId and ParentSpanId 
			// provided from a client if any, otherwise a new SpanId should be generated and ParentSpanId should be null
			long? parentSpanId = null;
			var spanId = Backpack.Get(BackpackConstants.SpanId, default(long));

			if (spanId != default(long))
			{
				var backpackedParentSpanId = Backpack.Get(BackpackConstants.ParentSpanId, default(long));
				if (backpackedParentSpanId != default(long))
				{
					parentSpanId = backpackedParentSpanId;
				}
			}
			else
			{
				spanId = RandomHelper.NewId();
			}

			var traceId = Backpack.Get(BackpackConstants.TraceId, Guid.Empty);
			if (traceId == Guid.Empty)
			{
				traceId = Guid.NewGuid();
			}

			InitTrace(name, spanId, parentSpanId, traceId);

			// if sampling is not provided by callers then we need to decide whether it's sampled
			var isSampledItem = Backpack.Get(BackpackConstants.IsSampled);
			if (isSampledItem == null || isSampledItem.BoolValue.HasValue == false)
			{
				var isSampled = ZipkinConfig.ShouldSample();
				Scope.Add(BackpackConstants.IsSampled, isSampled, isHidden: true);
			}
		}
		
		private void CreateNewServerTrace(string name, TraceInfo traceInfo)
		{
			InitTrace(name, traceInfo.SpanId, traceInfo.ParentSpanId, traceInfo.TraceId);

			if (traceInfo.IsDebug.HasValue)
			{
				Scope.Add(BackpackConstants.IsDebug, traceInfo.IsDebug.Value);
			}

			Scope.Add(BackpackConstants.IsSampled, traceInfo.IsSampled ?? ZipkinConfig.ShouldSample(), isHidden: true);
		}

		private void InitTrace(string name, long spanId, long? parentSpanId, Guid traceId)
		{
			Scope.Add(BackpackConstants.SpanName, name);
			Scope.Add(BackpackConstants.SpanType, (byte)SpanType.Server);

			Scope.Add(BackpackConstants.TraceId, traceId);
			Scope.Add(BackpackConstants.SpanId, spanId);

			if (parentSpanId != null && parentSpanId != default(long))
			{
				Scope.Add(BackpackConstants.ParentSpanId, parentSpanId.Value);
			}

			Scope.Add(BackpackConstants.SpanStartInUnixTimeMicro, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
				isHidden: true);
			Scope.Add(BackpackConstants.SpanStartInTicks, TickClock.Start(),
				isHidden: true);
		}
	}
}