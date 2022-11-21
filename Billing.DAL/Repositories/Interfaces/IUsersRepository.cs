using Billing.DAL.Models;

namespace Billing.DAL.Repositories.Interfaces
{
    public interface IUsersRepository:IGenericRepository<User>
    {
        Task<long> Count();
        Task<User?> GetByName(string name);
    }
}
