namespace MyBank
{
    public class Account :IAccount
    {
        public long AccountNumber { get; set; }
        public string SortCode { get; set; }
        public decimal Balance { get; set; }
        public decimal Withdrawn { get; set; }
        public decimal PaidIn { get; set; }
    }
}
