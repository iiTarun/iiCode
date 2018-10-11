using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace UPRDEngine
{
    public class Format
    {
        public static DateTime FormatDateTime(string DateTime_)
        {
            var timePart = DateTime_;
            if (DateTime_.Length == 13) { timePart = "0" + DateTime_.Substring(10, 3) + "0"; }
            else
                if (DateTime_.Length == 14) { timePart = timePart = DateTime_.IndexOf(":0") > 0 ? (DateTime_.Substring(10, 4) + "0") : ("0" + (DateTime_.Substring(10, 4))); }
            else if (DateTime_.Length == 15) { timePart = DateTime_.Substring(10, 5); }

            try
            {
                DateTime date1;
                DateTime.TryParse(DateTime_.Substring(6, 4) + "-" + DateTime_.Substring(3, 2) + "-" + DateTime_.Substring(0, 2) + " " + timePart, out date1);
                return date1;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return DateTime.Today;
            }
        }
        public static string StringToTitleCase(string stringline)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;
            return ti.ToTitleCase(stringline.Trim());
        }

        public static DateTime FormatDate(string datetoformat)
        {
            try
            {
                DateTime date;
                DateTime.TryParse(datetoformat.Substring(6, 4) + "-" + datetoformat.Substring(3, 2) + "-" + datetoformat.Substring(0, 2) + " 00:00:00.0", out date);
                return date;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return DateTime.Today;
            }
        }

        public static string FormatStartDate(string datetoformat)
        {
            try
            {
                return datetoformat.Substring(0, 2) + "/" + datetoformat.Substring(3, 2) + "/" + datetoformat.Substring(6, 4);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DateTime _currentDateTime = DateTime.Now;
                return _currentDateTime.ToString("MM") + "/" + _currentDateTime.ToString("dd") + "/" + _currentDateTime.ToString("yyyy");
            }
        }

        public static string FormatEndDate(string datetoformat)
        {
            try
            {
                return datetoformat.Substring(0, 2) + "/" + datetoformat.Substring(3, 2) + "/" + datetoformat.Substring(6, 4);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DateTime _currentDateTime = DateTime.Now;
                return _currentDateTime.ToString("MM") + "/" + _currentDateTime.ToString("dd") + "/" + _currentDateTime.ToString("yyyy");
            }
        }

        public static string GisbDate()
        {
            DateTime date = DateTime.Now;

            string Month = date.Month.ToString();
            Month = Month.Length == 1 ? "0" + Month : Month;

            string Day = date.Day.ToString();
            Day = Day.Length == 1 ? "0" + Day : Day;

            string Hour = date.Hour.ToString();
            Hour = Hour.Length == 1 ? "0" + Hour : Hour;

            string Minute = date.Minute.ToString();
            Minute = Minute.Length == 1 ? "0" + Minute : Minute;

            string Second = date.Second.ToString();
            Second = Second.Length == 1 ? "0" + Second : Second;

            return DateTime.UtcNow.Year.ToString() + Month + Day + Hour + Minute + Second;
        }

        public static string GisbDate(string datetoformat)
        {
            return datetoformat.Substring(6, 4) + datetoformat.Substring(3, 2) + datetoformat.Substring(0, 2);
        }

        public static string NomDates(string datetoformat)
        {
            return datetoformat.Substring(6, 4) + datetoformat.Substring(0, 2) + datetoformat.Substring(3, 2);
        }

        public static string EDIFormat(string datetoformat)
        {
            DateTime date;
            DateTime.TryParse(datetoformat.Substring(6, 4) + "-" + datetoformat.Substring(3, 2) + "-" + datetoformat.Substring(0, 2) + " 00:00:00.0", out date);

            StringBuilder sb = new StringBuilder();

            string Year = date.Year.ToString();

            string Month = date.Month.ToString();
            Month = Month.Length == 1 ? "0" + Month : Month;

            string Day = date.Day.ToString();
            Day = Day.Length == 1 ? "0" + Day : Day;

            string Hour = date.Hour.ToString();
            Hour = Hour.Length == 1 ? "0" + Hour : Hour;

            string Minute = date.Minute.ToString();
            Minute = Minute.Length == 1 ? "0" + Minute : Minute;

            string Second = date.Second.ToString();
            Second = Second.Length == 1 ? "0" + Second : Second;

            return Year + Month + Day + Hour + Minute + Second;
        }

        public static Guid ExtractMessageID(string FileName)
        {
            try
            {
                FileName = FileName.Replace("{", "").Replace("}", "");
                FileName = FileName.Substring(0, FileName.IndexOf("."));

                return new Guid(FileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Guid.NewGuid();
            }
        }
        public static Guid ExtractMessageID(string FileName, bool ToGetNewName)
        {
            try
            {
                if (ToGetNewName)
                {
                    FileName = FileName.Replace(FileName.Substring(0, 36), "");
                    FileName = FileName.Replace("{", "").Replace("}", "").Replace("_", "").Replace(".txt", "");
                }
                else
                {
                    FileName = FileName.Substring(0, 36);
                }
                return new Guid(FileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Guid.NewGuid();
            }
        }
        public static string FormatDuns(String Duns)
        {
            var regEx = new Regex("(?<Alpha>[a-zA-Z]*)(?<Numeric>[0-9]*)");
            var match = regEx.Match(Duns);
            return match.Groups["Numeric"].Value.ToString();
        }
    }
}
