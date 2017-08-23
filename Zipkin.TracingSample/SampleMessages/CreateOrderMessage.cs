using System;

namespace SampleMessages
{
	public class CreatePriorityOrderMessage
	{
		public Guid OrderId { get; set; }
		public int ItemsCount { get; set; }
	}

	public class CreateOrderMessage
    {
	    public Guid OrderId { get; set; }
	    public int ItemsCount { get; set; }
	}
}
