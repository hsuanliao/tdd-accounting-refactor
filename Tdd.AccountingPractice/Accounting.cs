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
                totalAmount += EffectiveAmount(budget, period);
            }

            return totalAmount;
        }

        private static int EffectiveAmount(Budget budget, Period period)
        {
            return budget.DailyAmount() * period.EffectiveDays(budget.CreatePeriod());
        }

        private bool IsValid(DateTime start, DateTime end)
        {
            return start <= end;
        }
    }
}