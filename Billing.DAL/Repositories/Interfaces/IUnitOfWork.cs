using Billing.DAL.Repositories.Interfaces;

namespace Billing.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICoinsRepository CoinsRepository { get; }
        IHistoriesRepository HistoriesRepository { get; }
        IUsersRepository UsersRepository { get; }

        Task SaveChanges();
    }
}