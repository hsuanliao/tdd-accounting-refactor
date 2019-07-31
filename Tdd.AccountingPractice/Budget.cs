using System;

namespace Tdd.AccountingPractice
{
    public class Budget
    {
        public int Amount { get; set; }
        public string YearMonth { get; set; }

        public int DailyAmount()
        {
            return Amount / DayCount();
        }

        public int OverlappingAmount(Period period)
        {
            return DailyAmount() * period.OverlappingDayCount(GetPeriod());
        }

        private int DayCount()
        {
            return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
        }

        private DateTime FirstDay()
        {
            return DateTime.ParseExact($"{YearMonth}01", "yyyyMMdd", null);
        }

        private Period GetPeriod()
        {
            return new Period(FirstDay(), LastDay());
        }

        private DateTime LastDay()
        {
            return DateTime.ParseExact($"{YearMonth}{DayCount()}", "yyyyMMdd", null);
        }
    }
}