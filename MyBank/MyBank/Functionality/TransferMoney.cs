namespace MyBank
{
    public class TransferMoney
    {
        private IAccountDataStore _accountDataStore;

        public TransferMoney(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }
        public void TransferFunds(Guid fromAccountId, Guid toAccountId, decimal amount)
        {

        }
    }
}
