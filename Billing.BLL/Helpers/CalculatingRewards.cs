using Billing.DAL.Models;
using Billing.BLL.Helpers.Interfaces;
using Billing.BLL.Helpers.Models;

namespace Billing.BLL.Helpers
{
    public class CalculatingRewards : ICalculatingRewards
    {
        public IEnumerable<RewardInfo> GetRewardsInfo(IEnumerable<User> users, long userCount, long totalReward)
        {
            if (userCount > totalReward)
                //TODO: Return failure
                throw new Exception();

            long ratingSum = users.Sum(x => x.Rating);
            long coinsBalance = totalReward;
            double coinCost = (double)ratingSum / totalReward;
            ICollection<RewardInfo> rewards = new List<RewardInfo>();

            foreach (User user in users)
            {
                RewardInfo rewardInfo = AccrualRewardByRating(rewards, user, coinCost);
                coinsBalance -= rewardInfo.Reward;
            }

            IEnumerable<RewardInfo> orderedRewards = rewards!.Where(x => x.Reward > 0).OrderByDescending(x => x.Rating);
            IEnumerator<RewardInfo> enumerator = orderedRewards.GetEnumerator();

            while (coinsBalance > 0)
            {
                if (!enumerator.MoveNext())
                {
                    enumerator = orderedRewards.GetEnumerator();
                    enumerator.MoveNext();
                }

                enumerator.Current.Reward += 1;
                coinsBalance -= 1;
            }

            return rewards;
        }

        private RewardInfo AccrualRewardByRating(ICollection<RewardInfo>? ratingRemainders,
                                                 User user,
                                                 double coinCost)
        {
            long reward = (long)(user.Rating / coinCost);
            double ratingRemainder = user.Rating - reward * coinCost;
            RewardInfo rewardInfo = user.Rating < coinCost? new RewardInfo(user, 0, 1) : new RewardInfo(user, ratingRemainder, reward);
            ratingRemainders!.Add(rewardInfo);
            return rewardInfo;
        }
    }
}
