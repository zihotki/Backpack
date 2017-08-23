using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;

using Zipkin.Tracers;
using Zipkin.XB3Propagation;

namespace Zipkin.AspNetCore
{
	public static class ZipkinApplicationBuilderExtensions
	{
		public static void UseZipkin(this IApplicationBuilder app, string requestTraceName)
		{
			app.Use((context, next) =>
			{
				var zipkinHeaders = context.Request.Headers.Where(x => XB3Propagation.Constants.KEYS.Any(k => string.Equals(x.Key, k, StringComparison.OrdinalIgnoreCase)))
					.ToDictionary(x => x.Key, x => x.Value.FirstOrDefault());

				var traceInfo = zipkinHeaders.LoadTraceInfo();
			
				var clientTrace = new ServerBackpackTrace(requestTraceName, traceInfo);

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