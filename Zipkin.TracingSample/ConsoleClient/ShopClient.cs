using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BackpackCore;
using Zipkin.Http;
using Zipkin.Tracers;

namespace Zipkin.Sample
{
	public class ShopClient
	{
		private static readonly Random Random = new Random();

		private static readonly HttpClient HttpClient = new HttpClient(new ZipkinHttpMessageHandler())
		{
			BaseAddress = new Uri("http://localhost:20186/api/")
		};

		public void SendOrder()
		{
			var itemsCountToOrder = Random.Next(10);

			Task.Run(() => SendOrderAsync(itemsCountToOrder));
			Console.WriteLine($"Scheduled order for {itemsCountToOrder} items.");
		}

		private async Task SendOrderAsync(int itemsToOrder)
		{
			var rootScope = Backpack.CreateScope("Unit of Work");
			try
			{
				var trace = new LocalBackpackTrace("Send Order");

				var orderId = Guid.NewGuid();

				Backpack.Add("OrderId", orderId);
				Backpack.Add("ItemsCount", itemsToOrder);

				var isPriorityOrder = false;
				if (itemsToOrder % 3 == 0)
				{
					// todo: logging message
					isPriorityOrder = true;
				}

				using (var request = new HttpRequestMessage())
				{
					request.RequestUri = new Uri("orders");
					request.Method = HttpMethod.Post;
					request.Content = new StringContent($"{{'orderId': '{orderId}', 'itemsCount': {itemsToOrder}, 'priority':{isPriorityOrder} }}",
						Encoding.UTF8, "application/json");

					await HttpClient.SendAsync(request);
				}

				// will automatically close the trace when closing root scope
				rootScope.Clear();
			}
			catch (Exception e)
			{
				rootScope.Clear(e);
			}
		}
	}
}