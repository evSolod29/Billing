namespace Billing.DAL.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public long Rating { get; set; }
    }
}
