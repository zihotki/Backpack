using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BackpackCore;
using Zipkin.Http;
using Zipkin.Tracers;

namespace Zipkin.Http
{
	public class ZipkinHttpMessageHandler : DelegatingHandler
	{
		public ZipkinHttpMessageHandler() : base(new HttpClientHandler())
		{
		}

		public ZipkinHttpMessageHandler(HttpMessageHandler innerHandler) : base(innerHandler)
		{
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var clientTrace = new ClientBackpackTrace("Http Client");
			request.EnableZipkinTrace();

			Backpack.Add("RequestUri", request.RequestUri.ToString());

			return base.SendAsync(request, cancellationToken)
				.ContinueWith(x =>
				{
					if (x.IsFaulted)
					{
						clientTrace.Close(x.Exception);
					}
					else
					{
						clientTrace.Close();
					}

					return x.Result;
				}, cancellationToken);
		}
	}
}