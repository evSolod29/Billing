using Billing.DAL.Contexts;
using Billing.DAL.Models;
using Billing.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Billing.DAL.Repositories.MemoryRepositories
{
    public class CoinsRepository : GenericRepository<Coin>, ICoinsRepository
    {
        public CoinsRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Coin>> GetAllByUsersId(long userId)
            => await _db.Where(u => u.UserId == userId).ToListAsync();

        public async Task<IEnumerable<Coin>> TakeCoins(long userId, long count)
            => await _db.Where(u => u.UserId == userId).Take((int) count).ToListAsync();
        public async Task<long> GetUserCoinAmount(long userId)
            => await _db.LongCountAsync(u => u.UserId == userId);
    }
}
