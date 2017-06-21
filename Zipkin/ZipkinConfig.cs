using System.Runtime.CompilerServices;
using Zipkin.Model;
using Zipkin.Utils;

namespace Zipkin
{
	internal static class ZipkinConfig
	{
		internal static double SampleRate;
		internal static Endpoint ThisService;
		internal static Recorder.Recorder Recorder;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ShouldSample()
		{
			return RandomHelper.Sample(SampleRate);
		}

		public static void Record(Span span)
		{
			Recorder?.Record(span);
		}
	}
}