using System;
using System.Collections.Generic;
using System.Net.Http;
using Zipkin.XB3Propagation;

namespace Zipkin.Http
{
    public static class HttpRequestMessageExtensions
    {
	    public static void EnableZipkinTrace(this HttpRequestMessage requestMessage)
	    {
		    var headers = requestMessage.Headers;

		    var zipkinHeaders = new Dictionary<string, string>(8);
			zipkinHeaders.WriteX3BHeaders();

		    if (zipkinHeaders.Count == 0)
		    {
			    return;
		    }

		    foreach (var zipkinHeader in zipkinHeaders)
		    {
			    headers.Add(zipkinHeader.Key, zipkinHeader.Value);
		    }
	    }
    }
}
