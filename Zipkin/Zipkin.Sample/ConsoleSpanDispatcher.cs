using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zipkin.Dispatchers;
using Zipkin.Model;

namespace Zipkin.Sample
{
	public class ConsoleSpanDispatcher : SpanDispatcher
	{
		public override Task DispatchSpans(IList<Span> spans)
		{
			foreach (var span in spans)
			{
				Console.WriteLine($"({span.SpanType}) span {span.Name}/{span.Id} (child of {span.ParentId}) "
				                  + $"started {span.TimestampInUnixMicroseconds} lasted {span.DurationInMicroseconds} "
				                  + $"TraceId {span.TraceId} with {span.BinaryAnnotations.Length} binary items");
			}

			return Task.CompletedTask;
		}

		public override void Dispose()
		{
		}
	}
}
