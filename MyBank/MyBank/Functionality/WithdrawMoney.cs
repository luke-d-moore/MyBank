namespace MyBank
{
    public class WithdrawMoney :TransactionBase
    {
        private IAccountDataStore _accountDataStore;

        public WithdrawMoney(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        public override decimal Limit => throw new NotImplementedException();

        public bool WithdrawFunds(long accountNumber, string sortCode, decimal amount)
        {
            return true;
        }
    }
}
