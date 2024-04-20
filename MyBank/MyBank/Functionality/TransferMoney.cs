using System.Security.Principal;

namespace MyBank
{
    public static class TransferMoney
    {
        private static decimal BasicLimit => 1000;

        public static bool TransferFunds(IAccount fromAccount, IAccount toAccount, decimal amount)
        {
            if (fromAccount.Withdrawn + amount > BasicLimit) throw new InvalidOperationException("Amount would exceed withdrawn limit");
            if (fromAccount.Balance - amount < 0) throw new InvalidOperationException("Account has insufficient funds to make transfer");

            fromAccount.Balance -= amount;
            fromAccount.Withdrawn += amount;

            toAccount.PaidIn += amount;
            toAccount.Balance += amount;

            return true;
        }
    }
}
