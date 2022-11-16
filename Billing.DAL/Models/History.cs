namespace Billing.DAL.Models
{
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
