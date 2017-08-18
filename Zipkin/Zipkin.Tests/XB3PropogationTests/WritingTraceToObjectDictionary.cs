using System;
using System.Collections.Generic;
using BackpackCore;
using Xunit;
using Zipkin.Constants;
using Zipkin.XB3Propagation;

namespace Zipkin.Tests.XB3PropogationTests
{
	public class WritingTraceToObjectDictionary : IDisposable
	{
		public WritingTraceToObjectDictionary()
		{
			Backpack.Reset();
			Backpack.CreateScope();
		}

		[Fact]
		public void Should_Not_Write_Anything_When_No_Trace()
		{
			var container = new Dictionary<string, object>();

			container.WriteX3BHeaders();

			container.Count.ShouldBe(0);
		}

		[Fact]
		public void Should_Write_TraceInfo_When_TraceAndSpan_Present()
		{
			var traceId = Guid.NewGuid();
			var spanId = 123L;

			Backpack.Add(BackpackConstants.TraceId, traceId);
			Backpack.Add(BackpackConstants.SpanId, spanId);
			var container = new Dictionary<string, object>();

			container.WriteX3BHeaders();

			container.Count.ShouldBe(2);
			container[XB3Propagation.Constants.TRACE_ID].ShouldBe(traceId);
			container[XB3Propagation.Constants.SPAN_ID].ShouldBe(spanId);
		}


		public void Dispose()
		{
			Backpack.Reset();
		}
	}
}