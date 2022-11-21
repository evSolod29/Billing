using Billing.BLL.DataManagement;
using Billing.DAL.Models;
using Billing.Tests.BLL.Fixtures;
using Billing.Tests.BLL.Comparers;
using Billing.BLL.DTO;

namespace Billing.Tests.BLL.DataManagement
{
    public class CoinsManagementTests: IDisposable
    {
        private readonly DatabaseFixture database;

        public CoinsManagementTests()
        {
            database = new DatabaseFixture();
            database.DbContext.Add(new User() { Name = "boris", Rating = 5000 });
            database.DbContext.SaveChanges();
            
        }

        public void Dispose()
        {
            database.Dispose();
        }
        
        [Fact]
        public async void CoinsEmission_CorrectCoinsAmountIsCharged()
        {
            long expected = 10;

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            await management.CoinsEmission(expected);
            long actual = database.DbContext.Coins.LongCount();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void CoinsEmission_CorrectHistoryAmountIsAdded()
        {
            long expected = 10;

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            await management.CoinsEmission(expected);

            long actual = database.DbContext.Histories.LongCount();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void CoinsEmission_ValidCoinAdded()
        {
            IEqualityComparer<Coin> comparer = new CoinComparer();
            User coinUser = database.UnitOfWork.UsersRepository.GetAllAsQueryable().First();
            Coin expected = new Coin() { User = coinUser, UserId = coinUser.Id, Id = 1 };

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            await management.CoinsEmission(1);

            Coin actual = database.DbContext.Coins.First();
            Assert.Equal(expected, actual, comparer);
        }

        [Fact]
        public async void CoinsEmission_ValidHistoryAdded()
        {
            IEqualityComparer<History> comparer = new HistoryComparer();
            User coinUser = database.UnitOfWork.UsersRepository.GetAllAsQueryable().First();
            Coin coin = new Coin() { User = coinUser, UserId = coinUser.Id, Id = 1 };
            History expected = new History() 
            { 
                Id = 1, 
                Coin = coin, 
                CoinId = coin.Id, 
                ToUser = coinUser, 
                ToUserId = coinUser.Id
            };

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            await management.CoinsEmission(1);

            History actual = database.DbContext.Histories.First();
            Assert.Equal(expected, actual, comparer);
        }

        private static IEnumerable<RewardInfo> GetRewardsInfo(IEnumerable<User> users, long totalReward)
            => new List<RewardInfo>() { new RewardInfo(users.First(), 0, totalReward) };
    }
}