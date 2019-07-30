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

            totalAmount += GeFirstAndLastTotalAmounts(start, end, budgetList);

            totalAmount += GetMiddleTotalAmounts(start, end, budgetList);

            return totalAmount;
        }

        private int EffectiveDayCount(DateTime start, DateTime end)
        {
            return (end - start).Days + 1;
        }

        private int GeFirstAndLastTotalAmounts(DateTime start, DateTime end, IEnumerable<Budget> budgetList)
        {
            var totalAmount = 0;
            var filterYearMonths = new List<DateTime>() { start, end };

            foreach (var targetDateTime in filterYearMonths)
            {
                var targetMonthBudget = GetBudget(budgetList, targetDateTime);
                if (targetMonthBudget == null)
                {
                    totalAmount += 0;
                }

                var monthOfDays = GetDayInTargetMonth(targetDateTime);
                var unitOfDay = targetMonthBudget.Amount / monthOfDays;
                var targetAmount = 0;
                targetAmount = GetTargetAmount(start, end, targetDateTime, targetAmount, unitOfDay,
                    monthOfDays);
                totalAmount += targetAmount;
            }

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

        private int GetMiddleTotalAmounts(DateTime start, DateTime end, IEnumerable<Budget> budgetList)
        {
            var monthsInTargetRange = GetMonthsInTargetRange(start, end);
            var totalAmount = 0;
            if (monthsInTargetRange > 1)
            {
                for (int i = 1; i < monthsInTargetRange; i++)
                {
                    var searchMonth = start.AddMonths(i);
                    var targetMonthBudget = GetBudget(budgetList, searchMonth);
                    if (targetMonthBudget != null)
                    {
                        totalAmount += targetMonthBudget.Amount;
                    }
                }
            }

            return totalAmount;
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

        private bool IsValidateDateRange(DateTime start, DateTime end)
        {
            return start <= end;
        }
    }
}