using System.IO;
using CommandLine;
using LinqToExcel;
using Serilog;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System;

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
        private static PropertyInfo[] _PropertyInfos = null;
        class Client
        {
            public string FullName { get; set; }
            public string Id { get; set; }

            public List<PaymentRecord> Records { get; set; }

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Client: {FullName}-{Id}");
                foreach (var record in Records)
                {
                    sb.AppendLine($"Record:{System.Environment.NewLine}{record}");
                }
                return sb.ToString();
            }

            public void VerifyRecords()
            {
                bool hasError = false;
                Records[0].IsValid = true;
                for (int i = Records.Count - 1; i >= 1; i--)
                {
                    var r1 = Records[i];
                    var r2 = Records[i - 1];
                    var r1Debt = Double.Parse(r1.Debt);
                    var r1Fee = Double.Parse(r1.Fee);
                    var r2Debt = Double.Parse(r2.Debt);
                    var r1PaymentAmount = Double.Parse(r1.PaymentAmount);
                    r1.IsValid = r1Debt == (r2Debt - r1Fee + r1PaymentAmount);
                }
            }
        }
        class PaymentRecord
        {
            public string Debt { get; set; }
            public string Fee { get; set; }
            public string PaymentAmount { get; set; }

            public bool IsValid { get; set; }
            public string PaymentDate { get; set; }

            public override string ToString()
            {
                if (_PropertyInfos == null)
                    _PropertyInfos = this.GetType().GetProperties();

                var sb = new StringBuilder();

                foreach (var info in _PropertyInfos)
                {
                    var value = info.GetValue(this, null) ?? "(null)";
                    sb.AppendLine(info.Name + ": " + value.ToString());
                }

                return sb.ToString();
            }
        }

        private string FillIfEmpty(string source)
        {
            return string.IsNullOrEmpty(source) ? "0" : source;
        }
    }
}