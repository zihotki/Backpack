using Serilog.Core;
using Serilog.Events;

namespace Backpack.Serilog
{
    public class BackpackEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            foreach (var item in Backpack.GetAll())
            {
                if (logEvent.Properties.ContainsKey(item.Name))
                {
                    continue;
                }

                // TODO: find a way to send json directly
                var property = propertyFactory.CreateProperty(item.Name, item.Value);
                logEvent.AddPropertyIfAbsent(property);
            }
        }
    }
}