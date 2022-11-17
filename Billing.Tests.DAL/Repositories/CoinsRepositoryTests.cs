using Billing.DAL.Contexts;
using Billing.DAL.Models;
using Billing.DAL.Repositories.MemoryRepositories;

using Billing.Tests.DAL.Comparers;

namespace Billing.Tests.DAL.Repositories
{
    public class CoinsRepositoryTests
    {
        private readonly MemoryContext context;

        public CoinsRepositoryTests()
        {
            context = MakeContext();
        }

        #region AddTests
        [Fact]
        public async void Add_AddedCoinToContext()
        {
            

            CoinsRepository coinsRepository = new CoinsRepository(context);

            Coin expected = new Coin(context.Users.First(x => x.Id == 1)) { Id = 1 };

            User user = new User("", 0){Id = 1};
            Coin coin = new Coin(user);

            await coinsRepository.Add(coin);
            Coin actual = context.Coins.Last();

            Assert.Equal(expected, actual, new CoinEqualityComparer());
        }

        [Fact]
        public async void Add_UserKeyNotFound()
        {
            CoinsRepository coinsRepository = new CoinsRepository(context);

            string expectedMessage = "User key isn't found. Key is 10";
            User user = new User("", 0) {Id = 10};
            Coin coin = new Coin(user);

            Func<Task> testMethod = () => coinsRepository.Add(coin);
            
            KeyNotFoundException exception = await Assert.ThrowsAsync<KeyNotFoundException>(testMethod);
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public async void Add_UpdateArgCoinToActualState()
        {

            CoinsRepository coinsRepository = new CoinsRepository(context);

            Coin expected = new Coin(context.Users.First(x => x.Id == 1)) { Id = 1 };

            User user = new User("", 0) {Id = 1};
            Coin coin = new Coin(user);

            await coinsRepository.Add(coin);

            Assert.Equal(expected, coin, new CoinEqualityComparer());
        }

        [Fact]
        public async void Add_NotEqualReference()
        {
            CoinsRepository coinsRepository = new CoinsRepository(context);

            User user = new User("", 0) {Id = 1};
            Coin expected = new Coin(user);

            await coinsRepository.Add(expected);
            Coin actual = context.Coins.Last(x => x.Id == 1 );

            Assert.NotEqual(expected, actual);
        }
        #endregion

        #region UpdateTests
        [Fact]
        public async void Update_UpdateEntryInCollection()
        {
            long mariaUserId = 2;
            long coinId = 1;

            User user = new User("maria", 1000) { Id = mariaUserId };
            Coin expected = new Coin(user) {Id = coinId};

            context.Coins.Add(new Coin(context.Users.First()) { Id = coinId });
            context.Users.Add(user);
            CoinsRepository coinsRepository = new CoinsRepository(context);

            Coin coin = new Coin(new User("", 0) { Id = mariaUserId}) {Id = coinId};

            await coinsRepository.Update(coin);
            Coin actual = context.Coins.First( x => x.Id == coinId);

            Assert.Equal(expected, actual, new CoinEqualityComparer());
        }

        [Fact]
        public async void Update_CoinKeyNotFound()
        {
            string expectedMessage = "Coin key isn't found. Key is 0";

            CoinsRepository coinsRepository = new CoinsRepository(context);

            Func<Task> update = () => coinsRepository.Update(new Coin(new User("", 0)));
            KeyNotFoundException exception = await Assert.ThrowsAsync<KeyNotFoundException>(update);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public async void Update_UserKeyNotFound()
        {
            long coinId = 1;
            string expectedMessage = "User key isn't found. Key is 0";
            Coin coin = new Coin(context.Users.First()) {Id = coinId};
            context.Coins.Add(coin);

            CoinsRepository coinsRepository = new CoinsRepository(context);

            Func<Task> update = () => coinsRepository.Update(new Coin(new User("", 0)) {Id = coinId});
            KeyNotFoundException exception = await Assert.ThrowsAsync<KeyNotFoundException>(update);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public async void Update_UpdateArgCoinToActualState()
        {

            long mariaUserId = 2;
            long coinId = 1;

            User user = new User("maria", 1000) { Id = mariaUserId };
            Coin expected = new Coin(user) {Id = coinId};

            context.Coins.Add(new Coin(context.Users.First()) { Id = coinId });
            context.Users.Add(user);
            CoinsRepository coinsRepository = new CoinsRepository(context);

            Coin coin = new Coin(new User("", 0) { Id = mariaUserId}) {Id = coinId};

            await coinsRepository.Update(coin);

            Assert.Equal(expected, coin, new CoinEqualityComparer());
        }

        [Fact]
        public async void Update_NotEqualReference()
        {
            long mariaUserId = 2;
            long coinId = 1;

            User user = new User("maria", 1000) { Id = mariaUserId };

            context.Coins.Add(new Coin(context.Users.First()) { Id = coinId });
            context.Users.Add(user);
            CoinsRepository coinsRepository = new CoinsRepository(context);

            Coin expected = new Coin(new User("", 0) { Id = mariaUserId}) {Id = coinId};

            await coinsRepository.Update(expected);
            var actual = context.Coins.First( x => x.Id == coinId );

            Assert.NotEqual(expected, actual);
        }
        #endregion

        private MemoryContext MakeContext()
        {
            ICollection<User> users = new List<User> { new User("boris", 5000) { Id = 1 }};
            ICollection<Coin> coins = new List<Coin>();
            ICollection<History> histories = new List<History>();
            return new MemoryContext(users, coins, histories);
        }
    }
}
