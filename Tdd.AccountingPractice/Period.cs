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

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public int EffectiveDays(Period another)
        {
            if (HasNoOverlap(another))
            {
                return 0;
            }

            DateTime effectiveStart = another.Start > Start
                ? another.Start
                : Start;

            DateTime effectiveEnd = another.End < End
                ? another.End
                : End;

            return (effectiveEnd - effectiveStart).Days + 1;
        }

        private bool HasNoOverlap(Period another)
        {
            return Start > another.End || End < another.Start;
        }
    }
}