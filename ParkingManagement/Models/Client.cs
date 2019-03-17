using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingManagement
{
    public class Client
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
            // bool hasError = false;
            Records[0].IsValid = true;
            for (int i = Records.Count - 1; i >= 1; i--)
            {
                var r1 = Records[i];
                var r2 = Records[i - 1];
                var r1Debt = r1.Debt;
                var r1Fee = r1.Fee;
                var r2Debt = r2.Debt;
                var r1PaymentAmount = r1.PaymentAmount;
                r1.IsValid = r1Debt == (r2Debt - r1Fee + r1PaymentAmount);
            }
        }
    }
}