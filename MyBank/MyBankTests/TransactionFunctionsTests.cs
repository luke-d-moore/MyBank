using Moq;
using MyBank;

namespace MyBankTests
{
    public class TransactionFunctionsTests
    {
        [Fact]
        public void TransferFunds_ValidTransferAmount_ReturnsTrue()
        {
            //Arrange
            var fromAccount = GetTransferAccount(12345678, "12-34-56", 0, 0);
            var toAccount = GetTransferAccount(12345555, "12-34-55", 0, 0);
            var result = TransactionFunctions.TransferFunds(fromAccount, toAccount, 5);
            //Act and Assert
            Assert.True(result);
        }

        private Account GetTransferAccount(long AccountNumber, string SortCode, decimal PaidIn, decimal Withdrawn)
        {
            return new Account()
            {
                AccountNumber = AccountNumber,
                SortCode = SortCode,
                Balance = 500,
                Withdrawn = Withdrawn,
                PaidIn = PaidIn
            };
        }
        public static IEnumerable<object[]> ExceedTransferLimitData =>
        new List<object[]>
        {
            new object[] { 5 , "Amount would exceed withdrawn limit"},
            new object[] { 1000 , "Amount would exceed withdrawn limit"},
            new object[] { 2 , "Amount would exceed withdrawn limit" }
        };

        [Theory, MemberData(nameof(ExceedTransferLimitData))]
        public void TransferFunds_AmountExceedsTransferLimit_ThrowsInvalidOperationException(decimal Amount, string Message)
        {
            //Arrange
            var fromAccount = GetTransferAccount(12345678, "12-34-56", 0, 999);
            var toAccount = GetTransferAccount(12345555, "12-34-55", 0, 0);
            var exceptionType = typeof(InvalidOperationException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => TransactionFunctions.TransferFunds(fromAccount, toAccount, Amount));

            Assert.Equal(Message, ex.Message);
        }
        public static IEnumerable<object[]> InsufficientTransferFundsData =>
        new List<object[]>
        {
            new object[] { 501 , "Account has insufficient funds to make transfer" },
            new object[] { 999 , "Account has insufficient funds to make transfer" }
        };

        [Theory, MemberData(nameof(InsufficientTransferFundsData))]
        public void TransferFunds_AmountExceedsTransferBalance_ThrowsInvalidOperationException(decimal Amount, string Message)
        {
            //Arrange
            var fromAccount = GetTransferAccount(12345678, "12-34-56", 0, 0);
            var toAccount = GetTransferAccount(12345555, "12-34-55", 0, 0);
            var exceptionType = typeof(InvalidOperationException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => TransactionFunctions.TransferFunds(fromAccount, toAccount, Amount));

            Assert.Equal(Message, ex.Message);
        }
        [Fact]
        public void AddFunds_ValidAddAmount_ReturnsTrue()
        {
            //Arrange
            long accountNumber = 12345678;
            var sortCode = "12-34-56";
            var account = GetAddAccount(accountNumber, sortCode, 0, 0);
            var result = TransactionFunctions.AddFunds(account, 5);
            //Act and Assert
            Assert.True(result);
        }

        private Account GetAddAccount(long AccountNumber, string SortCode, decimal PaidIn, decimal Withdrawn)
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
        public static IEnumerable<object[]> ExceedAddLimitData =>
        new List<object[]>
        {
            new object[] { 12345678, "12-34-56", 5 , "Amount would exceed Pay in limit"},
            new object[] { 12345678, "12-34-56", 10000 , "Amount would exceed Pay in limit"},
            new object[] { 12345678, "12-34-56", 2 , "Amount would exceed Pay in limit" }
        };

        [Theory, MemberData(nameof(ExceedAddLimitData))]
        public void AddFunds_AmountExceedsAddLimit_ThrowsInvalidOperationException(long AccountNumber, string SortCode, decimal Amount, string Message)
        {
            //Arrange
            var account = GetAddAccount(AccountNumber, SortCode, 9999, 0);
            var exceptionType = typeof(InvalidOperationException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => TransactionFunctions.AddFunds(account, Amount));

            Assert.Equal(Message, ex.Message);
        }

        [Fact]
        public void WithdrawFunds_ValidWithdrawAmount_ReturnsTrue()
        {
            //Arrange
            long accountNumber = 12345678;
            var sortCode = "12-34-56";
            var account = GetWithdrawAccount(accountNumber, sortCode, 0, 0, 200);
            var result = TransactionFunctions.WithdrawFunds(account, 5);
            //Act and Assert
            Assert.True(result);
        }

        private Account GetWithdrawAccount(long AccountNumber, string SortCode, decimal PaidIn, decimal Withdrawn, decimal Balance)
        {
            return new Account()
            {
                AccountNumber = AccountNumber,
                SortCode = SortCode,
                Balance = Balance,
                Withdrawn = Withdrawn,
                PaidIn = PaidIn
            };
        }
        public static IEnumerable<object[]> ExceedWithdrawLimitData =>
        new List<object[]>
        {
            new object[] { 12345678, "12-34-56", 5 , "Amount would exceed withdrawn limit"},
            new object[] { 12345678, "12-34-56", 350 , "Amount would exceed withdrawn limit"},
            new object[] { 12345678, "12-34-56", 2 , "Amount would exceed withdrawn limit" }
        };

        [Theory, MemberData(nameof(ExceedWithdrawLimitData))]
        public void WithdrawFunds_AmountExceedsWithdrawLimit_ThrowsInvalidOperationException(long AccountNumber, string SortCode, decimal Amount, string Message)
        {
            //Arrange
            var account = GetWithdrawAccount(AccountNumber, SortCode, 0, 299, 200);
            var exceptionType = typeof(InvalidOperationException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => TransactionFunctions.WithdrawFunds(account, Amount));

            Assert.Equal(Message, ex.Message);
        }
        public static IEnumerable<object[]> InsufficientWithdrawFundsData =>
        new List<object[]>
        {
            new object[] { 12345678, "12-34-56", 201 , "Account has insufficient funds to make withdrawal" },
            new object[] { 12345678, "12-34-56", 299 , "Account has insufficient funds to make withdrawal" }
        };

        [Theory, MemberData(nameof(InsufficientWithdrawFundsData))]
        public void WithdrawFunds_AmountExceedsWithdrawBalance_ThrowsInvalidOperationException(long AccountNumber, string SortCode, decimal Amount, string Message)
        {
            //Arrange
            var account = GetWithdrawAccount(AccountNumber, SortCode, 0, 0, 200);
            var exceptionType = typeof(InvalidOperationException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => TransactionFunctions.WithdrawFunds(account, Amount));

            Assert.Equal(Message, ex.Message);
        }

    }
}