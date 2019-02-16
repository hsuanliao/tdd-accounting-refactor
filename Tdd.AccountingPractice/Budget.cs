using System;

namespace Tdd.AccountingPractice
{
    public class Budget
    {
        public string YearMonth { get; set; }

        public int Amount { get; set; }

        public int Days()
        {
            var firstDay = FirstDay();
            return DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
        }

        public DateTime FirstDay()
        {
            return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        }

        public int DailyAmount()
        {
            return Amount / Days();
        }

        public DateTime LastDay()
        {
            return new DateTime(FirstDay().Year, FirstDay().Month, DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month));
        }

        public Period CreatePeriod()
        {
            return new Period(FirstDay(), LastDay());
        }
    }
}