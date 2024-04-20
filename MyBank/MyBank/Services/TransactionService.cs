using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBank.Services
{
    public class TransactionService : ITransactionService
    {
        private IAccountDataStore _accountDataStore;

        public TransactionService(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }
        private bool ValidateAmount(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be greater than 0");

            return true;
        }
        private IAccount ValidateAccount(long accountNumber, string sortCode)
        {
            if (accountNumber <= 0) throw new ArgumentException("AccountNumber must be greater than 0");
            if (string.IsNullOrEmpty(sortCode)) throw new ArgumentException("SortCode must not be null or empty");
            if (accountNumber.ToString().Length != 8) throw new ArgumentException($"AccountNumber must be 8 digits in length");
            if (sortCode.ToString().Length != 8) throw new ArgumentException($"SortCode must be 8 characters in length");
            var r = new Regex(@"[0-9]{2}-[0-9]{2}-[0-9]{2}");
            //use [0-9] instead of \d because \d will match numbers from other languages and character sets
            //but only the numbers 0-9 are valid for bank sort codes
            if (!r.Match(sortCode).Success) throw new ArgumentException("SortCode has incorrect format, expected format : 11-11-11");

            var account = _accountDataStore.GetAccount(accountNumber, sortCode);

            if (account == null)
                throw new InvalidDataException($"Account could not be found with AccountNumber : {accountNumber} and SortCode : {sortCode}");

            return account;
        }

        public bool ExecuteTransaction(long fromAccountNumber, string fromSortCode, long toAccountNumber, string toSortCode, decimal amount)
        {
            ValidateAmount(amount);
            var fromAccount = ValidateAccount(fromAccountNumber, fromSortCode);
            var toAccount = ValidateAccount(toAccountNumber, toSortCode);

            TransferMoney.TransferFunds(fromAccount, toAccount, amount);

            _accountDataStore.UpdateAccount(fromAccount);
            _accountDataStore.UpdateAccount(toAccount);

            return true;
        }

        public bool ExecuteTransaction(eTransactionType transactionType, long accountNumber, string sortCode, decimal amount)
        {
            ValidateAmount(amount);
            var account = ValidateAccount(accountNumber, sortCode);

            switch (transactionType)
            {
                case eTransactionType.Add: 
                    AddMoney.AddFunds(account, amount);
                    break;
                case eTransactionType.Withdraw:
                    WithdrawMoney.WithdrawFunds(account, amount);
                    break;
                default: 
                    return false;
            }
            _accountDataStore.UpdateAccount(account);
            return true;
        }
    }
    public enum eTransactionType
    {
        Add,
        Withdraw
    }
}
