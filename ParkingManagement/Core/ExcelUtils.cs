using System.Linq;
using LinqToExcel;

namespace ParkingManagement
{
    public class ExcelUtils
    {
        public static bool TryGetWorksheetNames(ExcelQueryFactory excel, ref string [] worksheetNames)
        {
            bool isPreLoaded = true;
            if(worksheetNames == null)
            {
                isPreLoaded = false;
                worksheetNames = excel.GetWorksheetNames().ToArray();
            }
            return isPreLoaded;
        }
    }
}