namespace Billing.DAL.Models
{
    public class Coin
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<History> Histories { get; set; } = new List<History>();
    }
}
