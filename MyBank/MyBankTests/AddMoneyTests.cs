using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using MyBank;
namespace MyBankTests
{
    public class AddMoneyTests
    {
        [Fact]
        public void AddMoney_ValidAmount_ReturnsTrue()
        {
            //Arrange
            long accountNumber = 12345678;
            var sortCode = "12-34-56";
            var account = GetAccount(accountNumber, sortCode, 0, 0);
            var result = AddMoney.AddFunds(account, 5);
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
            var account = GetAccount(AccountNumber, SortCode, 9999, 0);
            var exceptionType = typeof(InvalidOperationException);
            // Act and Assert
            var ex = Assert.Throws(exceptionType, () => AddMoney.AddFunds(account, Amount));

            Assert.Equal(Message, ex.Message);
        }
    }
}