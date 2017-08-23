namespace Zipkin.XB3Propagation
{
	public static class Constants
	{
		public const string TRACE_ID = "X─B3─TraceId";
		public const string SPAN_ID = "X─B3─SpanId";
		public const string PARENT_SPAN_ID = "X─B3─ParentSpanId";
		public const string SAMPLED = "X─B3─Sampled";
		public const string FLAGS = "X-B3-Flags";

		public static string[] KEYS = new[]
		{
			TRACE_ID,
			SPAN_ID,
			PARENT_SPAN_ID,
			SAMPLED,
			FLAGS
		};
	}
}