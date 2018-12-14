using System;
using System.IO;
using BankServices.Controllers;
using BankServices.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankServices.Tests
{
    [TestClass]
    public class BalanceTest
    {
        AccountController _bank = new AccountController();

        [TestInitialize]
        public void TestInitialize()
        {
            // Need to setup DataDirectory for test project
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory().Replace("\\bin\\Debug", string.Empty) + "\\App_Data");
        }

        [TestMethod]
        public void CanGetBalance()
        {
            var account = _bank.Balance(1);
            Assert.IsNotNull(account);
            Assert.IsNotNull(account.Currency);
            Assert.IsTrue(account.OwnerId > 0);
            Assert.IsTrue(account.Balance > 0);
        }

        [TestMethod]
        public void BalanceAfterDepositMustBeCorrect()
        {
            var accountId = 1;
            var depositAmount = 100;

            var account = _bank.Balance(accountId);
            var oldBalance = account.Balance;

            var transaction = _bank.Deposit(new Transaction
            {
                AccountId = 1,
                Amount = depositAmount,
                Currency = "USD"
            });

            account = _bank.Balance(accountId);
            var newBalance = account.Balance;

            Assert.AreEqual(oldBalance + depositAmount, newBalance);
        }
    }
}
