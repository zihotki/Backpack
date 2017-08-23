using EasyNetQ;
using EasyNetQ.Interception;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zipkin;
using Zipkin.AspNetCore;
using Zipkin.Codecs;
using Zipkin.Dispatchers;
using Zipkin.EasyNetQ;

namespace WebApiServer
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

	        var bootstrap = new ZipkinBootstrapper("ShoppingApiServer");
	        bootstrap
		        .DispatchTo(new VoidSpanDispatcher())
		        .WithSampleRate(1)
		        .Start();
		}

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

	        var bus = RabbitHutch.CreateBus("username=guest;password=guest;host=localhost", r =>
	        {
		        r.EnableInterception(i => i.Add(new ZipkinTraceInterceptor()));
	        });
	        services.AddSingleton(bus);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

	        app.UseZipkin("Incoming Request");
			app.UseMvc();
        }
    }
}
