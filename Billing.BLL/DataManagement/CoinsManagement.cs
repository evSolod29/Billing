using Billing.BLL.DataManagement.Interfaces;
using Billing.BLL.DTO;
using Billing.BLL.Exceptions;
using Billing.BLL.Extensions;
using Billing.DAL.Models;
using Billing.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Billing.BLL.DataManagement
{
    /// <summary>
    /// ????? ?????????? ????????
    /// </summary>
    public class CoinsManagement : ICoinsManagement
    {
        private readonly IUnitOfWork repos;
        private readonly ICoinsRepository coinsRepo;
        private readonly IUsersRepository usersRepo;
        private readonly IHistoriesRepository historiesRepo;

        public CoinsManagement(IUnitOfWork unitOfWork)
        {
            repos = unitOfWork;
            coinsRepo = repos.CoinsRepository;
            historiesRepo = repos.HistoriesRepository;
            usersRepo = repos.UsersRepository;
        }

        /// <summary>
        /// ?????????? ????? ???? ????????????? ? ?????? ????????, ?? ?? ????? ?????.
        /// </summary>
        /// <param name="amount">?????????? ????? ??? ??????????.</param>
        /// <returns></returns>
        /// <exception cref="WrongQuantityException">?????????? ????? ??????,
        /// ??? ?????????? ?????????????.</exception>
        public async Task CoinsEmission(long amount)
        {
            var users = await usersRepo.GetAll();

            if (await usersRepo.Count() > amount)
                throw new WrongQuantityException($"Wrong coin count. " +
                    $"The number of coins must not be less than count of users.");

            IEnumerable<RewardInfo> rewards = GetReward(users, amount);

            foreach (RewardInfo info in rewards)
            {
                for (int i = 0; i < info.Reward; i++)
                {
                    Coin coin = new Coin() { UserId = info.User.Id };
                    await coinsRepo.Add(coin);

                    History history = new History() { CoinId = coin.Id, ToUserId = info.User.Id };
                    await historiesRepo.Add(history);
                }
            }

            await repos.SaveChanges();
        }

        /// <summary>
        /// ??????????? ????? ????? ??????????????.
        /// </summary>
        /// <param name="srcUsrName">??? ???????????? ? ???????? ??????? ??????.</param>
        /// <param name="dstUserName">??? ???????????? ???????? ???????? ??????.</param>
        /// <param name="amount">?????????? ?????.</param>
        /// <returns></returns>
        /// <exception cref="WrongQuantityException">? ???????????? ???????????? ?????.</exception>
        public async Task MoveCoinsByUserName(string srcUsrName, string dstUserName, long amount)
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

        /// <summary>
        /// ????????? ?????? ? ????? ??????? ????????.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotFoundException">??? ??????? ?????.</exception>
        public async Task<CoinDTO> LongestHistoryCoin()
        {
            Coin? coin = await coinsRepo.GetAllAsQueryable().Include(x => x.Histories)
                .ThenInclude(x => x.ToUser).OrderByDescending(x => x.Histories.Count())
                .FirstOrDefaultAsync();

            if (coin == null)
                throw new NotFoundException("Coins were not charged.");

            return coin.ToDTO();
        }

        private static IEnumerable<RewardInfo> GetReward(IEnumerable<User> users, long coinsBalance)
        {
            double coinCost = (double)users.Sum(x => x.Rating) / coinsBalance;
            long maxReward = coinsBalance - users.Count(x => x.Rating < coinCost);

            ICollection<RewardInfo> rewards = users.Select(x =>
            {
                long reward = (long)Math.Truncate(x.Rating / coinCost);
                reward = reward > 1 ? reward : 1;
                reward = reward > maxReward ? maxReward : reward;
                coinsBalance -= reward;
                return new RewardInfo(x, x.Rating - (reward * coinCost), reward);
            }).ToList();

            IEnumerator<RewardInfo> enumerator = rewards.Where(x => x.Rating > 0)
                .OrderByDescending(x => x.Rating).GetEnumerator();

            while (coinsBalance > 0 && enumerator.MoveNext())
            {
                enumerator.Current.Reward++;
                coinsBalance--;
            }

            return rewards;
        }

        private static User CheckUserByNull(User? user)
        {
            if (user == null)
                throw new NotFoundException("User is not found");

            return user;
        }
    }
}
