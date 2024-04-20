using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using MyBank;
using MyBank.Services;
namespace MyBankTests
{
    public class TransactionServiceTests
    {
        private readonly Mock<IAccountDataStore> _accountDataStore;
        private readonly ITransactionService _transactionService;
        public TransactionServiceTests()
        {
            _accountDataStore = new Mock<IAccountDataStore>();
            _transactionService = new TransactionService(_accountDataStore.Object);
        }

        [Fact]
        public void TransactionService_ValidAddAmount_ReturnsTrue()
        {
            //Arrange
            long accountNumber = 12345678;
            var sortCode = "12-34-56";
            var account = GetAccount(accountNumber, sortCode, 0, 0);
            _accountDataStore.Setup(x => x.GetAccount(accountNumber, sortCode)).Returns(account);
            var result = _transactionService.ExecuteTransaction(eTransactionType.Add,accountNumber, sortCode, 5);
            //Act and Assert
            Assert.True(result);
        }
        [Fact]
        public void TransactionService_ValidWithdrawAmount_ReturnsTrue()
        {
            //Arrange
            long accountNumber = 12345678;
            var sortCode = "12-34-56";
            var account = GetAccount(accountNumber, sortCode, 0, 0);
            _accountDataStore.Setup(x => x.GetAccount(accountNumber, sortCode)).Returns(account);
            var result = _transactionService.ExecuteTransaction(eTransactionType.Withdraw, accountNumber, sortCode, 5);
            //Act and Assert
            Assert.True(result);
        }
        [Fact]
        public void TransactionService_ValidTransferAmount_ReturnsTrue()
        {
            //Arrange
            var fromAccount = GetAccount(12345678, "12-34-56", 0, 0);
            var toAccount = GetAccount(12345555, "12-34-55", 0, 0);
            _accountDataStore.Setup(x => x.GetAccount(12345678, "12-34-56")).Returns(fromAccount);
            _accountDataStore.Setup(x => x.GetAccount(12345555, "12-34-55")).Returns(toAccount);
            var result = _transactionService.ExecuteTransaction(12345678, "12-34-56", 12345555, "12-34-55", 5);
            //Act and Assert
            Assert.True(result);
        }

        private Account GetAccount(long AccountNumber, string SortCode, decimal PaidIn, decimal Withdrawn)
        {
            return new Account()
            {
                AccountNumber = AccountNumber,
                SortCode = SortCode,
                Balance = 1000,
                Withdrawn = Withdrawn,
                PaidIn = PaidIn
            };
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
            new object[] { 12345678, "12534-56", 5 , "SortCode has incorrect format, expected format : 11-11-11" },
            new object[] { 12345678, "123456", 5 , "SortCode must be 8 characters in length" },
            new object[] { 12345678, "invalid", 5 , "SortCode must be 8 characters in length"},
            new object[] { 1, "12-34-56", 5 , "AccountNumber must be 8 digits in length" },
            new object[] { 123456789, "12-34-56", 5 , "AccountNumber must be 8 digits in length" }
        };

        [Theory, MemberData(nameof(InvalidArgumentData))]
        public void TransactionService_InvalidArgument_ThrowsArgumentException(long AccountNumber, string SortCode, decimal Amount, string Message)
        {
            // Arrange
            var exceptionType = typeof(ArgumentException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => _transactionService.ExecuteTransaction(eTransactionType.Add, AccountNumber, SortCode, Amount));

            Assert.Equal(Message, ex.Message);
        }

        public static IEnumerable<object[]> InvalidTransferArgumentData =>
        new List<object[]>
        {
            new object[] { 12345678, "12-34-56", 12345678, "12-34-56", 0 , "Amount must be greater than 0"},
            new object[] { 12345678, "12-34-56", 12345678, "12-34-56", -5 , "Amount must be greater than 0"},
            new object[] { 12345678, "", 12345678, "12-34-56", 5 , "SortCode must not be null or empty"},
            new object[] { 12345678, null, 12345678, "12-34-56", 5 , "SortCode must not be null or empty"},
            new object[] { 12345678, "12-34-56", 12345678, "", 5 , "SortCode must not be null or empty"},
            new object[] { 12345678, "12-34-56", 12345678, null, 5 , "SortCode must not be null or empty"},
            new object[] { 0, "12-34-56", 12345678, "12-34-56", 5 , "AccountNumber must be greater than 0"},
            new object[] { -5, "12-34-56", 12345678, "12-34-56", 5 , "AccountNumber must be greater than 0" },
            new object[] { 12345678, "12-34-56", 0, "12-34-56", 5 , "AccountNumber must be greater than 0"},
            new object[] { 12345678, "12-34-56", -5, "12-34-56", 5 , "AccountNumber must be greater than 0" },
            new object[] { 12345678, "12-34-56", 12345678, "12-34556", 5 , "SortCode has incorrect format, expected format : 11-11-11" },
            new object[] { 12345678, "12-34-56", 12345678, "12534-56", 5 , "SortCode has incorrect format, expected format : 11-11-11" },
            new object[] { 12345678, "123456", 12345678, "12-34-56", 5 , "SortCode must be 8 characters in length" },
            new object[] { 12345678, "invalid", 12345678, "12-34-56", 5 , "SortCode must be 8 characters in length"},
            new object[] { 12345678, "12-34-56", 12345678, "123456", 5 , "SortCode must be 8 characters in length" },
            new object[] { 12345678, "12-34-56", 12345678, "invalid", 5 , "SortCode must be 8 characters in length"},
            new object[] { 1, "12-34-56", 12345678, "12-34-56", 5 , "AccountNumber must be 8 digits in length" },
            new object[] { 123456789, "12-34-56", 12345678, "12-34-56", 5 , "AccountNumber must be 8 digits in length" },
            new object[] { 12345678, "12-34-56", 1, "12-34-56", 5 , "AccountNumber must be 8 digits in length" },
            new object[] { 12345678, "12-34-56", 123456789, "12-34-56", 5 , "AccountNumber must be 8 digits in length" }
        };

        [Theory, MemberData(nameof(InvalidTransferArgumentData))]
        public void TransactionService_InvalidTransferArgument_ThrowsArgumentException(long fromAccountNumber, string fromSortCode, long toAccountNumber, string toSortCode, decimal Amount, string Message)
        {
            // Arrange
            var exceptionType = typeof(ArgumentException);
            var fromAccount = GetAccount(fromAccountNumber, fromSortCode, 0, 0);
            var toAccount = GetAccount(toAccountNumber, toSortCode, 0, 0);
            _accountDataStore.Setup(x => x.GetAccount(fromAccountNumber, fromSortCode)).Returns(fromAccount);
            _accountDataStore.Setup(x => x.GetAccount(toAccountNumber, toSortCode)).Returns(toAccount);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => _transactionService.ExecuteTransaction(fromAccountNumber, fromSortCode, toAccountNumber, toSortCode, Amount));

            Assert.Equal(Message, ex.Message);
        }

        public static IEnumerable<object[]> InvalidAccountData =>
        new List<object[]>
        {
            new object[] { 11111111, "99-99-99", 5 , $"Account could not be found with AccountNumber : {11111111} and SortCode : {"99-99-99"}"},
            new object[] { 99999999, "11-11-11", 5 , $"Account could not be found with AccountNumber : {99999999} and SortCode : {"11-11-11"}" }
        };

        [Theory, MemberData(nameof(InvalidAccountData))]
        public void TransactionService_InValidAccountData_ThrowsDataException(long AccountNumber, string SortCode, decimal Amount, string Message)
        {
            //Arrange
            _accountDataStore.Setup(x => x.GetAccount(It.IsAny<long>(), It.IsAny<string>())).Returns(value: null);
            var exceptionType = typeof(InvalidDataException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => _transactionService.ExecuteTransaction(eTransactionType.Add, AccountNumber, SortCode, Amount));

            Assert.Equal(Message, ex.Message);
        }
        public static IEnumerable<object[]> InvalidTransferAccountData =>
        new List<object[]>
        {
            new object[] { 11111111, "99-99-99", 11111211, "99-55-99", 5 , $"Account could not be found with AccountNumber : {11111111} and SortCode : {"99-99-99"}"},
            new object[] { 99999999, "11-11-11", 11141111, "44-99-99", 5 , $"Account could not be found with AccountNumber : {99999999} and SortCode : {"11-11-11"}" }
        };

        [Theory, MemberData(nameof(InvalidTransferAccountData))]
        public void TransactionService_InValidTransferAccountData_ThrowsDataException(long fromAccountNumber, string fromSortCode, long toAccountNumber, string toSortCode, decimal Amount, string Message)
        {
            //Arrange
            _accountDataStore.Setup(x => x.GetAccount(It.IsAny<long>(), It.IsAny<string>())).Returns(value: null);
            var exceptionType = typeof(InvalidDataException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => _transactionService.ExecuteTransaction(fromAccountNumber, fromSortCode, toAccountNumber, toSortCode, Amount));

            Assert.Equal(Message, ex.Message);
        }
    }
}