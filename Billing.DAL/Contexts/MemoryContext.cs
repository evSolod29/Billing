using Billing.DAL.Models;

namespace Billing.DAL.Contexts
{
    public class MemoryContext
    {
        private static readonly ICollection<Coin> coins = new List<Coin>();
        private static readonly ICollection<History> histories = new List<History>();
        private static readonly ICollection<User> users = new List<User>() 
        { 
            new User("boris", 5000) { Id = 1},
            new User("maria", 1000) {Id = 2},
            new User("oleg", 800) {Id = 3}
        };

        public ICollection<Coin> Coins => coins;

        public ICollection<History> Histories => histories;

        public ICollection<User> Users => users;
    }
}
