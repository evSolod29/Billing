using Billing.BLL.DataManagement.Interfaces;
using Billing.BLL.DTO;
using Billing.BLL.Exceptions;
using Billing.BLL.Extensions;
using Billing.BLL.Helpers.Interfaces;
using Billing.BLL.Helpers.Models;
using Billing.DAL.Models;
using Billing.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Billing.BLL.DataManagement
{
    public class CoinsManagement : ICoinsManagement
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

            foreach (RewardInfo info in calculatingRewards.GetRewardsInfo(users, userCount, amount))
            {
                for (long i = 0; i < info.Reward; i++)
                {
                    await MakeCoin(info.User);
                }
            }

            await repos.SaveChanges();
        }

        public async Task MoveCoinByUserName(string srcUsrName, string dstUserName, long amount)
        {
            User srcUser = CheckUserByNull(await usersRepo.GetByName(srcUsrName));
            User dstUser = CheckUserByNull(await usersRepo.GetByName(dstUserName));

            if (amount > await coinsRepo.GetUserCoinAmount(srcUser.Id))
                throw new WrongQuantityException($"Wrong coin count. " +
                    $"User {srcUsrName} doesn't have that many coins.");

            foreach (Coin coin in await coinsRepo.TakeCoins(srcUser.Id, amount))
            {
                coin.UserId = dstUser.Id;
                coinsRepo.Update(coin);
                History history = new History() 
                { 
                    CoinId = coin.Id, 
                    FromUserId = srcUser.Id, 
                    ToUserId = dstUser.Id
                };
                await historiesRepo.Add(history);
            }
            await repos.SaveChanges();
        }

        public async Task<CoinDTO> LongestHistoryCoin()
        {
            var a = await coinsRepo.GetAllAsQueryable().Include(x => x.Histories)
                .OrderByDescending(x => x.Histories.Count()).ToListAsync();
            Coin? coin = await coinsRepo.GetAllAsQueryable().Include(x => x.Histories)
                .ThenInclude(x => x.ToUser).OrderByDescending(x => x.Histories.Count())
                .FirstOrDefaultAsync();

            if (coin == null)
                throw new NotFoundException("Coins were not charged.");

            return coin.ToDTO();
        }

        private User CheckUserByNull(User? user)
        {
            if (user == null)
                throw new NotFoundException("User is not found");

            return user;
        }

        private async Task MakeCoin(User user)
        {
            Coin coin = new Coin() { UserId = user.Id };
            await coinsRepo.Add(coin);

            History history = new History() { CoinId = coin.Id, ToUserId = user.Id };
            await historiesRepo.Add(history);
        }

    }
}
