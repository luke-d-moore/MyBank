namespace MyBank.Services
{
    public interface ITransactionService
    {
        public bool ExecuteTransaction(eTransactionType transactionType, long accountNumber, string sortCode, decimal amount);
        public bool ExecuteTransaction(long fromAccountNumber, string fromSortCode, long toAccountNumber, string toSortCode, decimal amount);
    }
}