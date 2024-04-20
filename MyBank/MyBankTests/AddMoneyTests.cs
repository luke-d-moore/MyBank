using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using MyBank;
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
            long accountNumber = 12345678;
            var sortCode = "12-34-56";
            var account = GetAccount(accountNumber, sortCode, 0, 0);
            _accountDataStore.Setup(x => x.GetAccount(accountNumber, sortCode)).Returns(account);
            var result = _addMoney.AddFunds(accountNumber, sortCode, 5);
            //Act and Assert
            Assert.True(result);
        }

        private Account GetAccount(long AccountNumber, string SortCode, decimal PaidIn, decimal Withdrawn)
        {
            var account = new Account()
            {
                AccountNumber = AccountNumber,
                SortCode = SortCode,
                Balance = 1000,
                Withdrawn = Withdrawn,
                PaidIn = PaidIn
            };

            return account;
        }
        public static IEnumerable<object[]> ExceedLimitData =>
        new List<object[]>
        {
            new object[] { 12345678, "12-34-56", 5 , "Amount would exceed Pay in limit"},
            new object[] { 12345678, "12-34-56", 10000 , "Amount would exceed Pay in limit"},
            new object[] { 12345678, "12-34-56", 2 , "Amount would exceed Pay in limit" }
        };

        [Theory, MemberData(nameof(ExceedLimitData))]
        public void AddMoney_AmountExceedsLimit_ThrowsInvalidOperationException(long AccountNumber, string SortCode, decimal Amount, string Message)
        {
            //Arrange
            _accountDataStore.Setup(x => x.GetAccount(AccountNumber, SortCode)).Returns(GetAccount(AccountNumber, SortCode, 9999, 0));
            var exceptionType = typeof(InvalidOperationException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => _addMoney.AddFunds(AccountNumber, SortCode, Amount));

            Assert.Equal(Message, ex.Message);
        }
        public static IEnumerable<object[]> InvalidArgumentData =>
        new List<object[]>
        {
            new object[] { 12345678, "12-34-56", 0 , "Amount must be greater than 0"},
            new object[] { 12345678, "12-34-56", -5 , "Amount must be greater than 0"},
            new object[] { 12345678, "", 5 , "SortCode must not be null or empty"},
            new object[] { 12345678, null, 5 , "SortCode must not be null or empty"},
            new object[] { 0, "12-34-56", 5 , "AccountNumber must be greater than 0"},
            new object[] { -5, "12-34-56", 5 , "AccountNumber must be greater than 0" },
            new object[] { 12345678, "12-34556", 5 , "SortCode has incorrect format, expected format : 11-11-11" },
            new object[] { 12345678, "12534-56", 5 , "SortCode has incorrect format, expected format : 11-11-11" }
        };

        [Theory, MemberData(nameof(InvalidArgumentData))]
        public void AddMoney_InvalidArgument_ThrowsArgumentException(long AccountNumber, string SortCode, decimal Amount, string Message)
        {
            // Arrange
            var exceptionType = typeof(ArgumentException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => _addMoney.AddFunds(AccountNumber, SortCode, Amount));

            Assert.Equal(Message, ex.Message);
        }

        public static IEnumerable<object[]> InvalidData =>
        new List<object[]>
        {
            new object[] { 12345678, "123456", 5 , "SortCode must be 8 characters in length" },
            new object[] { 12345678, "invalid", 5 , "SortCode must be 8 characters in length"},
            new object[] { 1, "12-34-56", 5 , "AccountNumber must be 8 digits in length" },
            new object[] { 123456789, "12-34-56", 5 , "AccountNumber must be 8 digits in length" }
        };

        [Theory, MemberData(nameof(InvalidData))]
        public void AddMoney_InvalidData_ThrowsDataException(long AccountNumber, string SortCode, decimal Amount, string Message)
        {
            // Arrange
            var exceptionType = typeof(InvalidDataException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => _addMoney.AddFunds(AccountNumber, SortCode, Amount));

            Assert.Equal(Message, ex.Message);
        }

        public static IEnumerable<object[]> InvalidAccountData =>
        new List<object[]>
        {
            new object[] { 11111111, "99-99-99", 5 , $"Account Could not be found with AccountNumber : {11111111} and SortCode : {"99-99-99"}"},
            new object[] { 99999999, "11-11-11", 5 , $"Account Could not be found with AccountNumber : {99999999} and SortCode : {"11-11-11"}" }
        };

        [Theory, MemberData(nameof(InvalidAccountData))]
        public void AddMoney_InValidAccountData_ThrowsDataException(long AccountNumber, string SortCode, decimal Amount, string Message)
        {
            //Arrange
            _accountDataStore.Setup(x => x.GetAccount(It.IsAny<long>(), It.IsAny<string>())).Returns(value: null);
            var exceptionType = typeof(InvalidDataException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => _addMoney.AddFunds(AccountNumber, SortCode, Amount));

            Assert.Equal(Message, ex.Message);
        }
    }
}