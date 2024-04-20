using Moq;
using MyBank;

namespace MyBankTests
{
    public class WithdrawMoneyTests
    {
        [Fact]
        public void WithdrawMoney_ValidAmount_ReturnsTrue()
        {
            //Arrange
            long accountNumber = 12345678;
            var sortCode = "12-34-56";
            var account = GetAccount(accountNumber, sortCode, 0, 0, 200);
            var result = WithdrawMoney.WithdrawFunds(account, 5);
            //Act and Assert
            Assert.True(result);
        }

        private Account GetAccount(long AccountNumber, string SortCode, decimal PaidIn, decimal Withdrawn, decimal Balance)
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
        public static IEnumerable<object[]> ExceedLimitData =>
        new List<object[]>
        {
            new object[] { 12345678, "12-34-56", 5 , "Amount would exceed withdrawn limit"},
            new object[] { 12345678, "12-34-56", 350 , "Amount would exceed withdrawn limit"},
            new object[] { 12345678, "12-34-56", 2 , "Amount would exceed withdrawn limit" }
        };

        [Theory, MemberData(nameof(ExceedLimitData))]
        public void WithdrawMoney_AmountExceedsLimit_ThrowsInvalidOperationException(long AccountNumber, string SortCode, decimal Amount, string Message)
        {
            //Arrange
            var account = GetAccount(AccountNumber, SortCode, 0, 299, 200);
            var exceptionType = typeof(InvalidOperationException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => WithdrawMoney.WithdrawFunds(account, Amount));

            Assert.Equal(Message, ex.Message);
        }
        public static IEnumerable<object[]> InsufficientFundsData =>
        new List<object[]>
        {
            new object[] { 12345678, "12-34-56", 201 , "Account has insufficient funds to make withdrawal" },
            new object[] { 12345678, "12-34-56", 299 , "Account has insufficient funds to make withdrawal" }
        };

        [Theory, MemberData(nameof(InsufficientFundsData))]
        public void WithdrawMoney_AmountExceedsBalance_ThrowsInvalidOperationException(long AccountNumber, string SortCode, decimal Amount, string Message)
        {
            //Arrange
            var account = GetAccount(AccountNumber, SortCode, 0, 0, 200);
            var exceptionType = typeof(InvalidOperationException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => WithdrawMoney.WithdrawFunds(account, Amount));

            Assert.Equal(Message, ex.Message);
        }
    }
}