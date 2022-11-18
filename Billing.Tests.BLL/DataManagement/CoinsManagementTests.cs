using Billing.BLL.DataManagement;
using Billing.BLL.Helpers.Interfaces;
using Billing.BLL.Helpers.Models;
using Billing.DAL.Models;
using Billing.Tests.BLL.Fixtures;

namespace Billing.Tests.BLL.DataManagement
{
    public class CoinsManagementTests: IDisposable
    {
        private readonly DatabaseFixture database;
        private readonly Mock<ICalculatingRewards> calcMock;

        public CoinsManagementTests()
        {
            database = new DatabaseFixture();
            database.DbContext.Add(new User() { Name = "boris", Rating = 5000 });
            database.DbContext.SaveChanges();

            calcMock = new Mock<ICalculatingRewards>();
            calcMock.Setup(x => x.GetRewardsInfo(It.IsAny<IEnumerable<User>>(),
                                                 It.IsAny<long>(),
                                                 It.IsAny<long>()))
                    .Returns<IEnumerable<User>, long, long>((users, count, amount) => GetRewardsInfo(users,
                                                                                                     count,
                                                                                                     amount));
            
        }

        public void Dispose()
        {
            database.Dispose();
        }
        
        [Fact]
        public async void CoinsEmission_CorrectCoinsAmountIsCharged()
        {
            long expected = 10;

            ICalculatingRewards calc = calcMock.Object;
            CoinsManagement management = new CoinsManagement(database.UnitOfWork, calc);

            await management.CoinsEmission(expected);
            long actual = database.DbContext.Coins.LongCount();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void CoinsEmission_CorrectHistoryAmountIsAdded()
        {
            long expected = 10;

            CoinsManagement management = new CoinsManagement(database.UnitOfWork, calcMock.Object);
            await management.CoinsEmission(expected);

            long actual = database.DbContext.Histories.LongCount();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void CoinsEmission_ValidCoinAdded()
        {
            User coinUser = database.UnitOfWork.UsersRepository.GetAllAsQueryable().First();
            Coin expected = new Coin() { User = coinUser, UserId = coinUser.Id, Id = 1 };

            CoinsManagement management = new CoinsManagement(database.UnitOfWork, calcMock.Object);
            await management.CoinsEmission(1);

            Coin actual = database.DbContext.Coins.First();
            Assert.Equal(expected, actual);
        }

        private IEnumerable<RewardInfo> GetRewardsInfo(IEnumerable<User> users, long userCount, long totalReward)
            => new List<RewardInfo>() { new RewardInfo(users.First(), 0, totalReward) };
    }
}