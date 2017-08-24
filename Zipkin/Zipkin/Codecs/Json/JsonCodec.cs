using System.Collections.Generic;
using System.IO;
using System.Text;
using BackpackCore;
using Zipkin.Codecs.Json;
using Zipkin.Constants;
using Zipkin.Model;

namespace Zipkin.Codecs
{
	public class JsonCodec : Codec
	{
		public override void WriteSpans(IList<Span> spans, Stream stream)
		{
			using (var w = new StreamWriter(stream, Encoding.UTF8, 128, leaveOpen: true))
			{
				w.Write("[");
				var firstSpan = true;
				foreach (var span in spans)
				{
					if (span == null)
					{
						continue;
					}
					
					if (!firstSpan)
					{
						w.Write(",");
					}

					firstSpan = false;

					InternalWriteSpan(w, span);
				}
				w.Write("]");
			}
		}

		public override void WriteSpan(Span span, Stream stream)
		{
			if (span == null)
			{
				return;
			}

			using (var w = new StreamWriter(stream, Encoding.UTF8, 128, leaveOpen: true))
			{
				InternalWriteSpan(w, span);
			}
		}

		private void InternalWriteSpan(StreamWriter w, Span span)
		{
			w.Write("{\"traceId\":\"");
			w.Write(span.TraceId.ToString("N"));
			w.Write("\",\"id\":\"");
			w.WriteLowerHex(span.Id);
			w.Write("\",\"name\":\"");
			w.WriteJsonEscaped(span.Name);
			w.Write('"');

			if (span.ParentId.HasValue && span.ParentId != default(long))
			{
				w.Write(",\"parentId\":\"");
				w.WriteLowerHex(span.ParentId.Value);
				w.Write('"');
			}

			/*w.Write(",\"timestamp\":");
			w.Write(span.TimestampInUnixMicroseconds);

			w.Write(",\"duration\":");
			w.Write(span.DurationInMicroseconds);
			*/

			w.Write(",\"annotations\":");
			WriteAnnotations(span, w);

			/*
			if (span.BinaryAnnotations != null && span.BinaryAnnotations.Length != 0)
			{
				w.Write(",\"binaryAnnotations\":");
				WriteBinaryAnnotations(span, w);
			}
			*/

			if (span.IsDebug)
			{
				w.Write(",\"debug\":true");
			}

			w.Write('}');
		}

		private static void WriteBinaryAnnotations(Span span, StreamWriter w)
		{
			w.Write("[");

			var appendComma = false;

			if (span.SpanType == SpanType.Local)
			{
				// append lc bin annotation

				appendComma = true;
				w.Write("{\"key\":\"");
				w.Write(StandardAnnotationKeys.LocalComponent);
				w.Write("\",\"value\":\"");
				w.WriteJsonEscaped(span.Name);
				w.Write("\"}");
			}

			foreach (var annotation in span.BinaryAnnotations)
			{
				if (appendComma)
				{
					w.Write(",");
				}
				else
				{
					appendComma = true;
				}

				w.Write("{\"key\":\"");
				w.WriteJsonEscaped(annotation.Name);
				w.Write("\",\"value\":");

				switch (annotation.ItemType)
				{
					case ItemType.Bool:
						w.Write(annotation.BoolValue == true ? "true" : "false");
						break;
					case ItemType.String:
						w.Write('"');
						w.WriteJsonEscaped(annotation.StringValue);
						w.Write('"');
						break;
					/*case itemt:
						w.Write('"');
						w.WriteBase64Url(annotation.ValAsBArray);
						w.Write('"');
						break;*/
					case ItemType.Byte:
						w.Write(annotation.ByteValue.Value.ToString());
						break;
					case ItemType.Short:
						w.Write(annotation.ShortValue.Value.ToString());
						break;
					case ItemType.Int:
						w.Write(annotation.IntValue.Value.ToString());
						break;
					case ItemType.Long:
						w.Write(annotation.LongValue.Value.ToString());
						break;
					case ItemType.Guid:
						w.Write('"');
						w.Write(annotation.GuidValue.Value.ToString("N"));
						w.Write('"');
						break;


					/*case BinaryAnnotationType.DOUBLE:
						var d = annotation.ValAsDouble;
						// double wrapped = Double.longBitsToDouble(ByteBuffer.wrap(value.value).getLong());
						w.Write(d.ToString(CultureInfo.InvariantCulture));
						break;*/
				}

				w.Write('}');
			}
			w.Write("]");
		}

		private static void WriteAnnotations(Span span, StreamWriter w)
		{
			string startTag, endTag;
			if (span.SpanType == SpanType.Client || span.SpanType == SpanType.Local)
			{
				startTag = StandardAnnotationKeys.ClientSend;
				endTag = StandardAnnotationKeys.ClientRecv;
			}
			else
			{
				startTag = StandardAnnotationKeys.ServerRecv;
				endTag = StandardAnnotationKeys.ServerSend;
			}

			w.Write("[{\"timestamp\":");
			w.Write(span.TimestampInUnixMicroseconds);

			w.Write(",\"value\":\"");
			w.Write(startTag);
			w.Write("\",\"endpoint\":");
			WriteHost(w);
			w.Write('}');

			w.Write(",{\"timestamp\":");
			w.Write((span.TimestampInUnixMicroseconds + span.DurationInMicroseconds));
			w.Write(",\"value\":\"");
			w.Write(endTag);
			w.Write("\",\"endpoint\":");
			WriteHost(w);

			w.Write("}]");
		}

		private static void WriteHost(StreamWriter w)
		{
			var endpoint = ZipkinConfig.ThisService;

			w.Write("{\"serviceName\":\"");
			w.WriteJsonEscaped(endpoint.ServiceName);
			w.Write('"');

			if (endpoint.IPAddress != null)
			{
				var ipv4 = endpoint.IPAddress.GetAddressBytes();
				if (ipv4.Length != 0)
				{
					w.Write(",\"ipv4\":\"");
					w.Write(ipv4[0]);
					w.Write('.');
					w.Write(ipv4[1]);
					w.Write('.');
					w.Write(ipv4[2]);
					w.Write('.');
					w.Write(ipv4[3]);
					w.Write('"');
				}
			}

			if (endpoint.Port.HasValue && endpoint.Port != 0)
			{
				w.Write(",\"port\":");
				w.Write(endpoint.Port.Value);
			}

			w.Write('}');
		}
	}
}