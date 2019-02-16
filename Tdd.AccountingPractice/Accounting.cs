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
            var budegtList = budgetRepo.GetAll();
            
            if (IsValidateDateRange(start, end))
            {
                var targetBudets = GetBudget(start, budegtList);
                
                if (targetBudets == null)
                {
                    return 0;
                }
                
                var unitOfDay = targetBudets.Amount / GetDayInTargetMonth(start);
                var daysOfTargetMonth = GetDifferentDays(start, end);
                
                return CalculateAmount(unitOfDay, daysOfTargetMonth);
            }
            
            return 0;
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