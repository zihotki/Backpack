using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiServer.Controllers
{
	public class Order
	{
		public Guid Id { get; set; }
		public int ItemsCount { get; set; }
		public bool Priority { get; set; }
	}
}
