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

        public int OverlappingDayCount(Period another)
        {
            var effectiveStart = Start > another.Start
                ? Start
                : another.Start;
            var effectiveEnd = End < another.End
                ? End
                : another.End;

            return DayCount(effectiveStart, effectiveEnd);
        }

        private static int DayCount(DateTime start, DateTime end)
        {
            return (end - start).Days + 1;
        }
    }
}