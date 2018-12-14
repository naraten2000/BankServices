using System;
using System.IO;
using BankServices.Controllers;
using BankServices.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankServices.Tests
{
    [TestClass]
    public class TransactionLogTest
    {
        AccountController _bank = new AccountController();

        [TestInitialize]
        public void TestInitialize()
        {
            // Need to setup DataDirectory for test project
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory().Replace(".Tests\\bin\\Debug", string.Empty) + "\\App_Data");
        }

        [TestMethod]
        public void VerifyThatTransactionLogGetsInsertedForValidCase()
        {
            var transactionLogs = _bank.TransactionLog();
            var oldCount = transactionLogs.Count;

            var transaction = _bank.Deposit(new Transaction
            {
                AccountId = 1,
                Amount = 100,
                Currency = "USD"
            });

            transactionLogs = _bank.TransactionLog();
            var newCount = transactionLogs.Count;

            Assert.AreEqual(oldCount + 1, newCount);
            Assert.AreEqual(transactionLogs[transactionLogs.Count - 1].Success, "Y");
            Assert.AreEqual(transactionLogs[transactionLogs.Count - 1].AccountId, transaction.AccountId);
            Assert.AreEqual(transactionLogs[transactionLogs.Count - 1].Amount, transaction.Amount);
            Assert.AreEqual(transactionLogs[transactionLogs.Count - 1].Currency, transaction.Currency);

        }

        [TestMethod]
        public void VerifyThatTransactionLogGetsInsertedForInvalidCase()
        {
            var transactionLogs = _bank.TransactionLog();
            var oldCount = transactionLogs.Count;

            var transaction = _bank.Deposit(new Transaction
            {
                AccountId = -1,
                Amount = 100,
                Currency = "USD"
            });

            transactionLogs = _bank.TransactionLog();
            var newCount = transactionLogs.Count;

            Assert.AreEqual(oldCount + 1, newCount);
            Assert.AreEqual(transactionLogs[transactionLogs.Count - 1].Success, "N");
            Assert.AreEqual(transactionLogs[transactionLogs.Count - 1].AccountId, transaction.AccountId);
            Assert.AreEqual(transactionLogs[transactionLogs.Count - 1].Amount, transaction.Amount);
            Assert.AreEqual(transactionLogs[transactionLogs.Count - 1].Currency, transaction.Currency);

        }
    }
}
