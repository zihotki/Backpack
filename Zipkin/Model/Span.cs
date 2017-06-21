using System;
using BackpackCore;
using Zipkin.Constants;

namespace Zipkin.Model
{
	/// <summary>
	/// A trace is a series of spans (often RPC calls) which form a latency tree.
	/// </summary>
	/// 
	/// <remarks>
	/// Spans are usually created by instrumentation in RPC clients or servers, but can also
	/// represent in-process activity. Annotations in spans are similar to log statements, and are
	/// sometimes created directly by application developers to indicate events of interest, such as a
	/// cache miss.
	/// 
	/// <para>The root span is where {@link #parentId} is null; it usually has the longest {@link #duration} in the trace.</para>
	/// 
	/// <para>Span identifiers are packed into longs, but should be treated opaquely. String encoding is 
	/// </para>
	/// </remarks>
	public class Span
	{
		/// <summary>
		/// Unique 16-byte identifier for a trace, set on all spans within it.
		/// </summary>
		public Guid TraceId;

		/// <summary>
		/// Span name in lowercase, rpc method for example.
		/// </summary>
		public string Name;

		/// <summary>
		/// Unique 8-byte identifier of this span within a trace.
		/// </summary>
		public long Id;

		/// <summary>
		/// The parent's {@link #id} or null if this the root span in a trace.
		/// </summary>
		public long? ParentId;

		/// <summary>
		/// Epoch microseconds of the start of this span.
		/// </summary>
		/// 
		/// <remarks>
		/// This value should be set directly by instrumentation, using the most precise value
		/// possible.For example, {@code gettimeofday} or multiplying  	{ @link System#currentTimeMillis} by 1000.
		/// 
		/// <para>
		/// For compatibility with instrumentation that precede this field, collectors or span stores
		/// can derive this via Annotation.timestamp. 
		/// </para>
		/// 
		/// <para>
		/// For example, {@link Constants#SERVER_RECV}.timestamp or {@link Constants#CLIENT_SEND}.timestamp.
		/// </para>
		/// 
		/// <para> 
		/// Timestamp is nullable for input only. Spans without a timestamp cannot be presented in a
		/// timeline: Span stores should not output spans missing a timestamp.
		/// </para>
		/// 
		/// <para>
		/// There are two known edge-cases where this could be absent: both cases exist when a
		/// collector receives a span in parts and a binary annotation precedes a timestamp. This is possible when.
		/// </para>
		/// 
		/// * The span is in-flight (ex not yet received a timestamp) 
		/// * The span's start event was lost
		/// </remarks>
		public long TimestampInUnixMicroseconds;


		public SpanType SpanType;

		/// <summary>
		/// Measurement in microseconds of the critical path, if known. 
		/// Durations of less than one microsecond must be rounded up to 1 microsecond.
		/// </summary>
		/// 
		/// <remarks>
		/// This value should be set directly, as opposed to implicitly via annotation timestamps. 
		/// Doing so encourages precision decoupled from problems of clocks, such as skew or 
		/// NTP updates causing time to move backwards.
		/// </remarks>
		public long DurationInMicroseconds;

		/// <summary>
		/// True is a request to store this span even if it overrides sampling policy.
		/// </summary>
		public bool IsDebug;

		public BackpackItem[] BinaryAnnotations;

		// If any of these is not set then it's not a valid span
		public bool IsValid => TraceId != default(Guid)
		                       && Id != default(long)
		                       && !string.IsNullOrEmpty(Name);
	}
}