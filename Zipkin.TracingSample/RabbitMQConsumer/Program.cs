using System;
using System.Threading;
using EasyNetQ;
using EasyNetQ.Interception;
using Zipkin;
using Zipkin.Dispatchers;
using Zipkin.EasyNetQ;
using Zipkin.Tracers;
using SampleMessages;

namespace RabbitMQConsumer
{
    class Program
    {
	    static void Main(string[] args)
	    {
		    var bootstrap = new ZipkinBootstrapper("Order Service");
		    bootstrap
			    .DispatchTo(new ConsoleSpanDispatcher())
			    .WithSampleRate(1)
			    .Start();

			using (var bus = RabbitHutch.CreateBus("username=guest;password=guest;host=localhost", r =>
		    {
			    r.EnableInterception(i => i.Add(new ZipkinTraceInterceptor()));
		    }))
		    {
			    bus.Subscribe<CreateOrderMessage>("order", HandleOrderMessage);
			    bus.Subscribe<CreatePriorityOrderMessage>("priority-order", HandlePriorityOrderMessage);


			    Console.WriteLine("Listening for messages. Press Ctrl+c to exit.");

			    while (true)
			    {
				    Console.ReadLine();
			    }
		    }
	    }

	    static void HandleOrderMessage(CreateOrderMessage orderMessage)
	    {
		    var traceInfo = TraceInfoUtil.TraceInfo;
		    var trace = new ServerBackpackTrace("CreateOrder Handler", traceInfo);

		    try
		    {
				Console.WriteLine($"Order {orderMessage.OrderId} received");
			    Thread.Sleep(5_000);

				trace.Close();
		    }
		    catch (Exception e)
		    {
			    trace.Close(e);
			    throw;
		    }
	    }

	    static void HandlePriorityOrderMessage(CreatePriorityOrderMessage orderMessage)
	    {
			var traceInfo = TraceInfoUtil.TraceInfo;
		    var trace = new ServerBackpackTrace("CreatePriorityOrder Handler", traceInfo);

		    try
		    {
			    Console.WriteLine($"Priority order {orderMessage.OrderId} received");
			    Thread.Sleep(5_000);

			    trace.Close();
		    }
		    catch (Exception e)
		    {
			    trace.Close(e);
			    throw;
		    }
		}

	}
}
