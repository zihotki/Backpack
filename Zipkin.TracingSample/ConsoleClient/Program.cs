using System;
using System.Net;
using Zipkin.Codecs;

namespace Zipkin.Sample
{
	class Program
	{
		static void Main()
		{
			var bootstrap = new ZipkinBootstrapper("Console Client");
			bootstrap
				.DispatchToZipkin("localhost")
				.WithSampleRate(1)
				.Start();

			var shopClient = new ShopClient();

			while (true)
			{
				Console.WriteLine("Press Enter to put an order or Ctrl+c to exit.");
				Console.ReadLine();

				shopClient.SendOrder();
			}
		}
	}
}
