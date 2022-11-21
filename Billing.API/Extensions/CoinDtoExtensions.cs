using Billing.BLL.DTO;

namespace Billing.API.Extensions
{
    public static class CoinDtoExtensions
    {
        public static Coin ToCoin(this CoinDTO coin) => new Coin() { Id = coin.Id, History = coin.History };
    }
}
