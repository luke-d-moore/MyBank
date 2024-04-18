namespace MyBank
{
    public class AddMoney
    {
        private IAccountDataStore _accountDataStore;

        public AddMoney(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        public void AddFunds(Guid accountId, decimal amount)
        {

        }
    }
}
