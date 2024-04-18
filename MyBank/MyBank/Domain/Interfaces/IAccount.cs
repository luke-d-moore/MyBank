
using static MyBank.Account;

namespace MyBank
{
    public interface IAccount
    {
        public long AccountNumber { get; set; }
        public string SortCode { get; set; }
        public decimal Balance { get; set; }
        public decimal Withdrawn { get; set; }
        public decimal PaidIn { get; set; }
    }
}
