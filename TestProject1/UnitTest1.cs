using System;
using NUnit.Framework;
using Tdd.AccountingPractice;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private Accounting _accounting = new Accounting();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ValidateDate()
        {
            var starDate = new DateTime(2019, 01, 05);
            var endDate = new DateTime(2019, 01, 04);
            
            var totalAmount = _accounting.TotalAmount(starDate, endDate);
            
            AmountShouldBe(0, totalAmount);
        }
        
        [Test]
        public void QueryTotalWithNoBudgetRecord()
        {
            var starDate = new DateTime(2019, 04, 01);
            var endDate = new DateTime(2019, 04, 02);
            
            var totalAmount = _accounting.TotalAmount(starDate, endDate);
            
            AmountShouldBe(0, totalAmount);
        }

        private static void AmountShouldBe(double expected, double totalAmount)
        {
            Assert.AreEqual(expected, totalAmount);
        }
    }
}