﻿using System;

namespace WebApiServer.Controllers.Models
{
	public class Order
	{
		public Guid OrderId { get; set; }
		public int ItemsCount { get; set; }
		public bool Priority { get; set; }
	}
}
