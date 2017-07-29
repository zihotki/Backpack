using System;
using System.Net;
using System.Threading;
using BackpackCore;
using Zipkin.Constants;
using Zipkin.Tracers;

namespace Zipkin.Sample
{
	class Program
	{
		static void Main(string[] args)
		{
			var thread1 = "thread 1";
			var thread2 = "thread 2";

			var bootstrap = new ZipkinBootstrapper("hello", IPAddress.IPv6Loopback, 666);
			bootstrap.DispatchTo(new ConsoleSpanDispatcher())
				.WithSampleRate(1)
				.Start();

			var trace = new ClientBackpackTrace("root trace");
			var scope = Backpack.CurrentScope;

			new Thread(Thread1Work)
			{
				Name = "== Sample 1 =="

			}.Start(scope);

			new Thread(Thread2Work)
			{
				Name = "== Sample 2 =="
			}.Start(scope);

			Console.ReadLine();

			trace.Finish();
		}

		private static void Thread2Work(object scopeObj)
		{
			var scope = (BackpackScope) scopeObj;

			for (var i = 0; i < 3; i++)
			{
				var rootScope = Backpack.CreateScope("Thread 2 root worker scope", scope);

				try
				{
					var trace = new ServerBackpackTrace("Order processing");

					// this should be set at the entry point of an app
					Backpack.Add(BackpackConstants.IsSampled, true);
					Backpack.Add(BackpackConstants.TraceId, Guid.NewGuid());

					Backpack.Add("CartId", 233322 + i);

					Thread.Sleep(100);

					for (var j = 0; j < 2; j++)
					{
						Thread.Sleep(100); // imitating work

						var localTrace = new LocalBackpackTrace("Item processing");

						Backpack.Add("ItemId", j);

						Thread.Sleep(100); // imitating some work

						var client = new ClientBackpackTrace("Item processing microservice call");

						Thread2ItemProcessing();

						client.Finish();
						localTrace.Finish();
					}

					trace.Finish();
				}
				catch (Exception e)
				{
					rootScope.Clear(e);
				}

				Thread.Sleep(200);
			}
		}

		private static void Thread2ItemProcessing()
		{
			var microserviceScope = Backpack.CreateScope();
			try
			{
				Thread.Sleep(50); // imitating latency

				var serverTrace = new ServerBackpackTrace("Item microservice");

				Thread.Sleep(300); // imitating work


				var local = new LocalBackpackTrace("local microservice task");

				Thread.Sleep(500); // imitating work

				local.Finish();

				serverTrace.Finish();
			}
			catch (Exception e)
			{
				microserviceScope.Clear(e);
			}
		}

		private static void Thread1Work(object scopeObj)
		{
			var scope = (BackpackScope) scopeObj;
		}
	}
}
