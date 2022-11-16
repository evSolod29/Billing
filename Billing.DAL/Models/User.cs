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
    }
}
