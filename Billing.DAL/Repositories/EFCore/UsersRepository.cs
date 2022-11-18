using Billing.DAL.Contexts;
using Billing.DAL.Repositories.Interfaces;
using Billing.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Billing.DAL.Repositories.MemoryRepositories
{
    public class UsersRepository : GenericRepository<User>, IUsersRepository
    {
        public UsersRepository(DbContext context) : base(context)
        {
        }

        public async Task<long> Count() => await _db.LongCountAsync();
    }
}