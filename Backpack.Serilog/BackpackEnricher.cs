using BackpackCore;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Bckpck
{
    public class BackpackEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            foreach (var item in Backpack.GetUnique())
            {
                if (logEvent.Properties.ContainsKey(item.Name))
                {
                    continue;
                }
                
                LogEventProperty property;

	            switch (item.ItemType)
	            {
		            case ItemType.String:
			            property = propertyFactory.CreateProperty(item.Name, item.StringValue);
			            break;
		            case ItemType.Bool:
			            property = propertyFactory.CreateProperty(item.Name, item.BoolValue.Value);
			            break;
		            case ItemType.Long:
			            property = propertyFactory.CreateProperty(item.Name, item.LongValue.Value);
			            break;
		            case ItemType.Int:
			            property = propertyFactory.CreateProperty(item.Name, item.IntValue.Value);
			            break;
		            case ItemType.Byte:
			            property = propertyFactory.CreateProperty(item.Name, item.ByteValue.Value);
			            break;
		            case ItemType.Short:
			            property = propertyFactory.CreateProperty(item.Name, item.ShortValue.Value);
			            break;
		            case ItemType.Guid:
			            property = propertyFactory.CreateProperty(item.Name, item.GuidValue.Value.ToString("N"));
			            break;
		            default:
			            continue;
	            }

	            logEvent.AddPropertyIfAbsent(property);
            }
        }
    }
}