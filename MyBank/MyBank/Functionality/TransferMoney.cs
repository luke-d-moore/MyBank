namespace MyBank
{
    public class TransferMoney
    {
        private IAccountDataStore _accountDataStore;

        public TransferMoney(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }
        public bool TransferFunds(long fromAccountNumber, string fromSortCode, long toAccountNumber, string toSortCode, decimal amount)
        {
            return true;
        }
    }
}
