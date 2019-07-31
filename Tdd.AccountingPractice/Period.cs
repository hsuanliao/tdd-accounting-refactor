using System;

namespace Tdd.AccountingPractice
{
    public class Period
    {
        public Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime End { get; private set; }
        public DateTime Start { get; private set; }

        public static int DayCount(DateTime start, DateTime end)
        {
            return (end - start).Days + 1;
        }

        public int OverlappingDayCount(Budget budget)
        {
            var effectiveStart = Start > budget.FirstDay()
                ? Start
                : budget.FirstDay();
            var effectiveEnd = End < budget.LastDay()
                ? End
                : budget.LastDay();

            return DayCount(effectiveStart, effectiveEnd);
        }
    }
}