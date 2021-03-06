﻿using System;
using System.Threading.Tasks;
using Autofac;
using EasyNetQ.AutoSubscribe;
using EasyNetQ.Consumer;
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
			using (var scope = _component.BeginLifetimeScope("RabbitMQ message"))
			{
				var consumer = scope.Resolve<TConsumer>();

				var traceInfo = TraceInfoUtil.TraceInfo;
				var trace = new ServerBackpackTrace(GetTraceName<TMessage, TConsumer>(), traceInfo);

				try
				{
					consumer.Consume(message);
					trace.Close();
				}
				catch (Exception e)
				{
					trace.Close(e);
					throw;
				}
			}
		}

		public Task DispatchAsync<TMessage, TConsumer>(TMessage message)
			where TMessage : class
			where TConsumer : IConsumeAsync<TMessage>
		{
			var scope = _component.BeginLifetimeScope("RabbitMQ async-message");

			var traceInfo = TraceInfoUtil.TraceInfo;
			var trace = new ServerBackpackTrace(GetTraceName<TMessage, TConsumer>(), traceInfo);
			
			var consumer = scope.Resolve<TConsumer>();
			var tsc = new TaskCompletionSource<object>();
			consumer
				.Consume(message)
				.ContinueWith(task =>
				{
					scope.Dispose();

					if (task.IsFaulted && task.Exception != null)
					{
						trace.Close(task.Exception);

						tsc.SetException(task.Exception);
					}
					else
					{
						trace.Close();

						tsc.SetResult(null);
					}
				});

			return tsc.Task;
		}

		private static string GetTraceName<TMessage, TConsumer>()
		{
			return $"{typeof(TConsumer).FullName} {typeof(TMessage).Name}";
		}
	}
}