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
                var effectiveDays = period.EffectiveDays(new Period(budget.FirstDay(), budget.LastDay()));
                totalAmount += budget.DailyAmount() * effectiveDays;
            }

            return totalAmount;
        }

        private bool IsValid(DateTime start, DateTime end)
        {
            return start <= end;
        }
    }
}