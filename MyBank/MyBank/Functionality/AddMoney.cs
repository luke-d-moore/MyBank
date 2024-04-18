namespace MyBank
{
    public class AddMoney : IAddMoney
    {
        private IAccountDataStore _accountDataStore;

        public AddMoney(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        public bool AddFunds(long accountNumber, string sortCode, decimal amount)
        {
            if(amount <= 0) throw new ArgumentException();
            if(accountNumber <= 0) throw new ArgumentException();
            if(string.IsNullOrEmpty(sortCode)) throw new ArgumentException();

            return true;
        }
    }
}
