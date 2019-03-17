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
        private readonly ICommandFactory _commandFactory;

        public ClientApplication(ILogger logger, ICommandFactory commandFactory)
        {
            _logger = logger;
            _commandFactory = commandFactory;
        }
        public void Run(string[] args)
        {
            var result = Parser.Default.ParseArguments<ReadReportOptions>(args)
                .MapResult(
                (ReadReportOptions opts) =>
                {
                    var command = _commandFactory.CreateCommand(opts.Command, opts);
                    command.Execute();
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
            //Environment.Exit(1);
        }
    }
}