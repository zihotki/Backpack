﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using BackpackCore;
using Zipkin.Constants;

namespace Zipkin.XB3Propagation
{
    public static class PropagationExtensions
    {
	    #region IDictionary<string, object>

	    public static void WriteX3BHeaders(this IDictionary<string, object> headers)
	    {
		    var traceInfo = ReadTraceInfo();
		    if (traceInfo == null)
		    {
			    return;
		    }

		    headers[Constants.TRACE_ID] = traceInfo.TraceId;
		    headers[Constants.SPAN_ID] = traceInfo.SpanId;

		    if (traceInfo.ParentSpanId.HasValue)
		    {
			    headers[Constants.PARENT_SPAN_ID] = traceInfo.ParentSpanId;
		    }

		    if (traceInfo.IsSampled.HasValue)
		    {
			    headers[Constants.SAMPLED] = traceInfo.IsSampled;
		    }

		    if (traceInfo.IsDebug.HasValue)
		    {
			    headers[Constants.FLAGS] = traceInfo.IsDebug == true ? 1 : 0;
		    }
	    }

		public static TraceInfo LoadTraceInfo(this IDictionary<string, object> headers)
	    {
		    if (headers == null)
		    {
			    return null;
		    }

		    var traceId = ExtractGuid(headers, Constants.TRACE_ID);
		    var parentSpanId = ExtractLong(headers, Constants.PARENT_SPAN_ID);
		    var spanId = ExtractLong(headers, Constants.SPAN_ID);
		    var isSampled = ExtractBool(headers, Constants.SAMPLED);
		    var isDebug = ExtractBool(headers, Constants.FLAGS);

		    if (traceId != null && spanId != null)
		    {
			    return new TraceInfo(traceId.Value, spanId.Value, parentSpanId, isSampled, isDebug);
		    }

		    return null;
	    }

	    private static bool? ExtractBool(IDictionary<string, object> headers, string headerName)
	    {
		    if (headers.ContainsKey(headerName) == false)
		    {
			    return null;
		    }

		    var boolObj = GetHeader(headers, headerName);
		    if (boolObj is string boolStr)
		    {
			    if (bool.TryParse(boolStr, out var boolParsed))
			    {
				    return boolParsed;
			    }

			    return null;
		    }

		    if (boolObj is bool @bool)
		    {
			    return @bool;
		    }

		    if (boolObj is byte boolByte)
		    {
			    return boolByte == 1;
		    }

		    if (boolObj is short boolShort)
		    {
			    return boolShort == 1;
		    }

		    if (boolObj is int boolInt)
		    {
			    return boolInt == 1;
		    }

		    return null;
	    }

	    private static long? ExtractLong(IDictionary<string, object> headers, string headerName)
	    {
		    if (headers.ContainsKey(headerName) == false)
		    {
			    return null;
		    }

		    var longObj = GetHeader(headers, headerName);
		    if (longObj is string longString)
		    {
			    if (long.TryParse(longString, out var longParsed))
			    {
				    return longParsed;
			    }

			    return null;
		    }

		    if (longObj is long @long)
		    {
			    return @long;
		    }

		    return null;
	    }

	    private static Guid? ExtractGuid(IDictionary<string, object> headers, string headerName)
	    {
		    if (headers.ContainsKey(headerName) == false)
		    {
			    return null;
		    }

		    var guidObj = GetHeader(headers, headerName);
		    if (guidObj is string guidString)
		    {
			    if (Guid.TryParse(guidString, out var parsedGuid))
			    {
				    return parsedGuid;
			    }

			    return null;
		    }

		    if (guidObj is Guid guid)
		    {
			    return guid;
		    }

		    return null;
	    }

	    private static object GetHeader(IDictionary<string, object> headers, string headerName)
	    {
		    var header = headers[headerName];
		    if (header is byte[] headerBytes)
		    {
			    return Encoding.UTF8.GetString(headerBytes);
		    }

		    return header;
	    }




		#endregion

		#region IDictionary<string, string>

		public static void WriteX3BHeaders(this IDictionary<string, string> headers)
	    {
		    var traceInfo = ReadTraceInfo();
		    if (traceInfo == null)
		    {
			    return;
		    }

		    headers[Constants.TRACE_ID] = traceInfo.TraceId.ToString("N");
		    headers[Constants.SPAN_ID] = traceInfo.SpanId.ToString(CultureInfo.InvariantCulture);

		    if (traceInfo.ParentSpanId.HasValue)
		    {
			    headers[Constants.PARENT_SPAN_ID] = traceInfo.ParentSpanId.Value.ToString(CultureInfo.InvariantCulture);
		    }

		    if (traceInfo.IsSampled.HasValue)
		    {
			    headers[Constants.SAMPLED] = traceInfo.IsSampled.Value ? "true" : "false";
		    }

		    if (traceInfo.IsDebug.HasValue)
		    {
			    headers[Constants.FLAGS] = traceInfo.IsDebug == true ? "1" : "0";
		    }
	    }

	    public static TraceInfo LoadTraceInfo(this IDictionary<string, string> headers)
	    {
		    if (headers == null)
		    {
			    return null;
		    }

		    var traceId = ExtractGuid(headers, Constants.TRACE_ID);
		    var parentSpanId = ExtractLong(headers, Constants.PARENT_SPAN_ID);
		    var spanId = ExtractLong(headers, Constants.SPAN_ID);
		    var isSampled = ExtractBool(headers, Constants.SAMPLED);
		    var isDebug = ExtractBool(headers, Constants.FLAGS);

		    if (traceId != null && spanId != null)
		    {
			    return new TraceInfo(traceId.Value, spanId.Value, parentSpanId, isSampled, isDebug);
		    }

		    return null;
	    }

	    private static bool? ExtractBool(IDictionary<string, string> headers, string headerName)
	    {
		    if (headers.ContainsKey(headerName) == false)
		    {
			    return null;
		    }

		    var boolStr = headers[headerName];

		    if (boolStr == "1")
		    {
			    return true;
		    }

		    if (boolStr == "0" || boolStr == "-1")
		    {
			    return false;
		    }

		    if (bool.TryParse(boolStr, out var boolParsed))
		    {
			    return boolParsed;
		    }

		    return null;
	    }

	    private static long? ExtractLong(IDictionary<string, string> headers, string headerName)
	    {
		    if (headers.ContainsKey(headerName) == false)
		    {
			    return null;
		    }

		    var longString = headers[headerName];
		    if (long.TryParse(longString, out var longParsed))
		    {
			    return longParsed;
		    }

		    return null;
	    }

	    private static Guid? ExtractGuid(IDictionary<string, string> headers, string headerName)
	    {
		    if (headers.ContainsKey(headerName) == false)
		    {
			    return null;
		    }

		    var guidString = headers[headerName];
		    if (Guid.TryParse(guidString, out var parsedGuid))
		    {
			    return parsedGuid;
		    }

		    return null;
	    }

	    #endregion

		private static TraceInfo ReadTraceInfo()
	    {
		    var traceItem = Backpack.Get(BackpackConstants.TraceId);
		    if (traceItem?.GuidValue == null)
		    {
			    return null;
		    }
		    var spanItem = Backpack.Get(BackpackConstants.SpanId);
		    if (spanItem?.LongValue == null)
		    {
			    return null;
		    }

		    var parentSpanItem = Backpack.Get(BackpackConstants.ParentSpanId);

		    var sampledItem = Backpack.Get(BackpackConstants.IsSampled);
		    var debugItem = Backpack.Get(BackpackConstants.IsDebug);

		    return new TraceInfo(traceItem.GuidValue.Value,
			    spanItem.LongValue.Value,
			    parentSpanItem?.LongValue,
			    sampledItem?.BoolValue,
			    debugItem?.BoolValue);
	    }
	}
}
