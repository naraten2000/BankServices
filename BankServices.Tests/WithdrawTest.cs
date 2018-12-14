using System;
using System.IO;
using BankServices.Controllers;
using BankServices.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankServices.Tests
{

    [TestClass]
    public class WithdrawTest
    {
        AccountController _bank = new AccountController();

        [TestInitialize]
        public void TestInitialize()
        {
            // Need to setup DataDirectory for test project
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory().Replace(".Tests\\bin\\Debug", string.Empty) + "\\App_Data");
        }

        [TestMethod]
        public void InvalidAccount()
        {
            var transaction = _bank.Withdraw(new Transaction
            {
                AccountId = 0,
                Amount = 1,
                Currency = "USD"
            });
            Assert.AreEqual(transaction.Success, "N");
            Assert.AreEqual(transaction.Message, "Invalid account ID");
        }

        [TestMethod]
        public void InvalidAmount()
        {
            var transaction = _bank.Withdraw(new Transaction
            {
                AccountId = 1,
                Amount = -100,
                Currency = "USD"
            });
            Assert.AreEqual(transaction.Success, "N");
            Assert.AreEqual(transaction.Message, "The passed-in amount must be greater than 0");
        }

        [TestMethod]
        public void InvalidCurrency()
        {
            var transaction = _bank.Withdraw(new Transaction
            {
                AccountId = 1,
                Amount = 100,
                Currency = "SGD"
            });
            Assert.AreEqual(transaction.Success, "N");
            Assert.AreEqual(transaction.Message, "The passed-in currency is not the same as account currency");
        }

        [TestMethod]
        public void CanWithdraw()
        {
            var transaction = _bank.Withdraw(new Transaction
            {
                AccountId = 1,
                Amount = 1000,
                Currency = "USD"
            });
            Assert.AreEqual(transaction.Success, "Y");
            Assert.AreEqual(transaction.Message, string.Empty);
        }

        [TestMethod]
        public void CanNotWithdrawWhenBalanceIsNotEnough()
        {
            var transaction = _bank.Withdraw(new Transaction
            {
                AccountId = 1,
                Amount = 10000000,
                Currency = "USD"
            });
            Assert.AreEqual(transaction.Success, "N");
            Assert.AreEqual(transaction.Message, "Not enough balance to withdraw");
        }
    }
}
