using Billing.BLL.Helpers.Interfaces;
using Billing.BLL.Helpers.Models;
using Billing.DAL.Models;
using Billing.DAL.Repositories.Interfaces;

namespace Billing.BLL.DataManagement
{
    public class CoinsManagement
    {
        private readonly IUnitOfWork repos;
        private readonly ICoinsRepository coinsRepo;
        private readonly IUsersRepository usersRepo;
        private readonly IHistoriesRepository historiesRepo;
        private readonly ICalculatingRewards calculatingRewards;

        public CoinsManagement(IUnitOfWork unitOfWork, ICalculatingRewards calculatingRewards)
        {
            repos = unitOfWork;
            this.coinsRepo = repos.CoinsRepository;
            this.historiesRepo = repos.HistoriesRepository;
            this.usersRepo = repos.UsersRepository;
            this.calculatingRewards = calculatingRewards;
        }

        public async Task CoinsEmission(long amount)
        {
            IEnumerable<User> users = await usersRepo.GetAll();
            long userCount = await usersRepo.Count();

            foreach(RewardInfo info in calculatingRewards.GetRewardsInfo(users, userCount, amount))
            {
                for (long i = 0; i < info.Reward; i++)
                {
                    await MakeCoin(info.User);
                }
            }
        }

        private async Task MakeCoin(User user)
        {
            Coin coin = new Coin() { UserId = user.Id};
            await coinsRepo.Add(coin);

            History history = new History() { CoinId = coin.Id, ToUserId = user.Id };
            await historiesRepo.Add(history);

            await repos.SaveChanges();
        }
        
    }
}
