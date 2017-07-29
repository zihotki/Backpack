using Serilog.Configuration;

namespace Serilog.Bckpck
{
    public static class SerilogExtensions
    {
        public static LoggerConfiguration FromBackpack(LoggerEnrichmentConfiguration config)
        {
            return config.With<BackpackEnricher>();
        }
    }
}
