﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Zipkin.Model;

namespace Zipkin.Dispatchers
{
    public class VoidDispatcher : SpanDispatcher
    {
	    public override Task DispatchSpans(IList<Span> spans)
	    {
		    return Task.CompletedTask;
	    }

	    public override void Dispose()
	    {
		    
	    }
    }
}
