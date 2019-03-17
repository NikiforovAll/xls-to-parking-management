using AutoMapper;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Mappings
{
    public class DefaultProfile : Profile
    {

        public DefaultProfile()
        {
            CreateMap<RowNoHeader, Client>()
                .ForMember(
                    c => c.FullName,
                    (opts) =>
                    {
                        opts.MapFrom(s => s[1]);
                    }
                )
                .ForMember(
                    c => c.Id,
                    (opts) =>
                    {
                        opts.MapFrom(s => s[0]);
                    }
                )
                .ForMember(
                   c => c.Records,
                   src => src.MapFrom((source, client, item, context) => ToPaymentrecordsList(source, context.Mapper))
                );
            CreateMap<string[], PaymentRecord>()
                .ForMember(
                    pr => pr.Fee,
                    (opts) =>
                    {
                        opts.MapFrom(s => TryParseDecimal(s[0]));
                    }
                ).ForMember(
                    pr => pr.Debt,
                    (opts) =>
                    {
                        opts.MapFrom(s => TryParseDecimal(s[3]));
                    }
                ).ForMember(
                    pr => pr.PaymentAmount,
                    (opts) =>
                    {
                        opts.MapFrom(s => TryParseDecimal(s[2]));
                    }
                ).ForMember(
                    pr => pr.PaymentDate,
                    (opts) =>
                    {
                        opts.MapFrom(s => s[1]);
                    }
                );
        }

        //TODO: bad approach, need to use mapper composition, refactor
        private static object ToPaymentrecordsList(RowNoHeader row, IMapper mapper)
        {
            List<PaymentRecord> mapped = new List<PaymentRecord>();
            int baseIndex = 3;
            int shift = 4;
            int currentPaymentRecordPointer = baseIndex;

            for (int i = 0; currentPaymentRecordPointer + 3 < row.Count ; i++)
            {
                
                string[] paymentRecordSource = new string[4];
                for (int j = 0; j < 4; j++)
                {
                    paymentRecordSource[j] = row[currentPaymentRecordPointer + j];
                }
                var currentPaymentRecord = mapper.Map<string[], PaymentRecord>(paymentRecordSource);
                mapped.Add(currentPaymentRecord);
                currentPaymentRecordPointer += shift;
            }
            return mapped;
        }

        private static object TryParseDecimal(string source)
        {
            if (String.IsNullOrEmpty(source))
            {
                source = "0";
            }
            return Decimal.Parse(source);
        }
    }
}
