namespace MyBank
{
    public class Account :IAccount
    {
        public enum eTransactionType
        {
            Add,
            withdraw,
            transfer
        }
        public Guid AccountId { get; set; }

        public decimal Balance { get; set; }

        public decimal Withdrawn { get; set; }

        public decimal PaidIn { get; set; }

        public bool TransferValidate(decimal amount, eTransactionType type)
        {
            return true;
        }
        public bool WithdrawValidate(decimal amount, eTransactionType type)
        {
            return true;
        }
        public bool AddValidate(decimal amount, eTransactionType type)
        {
            return true;
        }
    }
}
