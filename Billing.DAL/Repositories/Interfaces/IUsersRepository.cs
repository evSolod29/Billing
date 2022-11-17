using Billing.DAL.Models;

namespace Billing.DAL.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<User?> Get(long id);
        Task<IEnumerable<User>> Get();
        Task Update(User user);
    }
}
