namespace MyBank
{
    public class TransferMoney : TransactionBase
    {
        private IAccountDataStore _accountDataStore;

        public TransferMoney(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        public override decimal Limit => throw new NotImplementedException();

        public bool TransferFunds(long fromAccountNumber, string fromSortCode, long toAccountNumber, string toSortCode, decimal amount)
        {
            return true;
        }
    }
}
