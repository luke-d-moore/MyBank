namespace MyBank
{
    public class AddMoney : TransactionBase, IAddMoney
    {
        private IAccountDataStore _accountDataStore;

        public AddMoney(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        public override decimal Limit => 10000;

        public bool AddFunds(long accountNumber, string sortCode, decimal amount)
        {
            if(amount <= 0) throw new ArgumentException("Amount must be greater than 0");
            if(accountNumber <= 0) throw new ArgumentException("AccountNumber must be greater than 0");
            if(string.IsNullOrEmpty(sortCode)) throw new ArgumentException("SortCode must not be null or empty");
            if(accountNumber.ToString().Length != 8) throw new InvalidDataException($"AccountNumber must be 8 digits in length");
            if(sortCode.ToString().Length != 8) throw new InvalidDataException($"SortCode must be 8 characters in length");

            var account = _accountDataStore.GetAccount(accountNumber, sortCode);

            if (account == null) 
                throw new InvalidDataException($"Account Could not be found with AccountNumber : {accountNumber} and SortCode : {sortCode}");



            return true;
        }
    }
}
