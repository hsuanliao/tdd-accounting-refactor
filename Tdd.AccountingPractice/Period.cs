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
    }
}