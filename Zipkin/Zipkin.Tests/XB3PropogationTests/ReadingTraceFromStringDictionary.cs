﻿using System;
using System.Collections.Generic;
using BackpackCore;
using Xunit;
using Zipkin.XB3Propagation;

namespace Zipkin.Tests.XB3PropogationTests
{
	public class ReadingTraceFromStringDictionary : IDisposable
	{
		public ReadingTraceFromStringDictionary()
		{
			Backpack.Reset();
			Backpack.CreateScope();
		}

		[Fact]
		public void Should_Return_Null_TraceInfo_When_No_Trace_Data()
		{
			var container = new Dictionary<string, string>();

			var traceInfo = container.LoadTraceInfo();

			traceInfo.ShouldBeNull();
		}

		[Fact]
		public void Should_Load_TraceInfo_When_TraceAndSpan_Present()
		{
			var traceId = Guid.NewGuid();
			var spanId = 123L;
			var container = new Dictionary<string, string>
			{
				{XB3Propagation.Constants.TRACE_ID, traceId.ToString("N")},
				{XB3Propagation.Constants.SPAN_ID, spanId.ToString()}
			};

			var traceInfo = container.LoadTraceInfo();

			traceInfo.TraceId.ShouldBe(traceId);
			traceInfo.SpanId.ShouldBe(spanId);

			traceInfo.IsDebug.ShouldBeNull();
			traceInfo.IsSampled.ShouldBeNull();
			traceInfo.ParentSpanId.ShouldBeNull();
		}

		public void Dispose()
		{
			Backpack.Reset();
		}
	}
}