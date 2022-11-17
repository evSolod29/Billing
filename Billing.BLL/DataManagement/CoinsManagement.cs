using System.Collections;
using Billing.DAL.Models;
using Billing.DAL.Repositories.Interfaces;

namespace Billing.BLL.DataManagement
{
    public class CoinsManagement
    {
        private readonly ICoinsRepository coinsRepo;
        private readonly IUsersRepository usersRepo;
        public CoinsManagement(ICoinsRepository coinsRepo, IUsersRepository usersRepo)
        {
            this.usersRepo = usersRepo;
            this.coinsRepo = coinsRepo;
        }

        public async Task CoinsEmission(long amount)
        {
            IEnumerable<User> users = await usersRepo.Get();
            int userCount = users.Count();

            if (userCount > amount)
                //TODO: Return failure
                return;

            long ratingSum = users.Sum(x => x.Rating);
            long coinsBalance = amount;
            double coinCost = (double)ratingSum / (amount - userCount);
            bool isDistributionEnabled = coinsBalance - userCount > 0;
            ICollection<RatingRemainder>? ratingRemainders = 
                isDistributionEnabled ? new List<RatingRemainder>() : null;

            foreach (User user in users)
            {
                if (isDistributionEnabled)
                {
                    long award = (long)(user.Rating / coinCost);
                    double ratingRemainder = user.Rating - award * coinCost;
                    ratingRemainders!.Add(new RatingRemainder(user, ratingRemainder));
                    for (int i = 0; i < award; i++)
                    {
                        await MakeCoin(user);
                    }
                    coinsBalance -= award;
                }
                await MakeCoin(user);
                coinsBalance -= 1;
            }

            if(coinsBalance == 0)
                return;

            IEnumerable<RatingRemainder> orderedRR 
                = ratingRemainders!.OrderByDescending(x => x.Remainder);
            IEnumerator<RatingRemainder> enumerator = orderedRR.GetEnumerator();

            while(coinsBalance > 0)
            {
                if(!enumerator.MoveNext())
                {
                    enumerator = orderedRR.GetEnumerator();
                    enumerator.MoveNext();
                }

                await MakeCoin(enumerator.Current.User);
                coinsBalance -= 1;
            }
        }


        private async Task<Coin> MakeCoin(User user)
        {
            Coin coin = new Coin(user);
            await coinsRepo.Add(coin);
            //TODO: Make new history
            return coin;
        }
    }

    public class RatingRemainder
    {
        public RatingRemainder(User user, double remainder)
        {
            User = user;
            Remainder = remainder;
        }
        public int ta { get; set; }
        public User User { get; set; }
        public double Remainder { get; set; }
    }


}
