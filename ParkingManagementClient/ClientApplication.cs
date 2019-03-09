using System;
using System.Collections.Generic;
using Autofac;
using CommandLine;
using ParkingManagement.CommandLine;
using Serilog;

namespace ParkingManagementClient
{
    public class ClientApplication : IApplication, IStartable
    {
        private static ILogger _logger;
        public ClientApplication(ILogger logger)
        {
            _logger = logger;
        }
        public void Run(string[] args)
        {
            var result = Parser.Default.ParseArguments<ReadReportCommand>(args)
                .MapResult(
                (ReadReportCommand opts) =>
                {
                    opts.Execute();
                    return 0;
                },
                errs => throw new ArgumentException("Arguments are not parsed. Please see --help information")
            );
            // .WithParsed<ReadReportCommand>(opts => opts.Execute())
            // .WithNotParsed(_ => throw new ArgumentException("Arguments are not parsed. Please see --help information"));
            // _logger.CloseAndFlush();
        }
        public void Start() => System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            _logger.Fatal(eventArgs.ExceptionObject.ToString());
            Environment.Exit(1);
        }
    }
}