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
            var budgets = budgetRepo.GetAll();

            if (!IsValid(start, end)) return 0;

            if (IsSameMonth(start, end))
            {
                var budget = GetBudget(start, budgets);

                if (budget == null)
                {
                    return 0;
                }

                var dailyAmount = budget.Amount / DateTime.DaysInMonth(start.Year, start.Month);
                var effectiveDays = EffectiveDays(start, end);

                return CalculateAmount(dailyAmount, effectiveDays);
            }

            var totalAmount = 0;

            totalAmount += GetFirstAndLastTotalAmounts(start, end, budgets);

            totalAmount += GetMiddleTotalAmounts(start, end, budgets);

            return totalAmount;
        }

        private int GetFirstAndLastTotalAmounts(DateTime start, DateTime end, IEnumerable<Budget> budgets)
        {
            var totalAmount = 0;
            var filterYearMonths = new List<DateTime>() { start, end };

            foreach (var targetDateTime in filterYearMonths)
            {
                var targetMonthBudget = GetTargetMonthBudget(budgets, targetDateTime);
                if (targetMonthBudget != null)
                {
                    var monthOfDays = GetDayInTargetMonth(targetDateTime);
                    var unitOfDay = targetMonthBudget.Amount / monthOfDays;
                    var targetAmount = 0;
                    targetAmount = GetTargetAmount(start, end, targetDateTime, targetAmount, unitOfDay,
                        monthOfDays);
                    totalAmount += targetAmount;
                }
            }

            return totalAmount;
        }

        private int GetMiddleTotalAmounts(DateTime start, DateTime end, IEnumerable<Budget> budgets)
        {
            var monthsInTargetRange = GetMonthsInTargetRange(start, end);
            var totalAmount = 0;
            if (monthsInTargetRange > 1)
            {
                for (int i = 1; i < monthsInTargetRange; i++)
                {
                    var searchMonth = start.AddMonths(i);
                    var targetMonthBudget = GetTargetMonthBudget(budgets, searchMonth);
                    if (targetMonthBudget != null)
                    {
                        totalAmount += targetMonthBudget.Amount;
                    }
                }
            }

            return totalAmount;
        }

        private Budget GetTargetMonthBudget(IEnumerable<Budget> budgetList, DateTime targetDateTime)
        {
            return budgetList.FirstOrDefault(x => x.YearMonth == targetDateTime.ToString("yyyyMM"));
        }

        private int GetMonthsInTargetRange(DateTime start, DateTime end)
        {
            var diffMonths = 12 * (end.Year - start.Year) + (end.Month - start.Month);
            return diffMonths;
        }

        private int GetTargetAmount(DateTime start, DateTime end, DateTime targetDateTime, int targetAmount,
            int unitOfDay, int monthOfDays)
        {
            if (targetDateTime == start)
            {
                targetAmount = unitOfDay * (monthOfDays - targetDateTime.Day + 1);
            }
            else if (targetDateTime == end)
            {
                targetAmount = unitOfDay * targetDateTime.Day;
            }

            return targetAmount;
        }

        private bool IsSameMonth(DateTime start, DateTime end)
        {
            return start.ToString("yyyyMM") == end.ToString("yyyyMM");
        }

        private int CalculateAmount(int dailyAmount, int effectiveDays)
        {
            return dailyAmount * effectiveDays;
        }

        private bool IsValid(DateTime start, DateTime end)
        {
            return start <= end;
        }

        private Budget GetBudget(DateTime start, IEnumerable<Budget> budgets)
        {
            return budgets.FirstOrDefault(x => x.YearMonth == start.ToString("yyyyMM"));
        }

        private int EffectiveDays(DateTime start, DateTime end)
        {
            return (end - start).Days + 1;
        }

        private int GetDayInTargetMonth(DateTime targetDateTime)
        {
            return DateTime.DaysInMonth(targetDateTime.Year, targetDateTime.Month);
        }
    }
}