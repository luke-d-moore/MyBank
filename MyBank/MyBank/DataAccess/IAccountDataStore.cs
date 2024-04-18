namespace MyBank
{
    public interface IAccountDataStore
    {
        IAccount GetAccountById(Guid accountId);

        void UpdateAccount(IAccount account);
    }
}
