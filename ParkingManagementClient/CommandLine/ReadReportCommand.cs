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

        public ReadReportCommand(ILogger logger, IOConfigSection ioConfig)
        {
            this.logger = logger;
            this.ioConfig = ioConfig;
        }

        public void Execute()
        {
            FileInfo sourceFile = new FileInfo(ioConfig.InputFile.Name);
            logger.Information($"Read file: {sourceFile.FullName}");
            var excel = new ExcelQueryFactory(sourceFile.FullName);
            //TODO: Refactor this mess
            var names = excel.GetWorksheetNames().ToArray();
            for (int name_i = 0; name_i < names.Length; name_i++)
            {
                logger.Information($"================= {names[name_i]} =================");
                var clients = excel.WorksheetRangeNoHeader("B4", "Q48", names[name_i]);
                int baseIndex = 3;
                int shift = 4;
                int numberOfRecordsToRead = 2;

                List<Client> processedClients = new List<Client>();
                foreach (var row in clients)
                {
                    var client = new Client()
                    {
                        FullName = row[1],
                        Id = row[0],
                        Records = new List<PaymentRecord>(numberOfRecordsToRead)
                    };
                    int currentPaymentRecordPointer = baseIndex;
                    client.Records.Add(new PaymentRecord()
                    {
                        Debt = FillIfEmpty(row[currentPaymentRecordPointer - 1]),
                    });
                    for (int i = 0; i < numberOfRecordsToRead; i++)
                    {
                        client.Records.Add(new PaymentRecord()
                        {
                            Fee = FillIfEmpty(row[currentPaymentRecordPointer]),
                            PaymentDate = row[currentPaymentRecordPointer + 1],
                            PaymentAmount = FillIfEmpty(row[currentPaymentRecordPointer + 2]),
                            Debt = FillIfEmpty(row[currentPaymentRecordPointer + 3]),
                        });
                        currentPaymentRecordPointer += shift;
                    }
                    client.VerifyRecords();

                    processedClients.Add(client);

                    // logger.Information(client.ToString());
                }
                var clientWithErrors = processedClients.Where(c => c.Records.Any(r => !r.IsValid));
                foreach (var err_client in clientWithErrors)
                {
                    logger.Information(err_client.ToString());
                }
            }
        }
        private string FillIfEmpty(string source)
        {
            return string.IsNullOrEmpty(source) ? "0" : source;
        }
    }
}