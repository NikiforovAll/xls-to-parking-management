using Autofac;
using System;
namespace ParkingManagement.CommandLine
{
    public class CommandFactory : ICommandFactory
    {
        private ILifetimeScope lifetimeScope;
        
        public CommandFactory(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }
        public ICommand CreateCommand(string commandName, ICommandOptions opts)
        {
            ICommand command = null;
            if(commandName.Equals(nameof(ReadReportCommand), StringComparison.OrdinalIgnoreCase)){
                command = lifetimeScope
                    .Resolve<ICommand>(new TypedParameter(typeof(ReadReportOptions), opts as ReadReportOptions));
            }
            return command;
        }
    }
}