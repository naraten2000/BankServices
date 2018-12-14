using BankServices.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BankServices.Controllers
{
    public class AccountController : ApiController
    {
        const string TRANSACTION_SUCCESS = "Y";
        const string TRANSACTION_FAILURE = "N";
        const string TRANSACTION_TYPE_DEPOSIT = "D";
        const string TRANSACTION_TYPE_WITHDRAW = "W";


        BankServicesEntities _bankServicesModel = new BankServicesEntities();

        [HttpGet]
        public List<Account> Balance()
        {
            return _bankServicesModel.Accounts.ToList();
        }

        [HttpGet]
        [Route("api/Account/Balance/{accountId:int}")]
        public Account Balance(int accountId)
        {
            return _bankServicesModel.Accounts.FirstOrDefault(a => a.Id == accountId);
        }

        [HttpGet]
        public List<TransactionLog> TransactionLog()
        {
            return _bankServicesModel.TransactionLogs.ToList();
        }

        [HttpPost]
        public TransactionLog Deposit([FromBody] Transaction transaction)
        {
            var bankAccount = _bankServicesModel.Accounts.FirstOrDefault(a => a.Id == transaction.AccountId);
            TransactionLog transactionLog;

            if (bankAccount == null)
                transactionLog = new TransactionLog
                                    {
                                        AccountId = transaction.AccountId,
                                        Amount = transaction.Amount,
                                        Currency = transaction.Currency,
                                        Success = TRANSACTION_FAILURE,
                                        TransactionType = TRANSACTION_TYPE_DEPOSIT,
                                        Message = "Invalid account ID",
                                        ModDate = DateTime.Now
                                    };
            else if (transaction.Amount <= 0)
            {
                transactionLog = new TransactionLog
                                        {
                                            AccountId = transaction.AccountId,
                                            Amount = transaction.Amount,
                                            Currency = transaction.Currency,
                                            Success = TRANSACTION_FAILURE,
                                            TransactionType = TRANSACTION_TYPE_DEPOSIT,
                                            Message = string.Format("The passed-in amount must be greater than 0",
                                                            transaction.AccountId, bankAccount.Currency),
                                            ModDate = DateTime.Now
                                        };
            }
            else if (bankAccount.Currency != transaction.Currency)
            {
                transactionLog = new TransactionLog
                                        {
                                            AccountId = transaction.AccountId,
                                            Amount = transaction.Amount,
                                            Currency = transaction.Currency,
                                            Success = TRANSACTION_FAILURE,
                                            TransactionType = TRANSACTION_TYPE_DEPOSIT,
                                            Message = "The passed-in currency is not the same as account currency",
                                            ModDate = DateTime.Now
                                        };
            }
            else
            {
                bankAccount.Balance += transaction.Amount;

                transactionLog = new TransactionLog
                                        {
                                            AccountId = transaction.AccountId,
                                            Amount = transaction.Amount,
                                            Currency = transaction.Currency,
                                            Success = TRANSACTION_SUCCESS,
                                            TransactionType = TRANSACTION_TYPE_DEPOSIT,
                                            Balance = bankAccount.Balance,
                                            Message = string.Empty,
                                            ModDate = DateTime.Now
                                        };
            }

            _bankServicesModel.TransactionLogs.Add(transactionLog);
            _bankServicesModel.SaveChanges();

            return transactionLog;
        }

        [HttpPost]
        public TransactionLog Withdraw([FromBody] Transaction transaction)
        {
            var bankAccount = _bankServicesModel.Accounts.FirstOrDefault(a => a.Id == transaction.AccountId);
            TransactionLog transactionLog;

            if (bankAccount == null)
                transactionLog = new TransactionLog
                                    {
                                        AccountId = transaction.AccountId,
                                        Amount = transaction.Amount,
                                        Currency = transaction.Currency,
                                        Success = TRANSACTION_FAILURE,
                                        TransactionType = TRANSACTION_TYPE_WITHDRAW,
                                        Message = "Invalid account ID",
                                        ModDate = DateTime.Now
                                    };
            else if (transaction.Amount <= 0)
            {
                transactionLog = new TransactionLog
                                    {
                                        AccountId = transaction.AccountId,
                                        Amount = transaction.Amount,
                                        Currency = transaction.Currency,
                                        Success = TRANSACTION_FAILURE,
                                        TransactionType = TRANSACTION_TYPE_DEPOSIT,
                                        Message = string.Format("The passed-in amount must be greater than 0",
                                                        transaction.AccountId, bankAccount.Currency),
                                            ModDate = DateTime.Now
                                        };
            }
            else if (bankAccount.Currency != transaction.Currency)
            {
                transactionLog = new TransactionLog
                                    {
                                        AccountId = transaction.AccountId,
                                        Amount = transaction.Amount,
                                        Currency = transaction.Currency,
                                        Success = TRANSACTION_FAILURE,
                                        TransactionType = TRANSACTION_TYPE_WITHDRAW,
                                        Message = "The passed-in currency is not the same as account currency",
                                        ModDate = DateTime.Now
                                    };
            }
            else if (bankAccount.Balance < transaction.Amount)
            {
                transactionLog = new TransactionLog
                                    {
                                        AccountId = transaction.AccountId,
                                        Amount = transaction.Amount,
                                        Currency = transaction.Currency,
                                        Success = TRANSACTION_FAILURE,
                                        TransactionType = TRANSACTION_TYPE_WITHDRAW,
                                        Message = "Not enough balance to withdraw",
                                        ModDate = DateTime.Now
                                    };
            }
            else
            {
                bankAccount.Balance -= transaction.Amount;

                transactionLog = new TransactionLog
                                    {
                                        AccountId = transaction.AccountId,
                                        Amount = transaction.Amount,
                                        Currency = transaction.Currency,
                                        Success = TRANSACTION_SUCCESS,
                                        TransactionType = TRANSACTION_TYPE_WITHDRAW,
                                        Balance = bankAccount.Balance,
                                        Message = string.Empty,
                                        ModDate = DateTime.Now
                                    };

            }

            _bankServicesModel.TransactionLogs.Add(transactionLog);
            _bankServicesModel.SaveChanges();

            return transactionLog;
        }
    }
}
