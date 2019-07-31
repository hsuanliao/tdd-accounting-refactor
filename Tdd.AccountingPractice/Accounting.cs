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
            if (IsSameMonth(start, end))
            {
                var budget = GetBudget(budgetList, start);
                if (budget == null)
                {
                    return 0;
                }

                return budget.Amount / budget.DayCount() * EffectiveDayCount(start, end);
            }

            var totalAmount = 0;

            var filterYearMonths = new List<DateTime>() { start, end };
            foreach (var currentDate in filterYearMonths)
            {
                var budget = GetBudget(budgetList, currentDate);
                if (budget == null)
                {
                    totalAmount += 0;
                    continue;
                }

                DateTime effectiveStart;
                DateTime effectiveEnd;
                if (currentDate == start)
                {
                    effectiveStart = currentDate;
                    effectiveEnd = budget.LastDay();
                }
                else
                {
                    effectiveStart = budget.FirstDay();
                    effectiveEnd = currentDate;
                }

                totalAmount += budget.Amount / budget.DayCount() * EffectiveDayCount(effectiveStart, effectiveEnd);
            }

            var monthsInTargetRange = GetMonthsInTargetRange(start, end);
            for (int i = 1; i < monthsInTargetRange; i++)
            {
                var currentDate = start.AddMonths(i);
                var budget = GetBudget(budgetList, currentDate);
                if (budget != null)
                {
                    totalAmount += budget.Amount / budget.DayCount() * EffectiveDayCount(budget.FirstDay(), budget.LastDay());
                }
            }

            return totalAmount;
        }

        private int EffectiveDayCount(DateTime start, DateTime end)
        {
            return (end - start).Days + 1;
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