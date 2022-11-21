using Billing.BLL.DTO;
using Billing.DAL.Models;

namespace Billing.BLL.Extensions
{
    public static class CoinExtensions
    {
        public static CoinDTO ToDTO(this Coin coin)
            => new CoinDTO(coin.Id, string.Join(", ", coin.Histories.Select(h => h.ToUser.Name)));
    }
}
