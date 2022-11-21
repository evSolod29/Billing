using Billing.DAL.Models;

namespace Billing.DAL.Repositories.Interfaces
{
    public interface ICoinsRepository : IGenericRepository<Coin>
    {
        Task<IEnumerable<Coin>> GetAllByUsersId(long userId);
        Task<long> GetUserCoinAmount(long userId);
        Task<IEnumerable<Coin>> TakeCoins(long userId, long count);
    }
}
