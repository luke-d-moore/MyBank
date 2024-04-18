namespace MyBank
{
    public class WithdrawMoney
    {
        private IAccountDataStore _accountDataStore;

        public WithdrawMoney(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        public bool WithdrawFunds(long accountNumber, string sortCode, decimal amount)
        {
            return true;
        }
    }
}
