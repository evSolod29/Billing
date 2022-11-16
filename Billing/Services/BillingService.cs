using Grpc.Core;

namespace Billing.Services
{
    public class BillingService : Billing.BillingBase
    {
        private static ICollection<User> users = new List<User>
        {
            new User("boris", 5000),
            new User("maria", 1000),
            new User("oleg", 800)
        };
        private static ICollection<Coin> coins = new List<Coin>();
        public override async Task ListUsers(None request,
                                             IServerStreamWriter<UserProfile> responseStream,
                                             ServerCallContext context)
        {
            foreach (User user in users)
            {
                await responseStream.WriteAsync(user.ToUserProfile());
            }
            var a = coins.Count == users.Sum(x => x.Amount);
        }

        public override async Task<Response> CoinsEmission(EmissionAmount request, ServerCallContext context)
        {

            if (request.Amount < users.Count)
                return new Response()
                {
                    Status = Response.Types.Status.Failed,
                    Comment = "Кол-во монет не может быть меньше, чем кол-во пользователей."
                };

            double ratingCost = request.Amount / (double)users.Sum(x => x.Rating);
            var balance = request.Amount - users.Count;
            var awards = users.OrderByDescending(x => {
                AddCoin(x);
                var award = (long)Math.Truncate(ratingCost * x.Rating) - 1;
                for (int i = 0; i < award; i++)
                    AddCoin(x);
                balance -= award;
                var weight = ratingCost * x.Rating - award;
                return weight;
            });

            foreach (var user in awards)
                if (balance-- > 0)
                    AddCoin(user);
                else
                    break;

            return new Response() { Status = Response.Types.Status.Ok, Comment = "Оперция прошла успешно." };
        }

        public override async Task<Coin> LongestHistoryCoin(None request, ServerCallContext context)
        {
            return await base.LongestHistoryCoin(request, context);
        }

        public override async Task<Response> MoveCoins(MoveCoinsTransaction request,
                                                       ServerCallContext context)
        {
            return new Response() { Status = Response.Types.Status.Ok, Comment = "" };
        }

        private void AddCoin(User user)
        {
            user.Amount += 1;
            coins.Add(new Coin() { Id = coins.Count + 1, History = user.Name });
        }
    }
    public class User
    {
        public User(string name, long rating)
        {
            Name = name;
            Rating = rating;
        }

        public User(string name, long rating, long amount)
        {
            Name = name;
            Rating = rating;
            Amount = amount;
        }

        public string Name { get; set; }
        public long Rating { get; set; }
        public long Amount { get; set; }

        public UserProfile ToUserProfile() => new UserProfile() { Name = Name, Amount = Amount };

    }
}

