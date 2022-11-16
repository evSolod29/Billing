namespace Billing.DAL.Models
{
    public class User
    {
        public User(string name, long rating)
        {
            Name = name;
            Rating = rating;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long Rating { get; set; }
        public ICollection<Coin> Coins { get; set; } = new List<Coin>();
    }

    public class Coin
    {
        public Coin(User user)
        {
            User = user;
        }

        public long Id { get; set; }
        public User User { get; set; }
        public ICollection<History> History { get; set; } = new List<History>();
    }

    public class History
    {
        public History(User user, Coin coin)
        {
            User = user;
            Coin = coin;
        }

        public long Id { get; set; }
        public User User { get; set; }
        public Coin Coin { get; set; }
    }
}
