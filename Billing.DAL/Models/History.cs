namespace Billing.DAL.Models
{
    public class History
    {
        public long Id { get; set; }
        public long CoinId { get; set; }
        public Coin Coin { get; set; } = null!;
        public long? FromUserId { get; set; }
        public User? FromUser { get; set; }
        public long ToUserId { get; set; }
        public User ToUser { get; set; } = null!;
    }
}
