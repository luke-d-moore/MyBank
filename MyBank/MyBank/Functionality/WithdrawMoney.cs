namespace MyBank
{
    public static class WithdrawMoney
    {
        private static decimal BasicLimit => 300;

        public static bool WithdrawFunds(IAccount account, decimal amount)
        {
            if (account.Withdrawn + amount > BasicLimit) throw new InvalidOperationException("Amount would exceed withdrawn limit");
            if (account.Balance - amount < 0) throw new InvalidOperationException("Account has insufficient funds to make withdrawal");

            account.Balance -= amount;
            account.Withdrawn += amount;

            return true;
        }
    }
}
