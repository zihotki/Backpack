using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BackpackCore;
using Microsoft.AspNetCore.Builder;

using Zipkin.Tracers;

namespace Zipkin.AspNetCore
{
	public static class ZipkinApplicationBuilderExtensions
	{
		public static void UseZipkin(this IApplicationBuilder app, string requestTraceName)
		{
			app.Use((context, next) =>
			{
				var clientTrace = new ServerBackpackTrace(requestTraceName);
				// todo: load trace info from request context  

				return next()
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
						}
					);

			});
		}
	}
}