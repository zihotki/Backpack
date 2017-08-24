using System;

namespace Zipkin.Utils
{
	public static class DateTimeOffsetExtensions
	{
		public static long ToUnixTimeMicroseconds(this DateTimeOffset value)
		{
			return Convert.ToInt64((value - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalMilliseconds * 1000);
		}
	}
}