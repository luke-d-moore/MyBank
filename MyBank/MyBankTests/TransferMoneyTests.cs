using Moq;
using MyBank;

namespace MyBankTests
{
    public class TransferMoneyTests
    {
        [Fact]
        public void TransactionService_ValidTransferAmount_ReturnsTrue()
        {
            //Arrange
            var fromAccount = GetAccount(12345678, "12-34-56", 0, 0);
            var toAccount = GetAccount(12345555, "12-34-55", 0, 0);
            var result = TransferMoney.TransferFunds(fromAccount, toAccount, 5);
            //Act and Assert
            Assert.True(result);
        }

        private Account GetAccount(long AccountNumber, string SortCode, decimal PaidIn, decimal Withdrawn)
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
        public static IEnumerable<object[]> ExceedLimitData =>
        new List<object[]>
        {
            new object[] { 5 , "Amount would exceed withdrawn limit"},
            new object[] { 1000 , "Amount would exceed withdrawn limit"},
            new object[] { 2 , "Amount would exceed withdrawn limit" }
        };

        [Theory, MemberData(nameof(ExceedLimitData))]
        public void TransferMoney_AmountExceedsLimit_ThrowsInvalidOperationException(decimal Amount, string Message)
        {
            //Arrange
            var fromAccount = GetAccount(12345678, "12-34-56", 0, 999);
            var toAccount = GetAccount(12345555, "12-34-55", 0, 0);
            var exceptionType = typeof(InvalidOperationException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => TransferMoney.TransferFunds(fromAccount, toAccount, Amount));

            Assert.Equal(Message, ex.Message);
        }
        public static IEnumerable<object[]> InsufficientFundsData =>
        new List<object[]>
        {
            new object[] { 501 , "Account has insufficient funds to make transfer" },
            new object[] { 999 , "Account has insufficient funds to make transfer" }
        };

        [Theory, MemberData(nameof(InsufficientFundsData))]
        public void TransferMoney_AmountExceedsBalance_ThrowsInvalidOperationException(decimal Amount, string Message)
        {
            //Arrange
            var fromAccount = GetAccount(12345678, "12-34-56", 0, 0);
            var toAccount = GetAccount(12345555, "12-34-55", 0, 0);
            var exceptionType = typeof(InvalidOperationException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => TransferMoney.TransferFunds(fromAccount, toAccount, Amount));

            Assert.Equal(Message, ex.Message);
        }
    }
}