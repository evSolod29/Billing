using Billing.BLL.DataManagement;
using Billing.DAL.Contexts;
using Billing.DAL.Models;
using Billing.DAL.Repositories.MemoryRepositories;

namespace Billing.Tests.BLL
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            MemoryContext context = MakeContext();
            CoinsManagement coinsManagement = new CoinsManagement(new CoinsRepository(context),
                                                                  new UsersRepository(context));
            await coinsManagement.CoinsEmission(10000);
            int boris = context.Coins.Where(x => x.User.Id == 1).Count();
            int maria = context.Coins.Where(x => x.User.Id == 2).Count();
            int oleg = context.Coins.Where(x => x.User.Id == 3).Count();
        }

        private MemoryContext MakeContext()
        {
            ICollection<User> users = new List<User> 
            { 
                new User("boris", 5000) { Id = 1 },
                new User("maria", 1000) { Id = 2 },
                new User("oleg", 800) { Id = 3 }
            };
            ICollection<Coin> coins = new List<Coin>();
            ICollection<History> histories = new List<History>();
            return new MemoryContext(users, coins, histories);
        }
    }
}