namespace Billing.BLL.DTO
{
    public class UserDTO
    {
        public UserDTO(long id, string name, long amount)
        {
            Id = id;
            Name = name;
            Amount = amount;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long Amount { get; set; }
    }
}
