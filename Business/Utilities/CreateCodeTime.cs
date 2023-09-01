using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Business.Utilities
{
    public static class CreateCodeTime
    {
        public static string CreateTime()
        {
            DateTime dateTime = DateTime.Now;
            string day = dateTime.Day.ToString();
            string month = dateTime.Month.ToString();
            string year = dateTime.Year.ToString();
            year = year.Substring(year.Length - 3);

            return day + month + year + dateTime.Minute + dateTime.Second;
        }
    }
}
