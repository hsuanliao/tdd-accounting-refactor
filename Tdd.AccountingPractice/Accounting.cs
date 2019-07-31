using System;
using System.Linq;

namespace Tdd.AccountingPractice
{
    public class Accounting
    {
        private readonly IBudgetRepo budgetRepo;

        public Accounting(IBudgetRepo repo)
        {
            budgetRepo = repo;
        }

        public double TotalAmount(DateTime start, DateTime end)
        {
            if (!IsValidateDateRange(start, end)) return 0;

            var period = new Period(start, end);

            return budgetRepo.GetAll().Sum(budget => budget.OverlappingAmount(period));
        }

        private bool IsValidateDateRange(DateTime start, DateTime end)
        {
            return start <= end;
        }
    }
}