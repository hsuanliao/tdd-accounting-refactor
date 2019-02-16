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
            
            if (IsValidateDateRange(start, end))
            {
                if (IsSameMonth(start, end))
                {
                    var targetBudgets = GetBudget(start, budgetList);
                
                    if (targetBudgets == null)
                    {
                        return 0;
                    }
                
                    var unitOfDay = targetBudgets.Amount / GetDayInTargetMonth(start);
                    var daysOfTargetMonth = GetDifferentDays(start, end);
                
                    return CalculateAmount(unitOfDay, daysOfTargetMonth);
                }
                
                var filterYearMonths = new List<DateTime>(){start, end};
                var totalAmount = 0;
                var monthsInTargetRange = GetMonthsInTargetRange(start, end);
                
                
                foreach (var targetDateTime in filterYearMonths)
                {
                    var targetMonthBudget = budgetList.FirstOrDefault(x => x.YearMonth == targetDateTime.ToString("yyyyMM"));
                    if (targetMonthBudget != null)
                    {
                        var monthOfDays = GetDayInTargetMonth(targetDateTime);
                        var unitOfDay = targetMonthBudget.Amount / monthOfDays;
                        var targetAmount = 0;
                        targetAmount = GetTargetAmount(start, end, targetDateTime, targetAmount, unitOfDay, monthOfDays);
                        totalAmount += targetAmount;
                    }
                }

                if (monthsInTargetRange > 1)
                {
                    for (int i = 1; i < monthsInTargetRange; i++)
                    {
                        var searchMonth = start.AddMonths(i);
                        var targetMonthBudget =
                            budgetList.FirstOrDefault(x => x.YearMonth == searchMonth.ToString("yyyyMM"));
                        if (targetMonthBudget != null)
                        {
                            totalAmount += targetMonthBudget.Amount;
                        }
                    }
                }

                return totalAmount;

            }
            
            return 0;
        }

        private int GetMonthsInTargetRange(DateTime start, DateTime end)
        {
            var diffMonths = 12 * (end.Year - start.Year) + (end.Month - start.Month);
            return diffMonths;

        }

        private static int GetTargetAmount(DateTime start, DateTime end, DateTime targetDateTime, int targetAmount,
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

        private static int CalculateAmount(int unitOfDay, int daysOfTargetMonth)
        {
            return unitOfDay*daysOfTargetMonth;
        }

        private static bool IsValidateDateRange(DateTime start, DateTime end)
        {
            return start <= end;
        }

        private static Budget GetBudget(DateTime start, IEnumerable<Budget> budegtList)
        {
            return budegtList.FirstOrDefault(x => x.YearMonth == start.ToString("yyyyMM"));
        }

        private int GetDifferentDays(DateTime start,DateTime end)
        {
            return (end-start).Days+1;
        }

        private int GetDayInTargetMonth(DateTime targetDateTime)
        {
            return DateTime.DaysInMonth(targetDateTime.Year,targetDateTime.Month);
        }
    }
}