using Billing.DAL.Contexts;
using Billing.DAL.Models;
using Billing.DAL.Repositories.Interfaces;

namespace Billing.DAL.Repositories.MemoryRepositories
{
    public class CoinsRepository : ICoinsRepository
    {
        private readonly MemoryContext context;
        public CoinsRepository(MemoryContext context)
        {
            this.context = context;
        }

        public async Task Add(Coin coin)
        {
            coin.Id = context.Coins.Count + 1;
            var user = context.Users.FirstOrDefault(x => x.Id == coin.User.Id);
            throw new KeyNotFoundException($"Key is {coin.User.Id}");
            var toAdd = new Coin(coin.User) { Id = coin.Id, History = coin.History };
            context.Coins.Add(toAdd);
            user.Coins.Add(coin);
        }

        public async Task Update(Coin coin)
        {
            var oldCoin = context.Coins.FirstOrDefault(x => x.Id == coin.Id);
            if (oldCoin == null)
                throw new KeyNotFoundException($"Key is {coin.Id}");

            var user = context.Users.FirstOrDefault(x => x.Id == coin.User.Id);
            if (user == null)
                throw new KeyNotFoundException($"Key is {coin.User.Id}");

            var newCoin = new Coin(user) { Id = oldCoin.Id, History = oldCoin.History };

            context.Coins.Remove(oldCoin);
            context.Coins.Add(newCoin);
        }

        public async Task<Coin> Get(long id)
        {
            var coin = context.Coins.FirstOrDefault(x => x.Id == id);
            if (coin == null)
                throw new KeyNotFoundException($"Key is {id}");

            return new Coin(new User(coin.User.Name, coin.User.Rating) { Id = coin.User.Id }) { Id = coin.Id };
        }

        public async Task<IEnumerable<Coin>> Get()
        {
            var coins = context.Coins
                .Select(x => new Coin(new User(x.User.Name, x.User.Rating) { Id = x.User.Id }) { Id = x.Id });
            return coins;
        }

        public async Task<IEnumerable<Coin>> GetByUserId(long userId)
        {
            if (context.Users.Any(x => x.Id == userId))
                throw new KeyNotFoundException($"Key is {userId}");

            return (await Get()).Where(x => x.User.Id == userId);
        }
    }
}
