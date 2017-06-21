using System.Collections.Generic;
using System.IO;
using Zipkin.Model;

namespace Zipkin.Codecs
{
	public abstract class Codec
	{
		public abstract void WriteSpans(IList<Span> spans, Stream stream);

		public abstract void WriteSpan(Span span, Stream stream);
	}
}