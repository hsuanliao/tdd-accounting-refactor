using System;

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
            var budgetList = budgetRepo.GetAll();

            if (!IsValidateDateRange(start, end)) return 0;

            var period = new Period(start, end);
            var totalAmount = 0;
            foreach (var budget in budgetList)
            {
                totalAmount += budget.OverlappingAmount(period);
            }

            return totalAmount;
        }

        private bool IsValidateDateRange(DateTime start, DateTime end)
        {
            return start <= end;
        }
    }
}