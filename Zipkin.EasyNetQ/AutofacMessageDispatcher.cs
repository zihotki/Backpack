using System;
using System.Threading.Tasks;
using Autofac;
using EasyNetQ.AutoSubscribe;
using Zipkin.Tracers;

namespace Zipkin.EasyNetQ
{
	public class AutofacMessageDispatcher : IAutoSubscriberMessageDispatcher
	{
		private readonly ILifetimeScope _component;

		public AutofacMessageDispatcher(ILifetimeScope component)
		{
			_component = component;
		}

		public void Dispatch<TMessage, TConsumer>(TMessage message)
			where TMessage : class
			where TConsumer : IConsume<TMessage>
		{
			using (var scope = _component.BeginLifetimeScope("message"))
			{
				var consumer = scope.Resolve<TConsumer>();

				var traceInfo = TraceInfoUtil.TraceInfo;
				var trace = new ServerBackpackTrace(typeof(TConsumer).FullName, traceInfo);

				try
				{
					consumer.Consume(message);
					trace.Clean();
				}
				catch (Exception e)
				{
					trace.Clean(e);
					throw;
				}
			}
		}

		public Task DispatchAsync<TMessage, TConsumer>(TMessage message)
			where TMessage : class
			where TConsumer : IConsumeAsync<TMessage>
		{
			var scope = _component.BeginLifetimeScope("async-message");

			var traceInfo = TraceInfoUtil.TraceInfo;
			var trace = new ServerBackpackTrace(typeof(TConsumer).FullName, traceInfo);
			
			var consumer = scope.Resolve<TConsumer>();
			var tsc = new TaskCompletionSource<object>();
			consumer
				.Consume(message)
				.ContinueWith(task =>
				{
					scope.Dispose();

					if (task.IsFaulted && task.Exception != null)
					{
						trace.Clean(task.Exception);

						tsc.SetException(task.Exception);
					}
					else
					{
						trace.Clean();

						tsc.SetResult(null);
					}
				});

			return tsc.Task;
		}
	}
}