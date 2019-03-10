using CommandLine;
using Serilog;

namespace ParkingManagement.CommandLine
{
    [Verb("read", HelpText = "Read reports from configured input")]
    public class ReadReportOptions: ICommandOptions
    {
        public string Command { get; } = nameof(ReadReportCommand);
        // [Option("Verbose", HelpText="Verbose param")]
        // public bool Verbose { get; set; }
        public ReadReportOptions()
        {
        }
    }
}