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
            var period = new Period(start, end);
            if (!IsValid(start, end)) return 0;

            return budgetRepo.GetAll().Sum(b => b.EffectiveAmount(period));
        }

        private bool IsValid(DateTime start, DateTime end)
        {
            return start <= end;
        }
    }
}