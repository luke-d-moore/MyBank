namespace MyBank
{
    public class WithdrawMoney
    {
        private IAccountDataStore _accountDataStore;

        public WithdrawMoney(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        public void WithdrawFunds(Guid accountId, decimal amount)
        {

        }
    }
}
