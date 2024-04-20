using System.Text.RegularExpressions;

namespace MyBank
{
    public class AddMoney : TransactionBase, IAddMoney
    {
        private IAccountDataStore _accountDataStore;

        public AddMoney(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        public override decimal Limit => 10000; //set Payin Limit to 10,000

        public bool AddFunds(long accountNumber, string sortCode, decimal amount)
        {
            if(amount <= 0) throw new ArgumentException("Amount must be greater than 0");
            if(accountNumber <= 0) throw new ArgumentException("AccountNumber must be greater than 0");
            if(string.IsNullOrEmpty(sortCode)) throw new ArgumentException("SortCode must not be null or empty");
            if(accountNumber.ToString().Length != 8) throw new ArgumentException($"AccountNumber must be 8 digits in length");
            if(sortCode.ToString().Length != 8) throw new ArgumentException($"SortCode must be 8 characters in length");
            var r = new Regex(@"[0-9]{2}-[0-9]{2}-[0-9]{2}");
            //use [0-9] instead of \d because \d will match numbers from other languages and character sets
            //but only the numbers 0-9 are valid for bank sort codes
            if (!r.Match(sortCode).Success) throw new ArgumentException("SortCode has incorrect format, expected format : 11-11-11");

            var account = _accountDataStore.GetAccount(accountNumber, sortCode);

            if (account == null) 
                throw new InvalidDataException($"Account could not be found with AccountNumber : {accountNumber} and SortCode : {sortCode}");

            if (account.PaidIn + amount > Limit) throw new InvalidOperationException("Amount would exceed Pay in limit");

            account.Balance += amount;
            account.PaidIn += amount;

            _accountDataStore.UpdateAccount(account);

            return true;
        }
    }
}
