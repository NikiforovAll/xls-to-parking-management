using CommandLine;
using Serilog;

namespace ParkingManagement.CommandLine
{
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