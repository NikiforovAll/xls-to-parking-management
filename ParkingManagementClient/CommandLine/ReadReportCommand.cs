using System.IO;
using CommandLine;
using LinqToExcel;
using Serilog;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System;
using ParkingManagement;
namespace ParkingManagement.CommandLine
{
    public class ReadReportCommand : ICommand
    {

        private readonly ILogger logger;
        private readonly IOConfigSection ioConfig;
        private readonly ClientManager _clientManager;

        public ReadReportCommand(ILogger logger, IOConfigSection ioConfig, ClientManager clientManager)
        {
            this.logger = logger;
            this.ioConfig = ioConfig;
            this._clientManager = clientManager;
        }

        public void Execute()
        {
            FileInfo sourceFile = new FileInfo(ioConfig.InputFile.Name);
            logger.Information($"Read file: {sourceFile.FullName}");
            //TODO: Add configuration
            var dict = new Dictionary<int, (string start, string finish)>{
                [0] = ("B4", "P48"),
                [1] = ("B4", "P96"),
                [2] = ("B4", "P371"),
                [3] = ("B4", "P196"),
                [4] = ("B4", "P210")
            };
            var dict2 = new Dictionary<int, int>
            {
                [0] = 2,
                [1] = 2,
                [2] = 2,
                [3] = 2,
                [4] = 1
            };

            _clientManager.SourceFile = sourceFile.FullName;
            for (int name_i = 0; name_i < dict.Count; name_i++)
            {
                List<Client> processedClients = new List<Client>();
                processedClients = _clientManager.ReadClients(name_i, dict[name_i].start, dict[name_i].finish, dict2[name_i]);
                var clientWithErrors = processedClients.Where(c => c.Records.Any(r => !r.IsValid));
                foreach (var err_client in clientWithErrors)
                {
                    logger.Information(err_client.ToString());
                }
            }
        }
    }
}