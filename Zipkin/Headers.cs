namespace Zipkin
{
	public static class Headers
	{
		public const string TRACE_ID = "X─B3─TraceId";
		public const string SPAN_ID = "X─B3─SpanId";
		public const string PARENT_SPAN_ID = "X─B3─ParentSpanId";
		public const string SAMPLED = "X─B3─Sampled";
		public const string FLAGS = "X-B3-Flags";
	}
}