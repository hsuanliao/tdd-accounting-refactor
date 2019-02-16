using System;

namespace Tdd.AccountingPractice
{
    public class Accounting
    {
        private IBudgetRepo budgetRepo;

        public Accounting(IBudgetRepo repo)
        {
            budgetRepo = repo;
        }

        public double TotalAmount(DateTime start, DateTime end)
        {
            var budgets = budgetRepo.GetAll();

            var period = new Period(start, end);
            if (!IsValid(start, end)) return 0;

            var totalAmount = 0;

            foreach (var budget in budgets)
            {
                DateTime effectiveStart = budget.FirstDay() > period.Start
                    ? budget.FirstDay()
                    : period.Start;

                DateTime effectiveEnd = budget.LastDay() < period.End
                    ? budget.LastDay()
                    : period.End;

                var effectiveDays = EffectiveDays(effectiveStart, effectiveEnd);
                totalAmount += budget.DailyAmount() * effectiveDays;
            }

            return totalAmount;
        }

        private static int EffectiveDays(DateTime effectiveStart, DateTime effectiveEnd)
        {
            return (effectiveEnd - effectiveStart).Days + 1;
        }

        private bool IsValid(DateTime start, DateTime end)
        {
            return start <= end;
        }
    }
}