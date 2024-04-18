using Moq;
using MyBank;

namespace MyBankTests
{
    public class TransferMoneyTests
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly TransferMoney _transferMoney;
        public TransferMoneyTests()
        {
            _accountDataStore = new Mock<IAccountDataStore>().Object;
            _transferMoney = new TransferMoney(_accountDataStore);
        }
        [Fact]
        public void Test1()
        {

        }
    }
}