namespace MyBank
{
    public interface IAddMoney
    {
        public bool AddFunds(long accountNumber, string sortCode, decimal amount);
    }
}