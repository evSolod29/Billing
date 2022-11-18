using Billing.DAL.Models;
using Billing.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Billing.DAL.Repositories.MemoryRepositories;

public class HistoriesRepository : GenericRepository<History>, IHistoriesRepository
{
    public HistoriesRepository(DbContext context) : base(context)
    {
    }
}
