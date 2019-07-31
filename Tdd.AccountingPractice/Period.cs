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
            DateTime effectiveStart;
            DateTime effectiveEnd;

            if (budget.YearMonth == Start.ToString("yyyyMM"))
            {
                effectiveStart = Start;
                effectiveEnd = budget.LastDay();
            }
            else if (budget.YearMonth == End.ToString("yyyyMM"))
            {
                effectiveStart = budget.FirstDay();
                effectiveEnd = End;
            }
            else
            {
                effectiveStart = budget.FirstDay();
                effectiveEnd = budget.LastDay();
            }

            var effectiveDayCount = DayCount(effectiveStart, effectiveEnd);
            return effectiveDayCount;
        }
    }
}