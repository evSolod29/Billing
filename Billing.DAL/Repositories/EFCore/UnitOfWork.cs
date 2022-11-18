using Billing.DAL.Contexts;
using Billing.DAL.Repositories.Interfaces;
using Billing.DAL.Repositories.MemoryRepositories;

namespace Billing.DAL.Repositories.EFCore
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly BillingContext context;
        private bool disposed;

        private ICoinsRepository? coinsRepository;
        private IHistoriesRepository? historiesRepository;
        private IUsersRepository? usersRepository;

        public UnitOfWork(BillingContext context)
        {
            this.context = context;
        }

        public ICoinsRepository CoinsRepository => coinsRepository ??= new CoinsRepository(context);
        public IHistoriesRepository HistoriesRepository =>
            historiesRepository ??= new HistoriesRepository(context);
        public IUsersRepository UsersRepository => usersRepository ??= new UsersRepository(context);

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task SaveChanges() => await context.SaveChangesAsync();
    }
}