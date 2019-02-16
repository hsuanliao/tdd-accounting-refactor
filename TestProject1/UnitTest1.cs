using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Tdd.AccountingPractice;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private FakeBudgetRepo _fakeBudgetRepo;
        private Accounting _accounting;

        [SetUp]
        public void Setup()
        {
            _fakeBudgetRepo = new FakeBudgetRepo();
            _accounting = new Accounting(_fakeBudgetRepo);
        }

        [Test]
        public void invalid_date()
        {
            GivenBudgets(new Budget() { YearMonth = "201901", Amount = 30 });
            var starDate = new DateTime(2019, 01, 05);
            var endDate = new DateTime(2019, 01, 04);

            AmountShouldBe(0, starDate, endDate);
        }

        private void GivenBudgets(params Budget[] budgets)
        {
            _fakeBudgetRepo.SetBudgets(budgets);
        }

        [Test]
        public void query_budget_its__amount_is_0()
        {
            GivenBudgets(new Budget() { Amount = 0, YearMonth = "201904" });

            AmountShouldBe(0, new DateTime(2019, 04, 01), new DateTime(2019, 04, 02));
        }

        [Test]
        public void query_single_day_of_single_budget()
        {
            GivenBudgets(new Budget() { Amount = 310, YearMonth = "201901" });

            AmountShouldBe(10, new DateTime(2019, 01, 01), new DateTime(2019, 01, 01));
        }

        [Test]
        public void query_whole_month_of_budget()
        {
            GivenBudgets(new Budget() { Amount = 310, YearMonth = "201901" });

            AmountShouldBe(310, new DateTime(2019, 01, 01), new DateTime(2019, 01, 31));
        }

        [Test]
        public void query_cross_2_month_of_2_budgets()
        {
            GivenBudgets(
                new Budget() { Amount = 280, YearMonth = "201902" },
                new Budget() { Amount = 310, YearMonth = "201903" });

            AmountShouldBe(20, new DateTime(2019, 02, 28), new DateTime(2019, 03, 01));
        }

        [Test]
        public void query_cross_2_months_of_one_budget_has_amount()
        {
            GivenBudgets(new Budget() { Amount = 310, YearMonth = "201903" },
                new Budget() { Amount = 0, YearMonth = "201904" });

            AmountShouldBe(20, new DateTime(2019, 03, 30), new DateTime(2019, 04, 04));
        }

        [Test]
        public void query_lunar_year_Feb_budget()
        {
            GivenBudgets(new Budget() { Amount = 290, YearMonth = "202002" });

            AmountShouldBe(280, new DateTime(2020, 02, 01), new DateTime(2020, 02, 28));
        }

        [Test]
        public void query_cross_3_months()
        {
            GivenBudgets(
                    new Budget() { Amount = 310, YearMonth = "201901" },
                    new Budget() { Amount = 280, YearMonth = "201902" },
                    new Budget() { Amount = 310, YearMonth = "201903" }
            );

            AmountShouldBe(400, new DateTime(2019, 01, 25), new DateTime(2019, 03, 05));
        }

        private void AmountShouldBe(double expected, DateTime start, DateTime end)
        {
            Assert.AreEqual(expected, _accounting.TotalAmount(start, end));
        }
    }

    public class FakeBudgetRepo : IBudgetRepo
    {
        private Budget[] _budgets;

        public IEnumerable<Budget> GetAll()
        {
            return _budgets.ToList();
            //var result = new List<Budget>
            //{
            //    new Budget() {Amount = 310, YearMonth = "201901"},
            //    new Budget() {Amount = 280, YearMonth = "201902"},
            //    new Budget() {Amount = 310, YearMonth = "201903"},
            //    new Budget() {Amount = 0, YearMonth = "201904"},
            //    new Budget() {Amount = 310, YearMonth = "201912"},
            //    new Budget() {Amount = 310, YearMonth = "202001"},
            //    new Budget() {Amount = 290, YearMonth = "202002"},
            //};
            //return result;
        }

        public void SetBudgets(params Budget[] budgets)
        {
            _budgets = budgets;
        }
    }
}