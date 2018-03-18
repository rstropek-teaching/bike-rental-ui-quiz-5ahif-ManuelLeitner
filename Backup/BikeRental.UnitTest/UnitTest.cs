using BikeRental.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BikeRental.UnitTest {
    [TestClass]
    public class UnitTest {
        [TestMethod]
        public void TestCaseAdditioalHours() {
            var rental = new Rental() { Begin = new DateTime(2018, 2, 14, 8, 15, 0), End = new DateTime(2018, 2, 14, 10, 30, 0), Bike = new Bike() { PriceFirstHour = 3, PriceFollowingHours = 5 } };
            Assert.AreEqual(13, rental.CalculatePrice());
        }
        [TestMethod]
        public void TestCaseOneHour() {
            var rental = new Rental() { Begin = new DateTime(2018, 2, 14, 8, 15, 0), End = new DateTime(2018, 2, 14, 8, 45, 0), Bike = new Bike() { PriceFirstHour = 3 } };
            Assert.AreEqual(3, rental.CalculatePrice());
        }
        [TestMethod]
        public void TestCaseFree() {
            var rental = new Rental() { Begin = new DateTime(2018, 2, 14, 8, 15, 0), End = new DateTime(2018, 2, 14, 10, 25, 0) };
            Assert.AreEqual(13, rental.CalculatePrice());
        }
    }
}
