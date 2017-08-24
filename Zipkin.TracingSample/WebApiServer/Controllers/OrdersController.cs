using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleMessages;
using WebApiServer.Controllers.Models;

namespace WebApiServer.Controllers
{
	[Route("api/[controller]")]
	public class OrdersController : Controller
	{
		private readonly IBus _bus;
		
		public OrdersController(IBus bus)
		{
			_bus = bus;
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody]Order value)
		{
			await Task.Delay(2_000);
					
			if (value.Priority)
			{
				_bus.Publish(new CreateOrderMessage
				{
					OrderId = value.OrderId,
					ItemsCount = value.ItemsCount
				});
			}
			else
			{
				_bus.Publish(new CreatePriorityOrderMessage
				{
					OrderId = value.OrderId,
					ItemsCount = value.ItemsCount
				});
			}

			return Ok();
		}
	}
}