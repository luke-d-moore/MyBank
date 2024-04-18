namespace MyBank
{
    public interface IAccountDataStore
    {
        IAccount GetAccount(long AccountNumber, string SortCode);

        void UpdateAccount(IAccount account);
    }
}
