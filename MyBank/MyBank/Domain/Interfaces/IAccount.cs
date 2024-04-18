
using static MyBank.Account;

namespace MyBank
{
    public interface IAccount
    {
        public Guid AccountId { get; set; }
        public decimal Balance { get; set; }
        public decimal Withdrawn { get; set; }
        public decimal PaidIn { get; set; }
        public bool WithdrawValidate(decimal amount, eTransactionType type);
        public bool TransferValidate(decimal amount, eTransactionType type);
        public bool AddValidate(decimal amount, eTransactionType type);
    }
}
