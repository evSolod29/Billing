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
    }
}
