using MyBank;
using Moq;
using System.Transactions;
namespace MyBankTests
{
    public class AddMoneyTests
    {
        private readonly Mock<IAccountDataStore> _accountDataStore;
        private readonly IAddMoney _addMoney;
        public AddMoneyTests()
        {
            _accountDataStore = new Mock<IAccountDataStore>();
            _addMoney = new AddMoney(_accountDataStore.Object);
        }

        [Fact]
        public void AddMoney_ValidAmount_ReturnsTrue()
        {
            //Arrange
            var account = new Account() 
            { 
                AccountNumber = 12345678, 
                SortCode = "12-34-56", 
                Balance = 1000, 
                Withdrawn = 0, 
                PaidIn = 0 
            };
            _accountDataStore.Setup(x => x.GetAccount(12345678, "12-34-56")).Returns(account);
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

        public static IEnumerable<object[]> InvalidData =>
        new List<object[]>
        {
            new object[] { 12345678, "123456", 5 },
            new object[] { 12345678, "invalid", 5 },
            new object[] { 1, "12-34-56", 5 },
            new object[] { 123456789, "12-34-56", 5 }
        };

        [Theory, MemberData(nameof(InvalidData))]
        public void AddMoney_InvalidData_ThrowsDataException(long AccountNumber, string SortCode, decimal Amount)
        {
            // Arrange
            var exceptionType = typeof(InvalidDataException);
            // Act and Assert
            Assert.Throws(exceptionType, () => _addMoney.AddFunds(AccountNumber, SortCode, Amount));
        }

        public static IEnumerable<object[]> InvalidAccountData =>
        new List<object[]>
        {
            new object[] { 11111111, "99-99-99", 5 },
            new object[] { 99999999, "11-11-11", 5 }
        };

        [Theory, MemberData(nameof(InvalidAccountData))]
        public void AddMoney_InValidAccountData_ThrowsDataException(long AccountNumber, string SortCode, decimal Amount)
        {
            //Arrange
            _accountDataStore.Setup(x => x.GetAccount(It.IsAny<long>(), It.IsAny<string>())).Returns(value: null);
            var exceptionType = typeof(InvalidDataException);
            // Act and Assert
            Assert.Throws(exceptionType, () => _addMoney.AddFunds(AccountNumber, SortCode, Amount));
        }
    }
}