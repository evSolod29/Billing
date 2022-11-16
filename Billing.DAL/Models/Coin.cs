namespace Billing.DAL.Models
{
    public class Coin
    {
        public Coin(User user)
        {
            User = user;
        }

        public long Id { get; set; }
        public User User { get; set; }
    }
}
