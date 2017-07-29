namespace Zipkin.Constants
{
	public enum SpanType : byte
	{
		// if no span type provided - the span will be local
		Local,
		Server,
		Client
	}
}