namespace Billing.DAL.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public long Rating { get; set; }
        public ICollection<Coin> Coins { get; set; } = new List<Coin>();
    }
}
