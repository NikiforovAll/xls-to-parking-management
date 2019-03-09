using CommandLine;
using Serilog;

namespace ParkingManagement.CommandLine
{
    [Verb("read", HelpText = "Read reports from configured input")]
    public class ReadReportCommand : ICommand
    {
        private readonly ILogger logger;

        public ReadReportCommand(ILogger logger)
        {
            this.logger = logger;
        }

        public void Execute()
        {
            logger.Information("ReadReportCommand. Invoked!");
        }
    }
}