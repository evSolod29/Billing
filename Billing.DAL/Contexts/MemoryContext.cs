using Billing.DAL.Models;

namespace Billing.DAL.Contexts
{
    public class MemoryContext
    {
        private static ICollection<Coin> coins = new List<Coin>();
        private static ICollection<History> histories = new List<History>();
        private static ICollection<User> users = new List<User>() 
        { 
            new User("boris", 5000) { Id = 1},
            new User("maria", 1000) {Id = 2},
            new User("oleg", 800) {Id = 3}
        };

        public MemoryContext()
        {
            
        }

        public MemoryContext(ICollection<User> users,
                             ICollection<Coin> coins,
                             ICollection<History> histories)
        {
            MemoryContext.coins = coins;
            MemoryContext.histories = histories;
            MemoryContext.users = users;
        }

        public ICollection<Coin> Coins => coins;

        public ICollection<History> Histories => histories;

        public ICollection<User> Users => users;
    }
}
