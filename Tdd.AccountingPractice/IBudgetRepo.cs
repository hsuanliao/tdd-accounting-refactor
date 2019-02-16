using System.Collections.Generic;

namespace Tdd.AccountingPractice
{
    public interface IBudgetRepo
    {
        IEnumerable<Budget> GetAll();
    }
}