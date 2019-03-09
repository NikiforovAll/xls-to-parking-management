using System;
using Autofac;
using ParkingManagement;
using Serilog;

namespace ParkingManagementClient
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ClientApplication>().As<IApplication>();
            builder.AddLogger();
            builder.AddStartupHandler();
            builder.AddFileConfig();
            return builder.Build();
        }

        public static void AddLogger(this ContainerBuilder builder)
        {
            builder.Register<ILogger>((c, p) =>
            {
                return new LoggerConfiguration()
                             .MinimumLevel.Debug()
                             .WriteTo.Console()
                             .WriteTo.File("logs/log.txt",
                                 rollingInterval: RollingInterval.Day,
                                 rollOnFileSizeLimit: true)
                             .CreateLogger();
            }).SingleInstance();
        }

        public static void AddFileConfig(this ContainerBuilder builder)
        {
            IOConfigSection config = System.Configuration
                .ConfigurationManager
                .GetSection(IOConfigSection.SECTION_NAME) as IOConfigSection;
            builder.RegisterInstance(config).As<IOConfigSection>().SingleInstance();
        }

        public static void AddStartupHandler(this ContainerBuilder builder)
        {
            builder.RegisterType<ClientApplication>()
                .As<IStartable>()
                .SingleInstance();
        }
    }
}