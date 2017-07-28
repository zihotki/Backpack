using System;
using System.Collections.Generic;
using BackpackCore;
using EasyNetQ.Interception;

namespace Zipkin.EasyNetQ
{
	public class ZipkinTraceInterceptor : IProduceConsumeInterceptor
	{
		public RawMessage OnProduce(RawMessage rawMessage)
		{
			

			return rawMessage;
		}

		public RawMessage OnConsume(RawMessage rawMessage)
		{
			PopulateTraceInfo(rawMessage);

			return rawMessage;
		}

		private static void PopulateTraceInfo(RawMessage rawMessage)
		{
			if (!rawMessage.Properties.HeadersPresent)
			{
				return;
			}

			var headers = rawMessage.Properties.Headers;

			var traceId = ExtractGuid(headers, Headers.TRACE_ID);
			var parentSpanId = ExtractLong(headers, Headers.PARENT_SPAN_ID);
			var spanId = ExtractLong(headers, Headers.SPAN_ID);
			var isSampled = ExtractBool(headers, Headers.SAMPLED);
			var isDebug = ExtractBool(headers, Headers.FLAGS);

			if (traceId != null && spanId != null)
			{
				TraceInfoUtil.TraceInfo = new TraceInfo(traceId.Value, spanId.Value, parentSpanId, isSampled, isDebug);
			}
		}

		private static bool? ExtractBool(IDictionary<string, object> headers, string headerName)
		{
			if (headers.ContainsKey(headerName) == false)
			{
				return null;
			}

			var boolObj = headers[headerName];
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

			var longObj = headers[headerName];
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

			var guidObj = headers[headerName];
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
	}
}