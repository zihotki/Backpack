using Microsoft.AspNetCore.Mvc;

namespace WebApiServer.Controllers
{
	[Route("api/[controller]")]
	public class OrdersController : Controller
	{
		[HttpPost]
		public void Post([FromBody]Order value)
		{

		}
	}
}