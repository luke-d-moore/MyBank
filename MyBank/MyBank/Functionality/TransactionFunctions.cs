using System.Security.Principal;

namespace MyBank
{
    public static class TransactionFunctions
    {
        private static decimal TransferLimit => 1000;
        private static decimal PayInLimit => 10000;
        private static decimal WithdrawLimit => 300;

        public static bool TransferFunds(IAccount fromAccount, IAccount toAccount, decimal amount)
        {
            if (fromAccount.Withdrawn + amount > TransferLimit) throw new InvalidOperationException("Amount would exceed withdrawn limit");
            if (fromAccount.Balance - amount < 0) throw new InvalidOperationException("Account has insufficient funds to make transfer");

            fromAccount.Balance -= amount;
            fromAccount.Withdrawn += amount;

            toAccount.PaidIn += amount;
            toAccount.Balance += amount;

            return true;
        }

        public static bool WithdrawFunds(IAccount account, decimal amount)
        {
            if (account.Withdrawn + amount > WithdrawLimit) throw new InvalidOperationException("Amount would exceed withdrawn limit");
            if (account.Balance - amount < 0) throw new InvalidOperationException("Account has insufficient funds to make withdrawal");

            account.Balance -= amount;
            account.Withdrawn += amount;

            return true;
        }
        public static bool AddFunds(IAccount account, decimal amount)
        {
            if (account.PaidIn + amount > PayInLimit) throw new InvalidOperationException("Amount would exceed Pay in limit");

            account.Balance += amount;
            account.PaidIn += amount;

            return true;
        }
    }
}
