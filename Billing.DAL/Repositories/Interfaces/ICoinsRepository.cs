using Billing.DAL.Models;

namespace Billing.DAL.Repositories.Interfaces
{
    public interface ICoinsRepository
    {
        Task Add(Coin coin);
        Task<Coin> Get(long id);
        Task<IEnumerable<Coin>> Get();
        Task<IEnumerable<Coin>> GetByUserId(long userId);
        Task Update(Coin coin);
    }
}
