using Billing.DAL.Models;
using Billing.BLL.Helpers.Models;

namespace Billing.BLL.Helpers.Interfaces
{
    public interface ICalculatingRewards
    {
        IEnumerable<RewardInfo> GetRewardsInfo(IEnumerable<User> users, long userCount, long totalReward);
    }
}
