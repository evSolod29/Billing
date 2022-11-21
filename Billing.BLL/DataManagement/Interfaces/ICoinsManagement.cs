using Billing.BLL.DTO;

namespace Billing.BLL.DataManagement.Interfaces
{
    public interface ICoinsManagement
    {
        Task CoinsEmission(long amount);
        Task<CoinDTO> LongestHistoryCoin();
        Task MoveCoinByUserName(string srcUsrName, string dstUserName, long amount);
    }
}