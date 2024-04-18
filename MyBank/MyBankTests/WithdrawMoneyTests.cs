using Moq;
using MyBank;

namespace MyBankTests
{
    public class WithdrawMoneyTests
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly WithdrawMoney _withdrawMoney;
        public WithdrawMoneyTests()
        {
            _accountDataStore = new Mock<IAccountDataStore>().Object;
            _withdrawMoney = new WithdrawMoney(_accountDataStore);
        }
        [Fact]
        public void Test1()
        {

        }
    }
}