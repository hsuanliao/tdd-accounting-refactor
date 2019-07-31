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
            foreach (var targetDateTime in filterYearMonths)
            {
                var budget1 = GetBudget(budgetList, targetDateTime);
                if (budget1 == null)
                {
                    totalAmount += 0;
                    continue;
                }

                var dailyAmount = budget1.Amount / budget1.DayCount();

                DateTime effectiveStart;
                DateTime effectiveEnd;
                if (targetDateTime == start)
                {
                    effectiveStart = targetDateTime;
                    effectiveEnd = budget1.LastDay();
                }
                else
                {
                    effectiveStart = budget1.FirstDay();
                    effectiveEnd = targetDateTime;
                }

                totalAmount += dailyAmount * EffectiveDayCount(effectiveStart, effectiveEnd);
            }

            totalAmount += GetMiddleTotalAmounts(start, end, budgetList);

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

        private int GetMiddleTotalAmounts(DateTime start, DateTime end, IEnumerable<Budget> budgetList)
        {
            var monthsInTargetRange = GetMonthsInTargetRange(start, end);
            var totalAmount = 0;
            for (int i = 1; i < monthsInTargetRange; i++)
            {
                var searchMonth = start.AddMonths(i);
                var targetMonthBudget = GetBudget(budgetList, searchMonth);
                if (targetMonthBudget != null)
                {
                    totalAmount += targetMonthBudget.Amount;
                }
            }

            return totalAmount;
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