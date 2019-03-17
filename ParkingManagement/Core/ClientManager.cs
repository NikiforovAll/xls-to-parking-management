using System.Collections.Generic;
using System;
using LinqToExcel;
using System.Linq;
using AutoMapper;

namespace ParkingManagement
{
    public class ClientManager
    {
        private readonly IMapper _mapper;
        private string _sourceFile;
        private ExcelQueryFactory _excel;

        private string[] _worksheetNames;
        public ClientManager(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public string SourceFile { 
            get
            {
                return _sourceFile;
            }
            set
            {
                _sourceFile = value;
                _excel = new ExcelQueryFactory(value);
            }
        }

        public List<Client> ReadClients(string worksheetName, string startRange, string endRange, int paymentRecordsToRead)
        {
            ExcelUtils.TryGetWorksheetNames(_excel, ref _worksheetNames);
            if(!_worksheetNames.Contains(worksheetName))
            {
                throw new ArgumentException("Sheet was not found");
            }
            RowNoHeader[] clientsRows = _excel.WorksheetRangeNoHeader(startRange, endRange, worksheetName).ToArray();
            var clients = _mapper.Map<RowNoHeader[], Client[]>(clientsRows).Select(c => {
                c.VerifyRecords();
                return c;
            }).ToList();
            return clients;
        }

        public List<Client> ReadClients(int worksheetNumber, string startRange, string endRange, int paymentRecordsToRead)
        {
            ExcelUtils.TryGetWorksheetNames(_excel, ref _worksheetNames);
            var worksheetName = _worksheetNames.ElementAtOrDefault(worksheetNumber);
            return ReadClients(worksheetName, startRange, endRange, paymentRecordsToRead);
        }
    }
}