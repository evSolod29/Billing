using Billing.DAL.Models;

namespace Billing.BLL.Helpers.Models
{
    public class RewardInfo
    {
        public RewardInfo(User user, double rating, long reward)
        {
            User = user;
            Rating = rating;
            Reward = reward;
        }
        public User User { get; set; }
        public double Rating { get; set; }
        public long Reward { get; set; }
    }
}
