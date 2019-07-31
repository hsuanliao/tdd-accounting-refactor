using System;
using System.Collections.Generic;
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
            var budgetList = budgetRepo.GetAll();

            if (!IsValidateDateRange(start, end)) return 0;

            var period = new Period(start, end);
            var totalAmount = 0;
            foreach (var budget in budgetList)
            {
                totalAmount += budget.OverlappingAmount(period);
            }
            //var monthsInTargetRange = GetMonthsInTargetRange(start, end);
            //for (int i = 0; i <= monthsInTargetRange; i++)
            //{
            //    var currentDate = start.AddMonths(i);
            //    var budget = GetBudget(budgetList, currentDate);
            //    if (budget != null)
            //    {
            //        totalAmount += budget.OverlappingAmount(period);
            //    }
            //}

            return totalAmount;
        }

        private Budget GetBudget(IEnumerable<Budget> budgets, DateTime target)
        {
            return budgets.FirstOrDefault(x => x.YearMonth == target.ToString("yyyyMM"));
        }

        private int GetDayInTargetMonth(DateTime targetDateTime)
        {
            return DateTime.DaysInMonth(targetDateTime.Year, targetDateTime.Month);
        }

        private int GetMonthsInTargetRange(DateTime start, DateTime end)
        {
            var diffMonths = 12 * (end.Year - start.Year) + (end.Month - start.Month);
            return diffMonths;
        }

        private bool IsSameMonth(DateTime start, DateTime end)
        {
            return start.ToString("yyyyMM") == end.ToString("yyyyMM");
        }

        private bool IsValidateDateRange(DateTime start, DateTime end)
        {
            return start <= end;
        }
    }
}