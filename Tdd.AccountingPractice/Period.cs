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

        public int EffectiveDays()
        {
            return (End - Start).Days + 1;
        }

        public int EffectiveDays(Budget budget)
        {
            var firstDay = budget.FirstDay();
            var lastDay = budget.LastDay();

            DateTime effectiveStart = firstDay > Start
                ? firstDay
                : Start;

            DateTime effectiveEnd = lastDay < End
                ? lastDay
                : End;

            return (effectiveEnd - effectiveStart).Days + 1;
        }
    }
}