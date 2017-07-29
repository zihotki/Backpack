﻿namespace Zipkin.Constants
{
	public static class StandardAnnotationKeys
	{
		/// <summary>
		/// The {BinaryAnnotation#value} of "lc" is the component or namespace of a local span.
		/// 
		/// Local Component ("lc") supports three key features: flagging, query by service and filtering Span.name by namespace.
		/// </summary>
		public const string LocalComponent = "lc";

		/// <summary>
		/// When in an {Annotation#value}, this indicates when an error occurred. 
		/// When in a {BinaryAnnotation#key}, the value is a human readable message associated with an error.
		/// </summary>
		public const string Error = "error";


		/// <summary>
		/// The client sent ("cs") a request to a server. There is only one send per
		/// span. For example, if there's a transport error, each attempt can be logged
		/// as a WIRE_SEND annotation.
		/// 
		/// If chunking is involved, each chunk could be logged as a separate
		/// CLIENT_SEND_FRAGMENT in the same span.
		/// 
		/// Annotation.host is not the server. It is the host which logged the send
		/// event, almost always the client. When logging CLIENT_SEND, instrumentation
		/// should also log the SERVER_ADDR.
		/// </summary>
		public const string ClientSend = "cs";

		/// <summary>
		/// The client received ("cr") a response from a server. There is only one
		/// receive per span. For example, if duplicate responses were received, each
		/// can be logged as a WIRE_RECV annotation.
		/// 
		/// If chunking is involved, each chunk could be logged as a separate
		/// CLIENT_RECV_FRAGMENT in the same span.
		/// 
		/// Annotation.host is not the server. It is the host which logged the receive
		/// event, almost always the client. The actual endpoint of the server is
		/// recorded separately as SERVER_ADDR when CLIENT_SEND is logged.
		/// </summary>
		public const string ClientRecv = "cr";

		/// <summary>
		/// The server sent ("ss") a response to a client. There is only one response
		/// per span. If there's a transport error, each attempt can be logged as a
		/// WIRE_SEND annotation.
		/// 
		/// Typically, a trace ends with a server send, so the last timestamp of a trace
		/// is often the timestamp of the root span's server send.
		/// 
		/// If chunking is involved, each chunk could be logged as a separate
		/// SERVER_SEND_FRAGMENT in the same span.
		/// 
		/// Annotation.host is not the client. It is the host which logged the send
		/// event, almost always the server. The actual endpoint of the client is
		/// recorded separately as CLIENT_ADDR when SERVER_RECV is logged.
		/// </summary>
		public const string ServerSend = "ss";

		/// <summary>
		/// The server received ("sr") a request from a client. There is only one
		/// request per span.  For example, if duplicate responses were received, each
		/// can be logged as a WIRE_RECV annotation.
		/// 
		/// Typically, a trace starts with a server receive, so the first timestamp of a
		/// trace is often the timestamp of the root span's server receive.
		/// 
		/// If chunking is involved, each chunk could be logged as a separate
		/// SERVER_RECV_FRAGMENT in the same span.
		/// 
		/// Annotation.host is not the client. It is the host which logged the receive
		/// event, almost always the server. When logging SERVER_RECV, instrumentation
		/// should also log the CLIENT_ADDR.
		/// </summary>
		public const string ServerRecv = "sr";

		/* // Currently not used annotations, omitted for clean view
		/// <summary>
		/// Optionally logs an attempt to send a message on the wire. Multiple wire send
		/// events could indicate network retries. A lag between client or server send
		/// and wire send might indicate queuing or processing delay.
		/// </summary>
		public const string WireSend = "ws";
		/// <summary>
		/// Optionally logs an attempt to receive a message from the wire. Multiple wire
		/// receive events could indicate network retries. A lag between wire receive
		/// and client or server receive might indicate queuing or processing delay.
		/// </summary>
		public const string WireRecv = "wr";
		/// <summary>
		/// Optionally logs progress of a (CLIENT_SEND, WIRE_SEND). For example, this
		/// could be one chunk in a chunked request.
		/// </summary>
		public const string ClientSendFragment = "csf";
		/// <summary>
		/// Optionally logs progress of a (CLIENT_RECV, WIRE_RECV). For example, this
		/// could be one chunk in a chunked response.
		/// </summary>
		public const string ClientRecvFragment = "crf";
		/// <summary>
		/// Optionally logs progress of a (SERVER_SEND, WIRE_SEND). For example, this
		/// could be one chunk in a chunked response.
		/// </summary>
		public const string ServerSendFragment = "ssf";
		/// <summary>
		/// Optionally logs progress of a (SERVER_RECV, WIRE_RECV). For example, this
		/// could be one chunk in a chunked request.
		/// </summary>
		public const string ServerRecvFragment = "srf";		
		/// <summary>
		/// Indicates a client address ("ca") in a span. Most likely, there's only one.
		/// Multiple addresses are possible when a client changes its ip or port within
		/// a span.
		/// </summary>
		public const string ClientAddr = "ca";
		/// <summary>
		/// Indicates a server address ("sa") in a span. Most likely, there's only one.
		/// Multiple addresses are possible when a client is redirected, or fails to a
		/// different server ip or port.
		/// </summary>
		public const string ServerAddr = "sa";
		*/
	}
}