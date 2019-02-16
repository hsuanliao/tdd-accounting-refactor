using System;

namespace Tdd.AccountingPractice
{
    public class Budget
    {
        public int Amount { get; set; }
        public string YearMonth { get; set; }

        public int EffectiveAmount(Period period)
        {
            return DailyAmount() * period.EffectiveDays(CreatePeriod());
        }

        private Period CreatePeriod()
        {
            return new Period(FirstDay(), LastDay());
        }

        private int DailyAmount()
        {
            return Amount / Days();
        }

        private int Days()
        {
            var firstDay = FirstDay();
            return DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
        }

        private DateTime FirstDay()
        {
            return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        }

        private DateTime LastDay()
        {
            return new DateTime(FirstDay().Year, FirstDay().Month,
                DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month));
        }
    }
}