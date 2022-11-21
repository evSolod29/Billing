using Billing.BLL.Helpers;
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

            var calc = new CalculatingRewards();

            var a = calc.GetRewardsInfo(users, 3, 6799);
            var boris = a.Where(x => x.User.Name == "boris").Sum(x => x.Reward);
            var maria = a.Where(x => x.User.Name == "maria").Sum(x => x.Reward);
            var oleg = a.Where(x => x.User.Name == "oleg").Sum(x => x.Reward);
        }
    }
}