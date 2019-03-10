using System;
using System.Globalization;
using System.Linq;

namespace EventsCalendar.Services.Helpers
{
    /**
     * returns first character of month
     * independent of Country
     */ 
    public class MonthFirstCharFormatter
    {
        private static IFormatProvider _formatProvider;
        public static IFormatProvider FormatProvider
        {
            get
            {
                if (_formatProvider != null) return _formatProvider;
                var dtfi = new DateTimeFormatInfo();

                dtfi.AbbreviatedMonthNames = dtfi.MonthNames
                    .Select(x => x.FirstOrDefault().ToString())
                    .ToArray();

                _formatProvider = dtfi;
                return _formatProvider;
            }
        }
    }
}
