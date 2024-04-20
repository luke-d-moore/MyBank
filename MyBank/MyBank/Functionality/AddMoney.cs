using System.Text.RegularExpressions;

namespace MyBank
{
    public static class AddMoney
    {
        private static decimal BasicLimit => 10000; //set Payin Limit to 10,000

        public static bool AddFunds(IAccount account, decimal amount)
        {
            if (account.PaidIn + amount > BasicLimit) throw new InvalidOperationException("Amount would exceed Pay in limit");

            account.Balance += amount;
            account.PaidIn += amount;

            return true;
        }
    }
}
