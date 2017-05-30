using Serilog.Configuration;
using Backpack.Serilog;

namespace Serilog
{
    public static class SerilogExtensions
    {
        public static LoggerConfiguration FromBackpack(LoggerEnrichmentConfiguration config)
        {
            return config.With<BackpackEnricher>();
        }
    }
}
