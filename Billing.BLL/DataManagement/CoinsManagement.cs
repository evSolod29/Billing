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
            ICollection<RatingRemainder> remainders = new List<RatingRemainder>();
            long userCount = 0;
            long ratingSum = users.Sum(x => {
                userCount += 1;
                return x.Rating; 
            });
            double coinCost = ratingSum/amount;
            long coinsCount = amount;
            if (userCount > amount)
                //TODO: Return failure
                return;

            foreach (User user in users)
            {
                long coinCount = (long) Math.Truncate(user.Rating / coinCost);
                coinCount = coinCount > amount - userCount ? 1 : coinCount;
                coinCount = coinCount < 1 ? 1 : coinCount;
                double remainder = (user.Rating - coinCount * coinCost);

                if(remainder > 0)
                    remainders.Add(new RatingRemainder(user, remainder));

                for (long i = 0; i < coinCount; i++)
                {
                    await MakeCoin(user);
                }
                coinsCount -= coinCount;
            }
            IEnumerable<RatingRemainder> orderedRemainders = remainders.OrderByDescending(x => x.Remainder);
            IEnumerator<RatingRemainder> enumerator = orderedRemainders.GetEnumerator();
            while(coinsCount > 0)
            {
                if(!enumerator.MoveNext())
                {
                    enumerator = orderedRemainders.GetEnumerator();
                    enumerator.MoveNext();
                }
                    


                await MakeCoin(enumerator.Current.User);
                coinsCount -= 1;
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

        public User User { get; set; }
        public double Remainder { get; set; }
    }


}
