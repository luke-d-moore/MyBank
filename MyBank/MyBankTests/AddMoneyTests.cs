using MyBank;
using Moq;
using System.Transactions;
namespace MyBankTests
{
    public class AddMoneyTests
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IAddMoney _addMoney;
        public AddMoneyTests()
        {
            _accountDataStore = new Mock<IAccountDataStore>().Object;
            _addMoney = new AddMoney(_accountDataStore);
        }
        [Fact]
        public void AddMoney_ValidAmount_ReturnsTrue()
        {
            //Arrange
            var result = _addMoney.AddFunds(12345678, "12-34-56", 5);
            //Act and Assert
            Assert.True(result);
        }
        public static IEnumerable<object[]> InvalidArgumentData =>
        new List<object[]>
        {
            new object[] { 12345678, "12-34-56", 0 },
            new object[] { 12345678, "12-34-56", -5 },
            new object[] { 12345678, "", 5 },
            new object[] { 12345678, null, 5 },
            new object[] { 0, "12-34-56", 5 },
            new object[] { -5, "12-34-56", 5 }
        };

        [Theory, MemberData(nameof(InvalidArgumentData))]
        public void AddMoney_InvalidArgument_ThrowsArgumentException(long AccountNumber, string SortCode, decimal Amount)
        {
            // Arrange
            var exceptionType = typeof(ArgumentException);
            // Act and Assert
            Assert.Throws(exceptionType, () => _addMoney.AddFunds(AccountNumber, SortCode, Amount));
        }
    }
}