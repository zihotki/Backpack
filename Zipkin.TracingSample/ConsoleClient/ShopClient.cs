using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BackpackCore;
using Zipkin.Http;
using Zipkin.Tracers;

namespace Zipkin.Sample
{
	public class ShopClient
	{
		private readonly Random _random = new Random();

		private readonly HttpClient _httpClient = new HttpClient(new ZipkinHttpMessageHandler()) {BaseAddress = new Uri("http://localhost:20186/api/")};

		public ShopClient()
		{
			_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public void SendOrder()
		{
			var itemsCountToOrder = _random.Next(10);

			Task.Run(() => SendOrderAsync(itemsCountToOrder));
			Console.WriteLine($"Scheduled order for {itemsCountToOrder} items.");
		}

		private async Task SendOrderAsync(int itemsToOrder)
		{
			var rootScope = Backpack.CreateScope("Unit of Work");
			try
			{
				var trace = new ServerBackpackTrace("Send Order");

				var orderId = Guid.NewGuid();

				Backpack.Add("OrderId", orderId);
				Backpack.Add("ItemsCount", itemsToOrder);

				await Task.Delay(150 * itemsToOrder);

				var isPriorityOrder = itemsToOrder % 3 == 0;

				var json = $"{{\"orderId\": \"{orderId}\", \"itemsCount\": {itemsToOrder}, \"priority\":{isPriorityOrder.ToString().ToLower()} }}";
				await _httpClient.PostAsync("orders", new StringContent(json, Encoding.UTF8, "application/json"));

				// will automatically close the trace when closing root scope
				rootScope.Clear();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());

				rootScope.Clear(e);
			}
		}
	}
}