using System;

namespace Tdd.AccountingPractice
{
    public class Budget
    {
        public int Amount { get; set; }
        public string YearMonth { get; set; }

        public int DayCount()
        {
            return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
        }

        public DateTime FirstDay()
        {
            return DateTime.ParseExact($"{YearMonth}01", "yyyyMMdd", null);
        }

        public Period GetPeriod()
        {
            var budgetPeriod = new Period(FirstDay(), LastDay());
            return budgetPeriod;
        }

        public DateTime LastDay()
        {
            return DateTime.ParseExact($"{YearMonth}{DayCount()}", "yyyyMMdd", null);
        }
    }
}