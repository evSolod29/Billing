using Billing.DAL.Contexts;
using Billing.DAL.Models;

namespace Billing.DAL.Repositories.MemoryRepositories
{
    public class CoinsRepository
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
            if(oldCoin == null)
                throw new KeyNotFoundException($"Key is {coin.Id}");

            var user = context.Users.FirstOrDefault(x => x.Id == coin.User.Id);
            if (user == null) 
                throw new KeyNotFoundException($"Key is {coin.User.Id}");

            var newCoin = new Coin(user) { Id = oldCoin.Id, History = oldCoin.History };

            context.Coins.Remove(oldCoin);
            context.Coins.Add(newCoin);
        }
    }
}
