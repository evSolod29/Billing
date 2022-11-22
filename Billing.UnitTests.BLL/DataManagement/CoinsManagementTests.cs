using Billing.BLL.DataManagement;
using Billing.BLL.DTO;
using Billing.BLL.Exceptions;

using Billing.DAL.Models;

using Billing.UnitTests.BLL.Comparers;
using Billing.UnitTests.BLL.Fixtures;

using Microsoft.EntityFrameworkCore;

namespace Billing.UnitTests.BLL.DataManagement
{
    public class CoinsManagementTests: IDisposable
    {
        private readonly DatabaseFixture database;

        public CoinsManagementTests()
        {
            database = new DatabaseFixture(nameof(CoinsManagementTests));
        }

        public void Dispose()
        {
            database.Dispose();
        }
        
        [Fact]
        public async void CoinsEmission_CorrectCoinsAmountIsCharged()
        {
            database.DbContext.Add(new User() { Name = "boris", Rating = 5000 });
            database.DbContext.SaveChanges();

            long expected = 10;

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            await management.CoinsEmission(expected);
            long actual = database.DbContext.Coins.LongCount();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void CoinsEmission_CorrectHistoryAmountIsAdded()
        {
            database.DbContext.Add(new User() { Name = "boris", Rating = 5000 });
            database.DbContext.SaveChanges();

            long expected = 10;

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            await management.CoinsEmission(expected);

            long actual = database.DbContext.Histories.LongCount();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void CoinsEmission_ValidCoinAdded()
        {
            database.DbContext.Add(new User() { Name = "boris", Rating = 5000 });
            database.DbContext.SaveChanges();

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
            database.DbContext.Add(new User() { Name = "boris", Rating = 5000 });
            database.DbContext.SaveChanges();

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

        [Theory]
        [MemberData(nameof(CoinsEmissionData))]
        public async void CoinsEmission_CorrectCoinDistribution(IEnumerable<User> users,
                                                                IEnumerable<long> expected)
        {
            await database.DbContext.AddRangeAsync(users);
            await database.DbContext.SaveChangesAsync();

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            await management.CoinsEmission(expected.Sum());

            IEnumerable<long> actual = 
                database.DbContext.Users.Include(x => x.Coins).Select(x => x.Coins.LongCount());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void CoinsEmission_UserCountLessThanAmount_ThrowException()
        {
            await database.DbContext.AddRangeAsync(new List<User>()
            {
                new User() { Name = "boris", Rating = 1 },
                new User() { Name = "maria", Rating = 1 }
            });
            await database.DbContext.SaveChangesAsync();

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            Func<Task> coinsEmission = async () => await management.CoinsEmission(1);

            await Assert.ThrowsAnyAsync<WrongQuantityException>(coinsEmission);
        }

        [Fact]
        public async void MoveCoins_CoinsIsRemovedFromSourceUser()
        {
            database.DbContext.AddRange(new List<object>()
            {
                new User() { Name = "boris", Rating = 1 },
                new User() { Name = "maria", Rating = 1 },
                new Coin() { UserId = 1 },
                new Coin() { UserId = 1 }
            });
            database.DbContext.SaveChanges();

            long expected = 0;

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            await management.MoveCoinsByUserName("boris", "maria", 2);

            long actual = database.DbContext.Coins.Include(x => x.User)
                .LongCount(x => x.User.Name == "boris");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void MoveCoins_CoinsIsAddedToDestUser()
        {
            database.DbContext.AddRange(new List<object>()
            {
                new User() { Name = "boris", Rating = 1 },
                new User() { Name = "maria", Rating = 1 },
                new Coin() { UserId = 1 },
                new Coin() { UserId = 1 }
            });
            database.DbContext.SaveChanges();

            long expected = 2;

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            await management.MoveCoinsByUserName("boris", "maria", 2);

            long actual = database.DbContext.Coins.Include(x => x.User)
                .LongCount(x => x.User.Name == "maria");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void MoveCoins_UserInCoinIsChanged()
        {
            await database.DbContext.AddRangeAsync(new List<object>()
            {
                new User() { Name = "boris", Rating = 1 },
                new User() { Name = "maria", Rating = 1 },
                new Coin() { UserId = 1 }
            });
            await database.DbContext.SaveChangesAsync();

            IEqualityComparer<Coin> comparer = new CoinComparer();
            User user = new User() { Id = 2, Name = "maria", Rating = 1 };
            Coin expected = new Coin() { Id = 1, User = user, UserId = user.Id };

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            await management.MoveCoinsByUserName("boris", "maria", 1);

            Coin actual = await database.DbContext.Coins.FirstAsync();
            Assert.Equal(expected, actual, comparer);
        }

        [Fact]
        public async void MoveCoins_ValidHistoryAdded()
        {
            await database.DbContext.AddRangeAsync(new List<object>()
            {
                new User() { Name = "boris", Rating = 1 },
                new User() { Name = "maria", Rating = 1 },
                new Coin() { UserId = 1 }
            });
            await database.DbContext.SaveChangesAsync();

            IEqualityComparer<History> comparer = new HistoryComparer();
            User fromUser = new User() { Id = 1, Name = "boris", Rating = 1 };
            User toUser = new User() { Id = 2, Name = "maria", Rating = 1 };
            Coin coin = new Coin() { Id = 1, User = toUser, UserId = toUser.Id };
            History expected = new History() 
            { 
                Id = 1,
                Coin = coin,
                CoinId = coin.Id,
                FromUser = fromUser,
                FromUserId = fromUser.Id,
                ToUser = toUser,
                ToUserId = toUser.Id
            };

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            await management.MoveCoinsByUserName("boris", "maria", 1);

            History actual = await database.DbContext.Histories.FirstAsync();
            Assert.Equal(expected, actual, comparer);
        }

        [Fact]
        public async void MoveCoins_NotFoundSrcUser_ThrowNotFoundException()
        {
            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            Func<Task> coinsEmission = async () => await management.MoveCoinsByUserName("boris", "maria", 1);

            await Assert.ThrowsAnyAsync<NotFoundException>(coinsEmission);
        }

        [Fact]
        public async void MoveCoins_NotFoundDstUser_ThrowNotFoundException()
        {
            await database.DbContext.AddRangeAsync(new List<object>()
            {
                new User() { Name = "boris", Rating = 1 },
            });
            database.DbContext.SaveChanges();

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            Func<Task> moveCoins = async () => await management.MoveCoinsByUserName("boris", "maria", 1);

            await Assert.ThrowsAnyAsync<NotFoundException>(moveCoins);
        }

        [Fact]
        public async void MoveCoins_UserDoesDotHaveEnoughCoins_ThrowException()
        {
            await database.DbContext.AddRangeAsync(new List<object>()
            {
                new User() { Name = "boris", Rating = 1 },
                new User() { Name = "maria", Rating = 1 }
            });
            database.DbContext.SaveChanges();

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            Func<Task> moveCoins = async () => await management.MoveCoinsByUserName("boris", "maria", 1);

            await Assert.ThrowsAnyAsync<WrongQuantityException>(moveCoins);
        }

        [Fact]
        public async void LongestHistoryCoin_ReturnCoinDtoWithLongestHistory()
        {
            database.DbContext.AddRange(new List<object>()
            {
                new User() { Name = "boris", Rating = 1 },
                new User() { Name = "maria", Rating = 1 },
                new Coin() { UserId = 1 },
                new Coin() { UserId = 2 },
                new History() { CoinId = 1, ToUserId = 1 },
                new History() { CoinId = 2, ToUserId = 1 },
                new History() { CoinId = 1, FromUserId = 1, ToUserId = 2 },
                new History() { CoinId = 1, FromUserId = 2, ToUserId = 1 },
                new History() { CoinId = 2, FromUserId = 1, ToUserId = 2 },
            });
            database.DbContext.SaveChanges();

            IEqualityComparer<CoinDTO> comparer = new CoinDtoComparer();
            CoinDTO expected = new CoinDTO(1, "boris, maria, boris");

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            CoinDTO actual = await management.LongestHistoryCoin();

            Assert.Equal(expected, actual, comparer);
        }

        [Fact]
        public async void LongestHistoryCoin_NotFoundHistory_ThrowException()
        {
            database.DbContext.AddRange(new List<object>()
            {
                new User() { Name = "boris", Rating = 1 },
                new User() { Name = "maria", Rating = 1 }
            });
            database.DbContext.SaveChanges();

            CoinsManagement management = new CoinsManagement(database.UnitOfWork);
            Func<Task> longestHistoryCoin = async () => await management.LongestHistoryCoin();

            await Assert.ThrowsAnyAsync<NotFoundException>(longestHistoryCoin);
        }


        private static IEnumerable<object[]> CoinsEmissionData()
        {
            return new List<object[]>()
            {
                new object[]
                {
                    new List<User>()
                    {
                        new User() { Name = "boris", Rating = 5000 },
                        new User() { Name = "maria", Rating = 1000 },
                        new User() { Name = "oleg", Rating = 800 }
                    },
                    new List<long>() { 7, 2, 1}
                },
                new object[]
                {
                    new List<User>()
                    {
                        new User() { Name = "boris", Rating = 5000 },
                        new User() { Name = "maria", Rating = 1000 },
                        new User() { Name = "oleg", Rating = 800 }
                    },
                    new List<long>() { 1, 1, 1 }
                },
                new object[]
                {
                    new List<User>()
                    {
                        new User() { Name = "boris", Rating = 5000 },
                        new User() { Name = "maria", Rating = 1000 },
                        new User() { Name = "oleg", Rating = 800 }
                    },
                    new List<long>() { 6, 1, 1 }
                },
                new object[]
                {
                    new List<User>()
                    {
                        new User() { Name = "boris", Rating = 1 },
                        new User() { Name = "maria", Rating = 0 },
                    },
                    new List<long>() { 1, 1 }
                },
                new object[]
                {
                    new List<User>()
                    {
                        new User() { Name = "boris", Rating = 5000 },
                        new User() { Name = "maria", Rating = 5000 },
                        new User() { Name = "oleg", Rating = 800 }
                    },
                    new List<long>() { 4, 3, 1 }
                }
            };
        }
    }
}