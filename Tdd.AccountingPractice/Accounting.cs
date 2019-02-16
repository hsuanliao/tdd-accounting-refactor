using System;
using System.Linq;

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

            return budgets.Sum(b => b.EffectiveAmount(period));
            //var totalAmount = 0;

            //foreach (var budget in budgets)
            //{
            //    totalAmount += budget.EffectiveAmount(period);
            //}

            //return totalAmount;
        }

        private bool IsValid(DateTime start, DateTime end)
        {
            return start <= end;
        }
    }
}