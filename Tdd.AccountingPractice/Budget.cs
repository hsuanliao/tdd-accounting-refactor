using System;

namespace Tdd.AccountingPractice
{
    public class Budget
    {
        public int Amount { get; set; }
        public string YearMonth { get; set; }

        public int DayCount()
        {
            var firstDay = DateTime.ParseExact($"{YearMonth}01", "yyyyMMdd", null);
            return DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
        }

        public DateTime LastDay()
        {
            return DateTime.ParseExact($"{YearMonth}{DayCount()}", "yyyyMMdd", null);
        }
    }
}