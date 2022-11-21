namespace Billing.BLL.DTO
{
    public class CoinDTO
    {
        public CoinDTO(long id, string history)
        {
            Id = id;
            History = history;
        }

        public long Id { get; set; }
        public string History { get; set; }
    }
}
