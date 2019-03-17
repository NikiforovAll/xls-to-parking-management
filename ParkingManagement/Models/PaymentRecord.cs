using System.Reflection;
using System.Text;

namespace ParkingManagement
{
    public class PaymentRecord
    {
        private static PropertyInfo[] _PropertyInfos = null;

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
}