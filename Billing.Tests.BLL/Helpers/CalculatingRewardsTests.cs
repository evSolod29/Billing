using Billing.DAL.Models;
using Xunit;

namespace Billing.Tests.BLL.Helpers
{
    public class CalculatingRewardsTests
    {
        [Fact]
        public void Test1()
        {
            var users = new List<User>()
            {
                new User() {Name = "boris", Rating = 5000},
                new User() {Name = "maria", Rating = 1000},
                new User() {Name = "oleg", Rating = 800},
            };
        }
    }
}