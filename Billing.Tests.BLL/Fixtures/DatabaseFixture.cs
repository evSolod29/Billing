using Billing.DAL.Contexts;
using Billing.DAL.Repositories.EFCore;
using Billing.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Billing.Tests.BLL.Fixtures
{
    public class DatabaseFixture: IDisposable
    {
        private readonly BillingContext context;
        private readonly IUnitOfWork unitOfWork;

        public DatabaseFixture()
        {
            DbContextOptionsBuilder options = new DbContextOptionsBuilder<BillingContext>();
            options.UseInMemoryDatabase("Billing");
            context = new BillingContext(options.Options);
            context.Database.EnsureCreated();
            unitOfWork = new UnitOfWork(context);
        }

        public BillingContext DbContext => context;

        public IUnitOfWork UnitOfWork => unitOfWork;

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            unitOfWork.Dispose();
        }
    }
}