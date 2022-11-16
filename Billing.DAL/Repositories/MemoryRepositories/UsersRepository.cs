using Billing.DAL.Contexts;
using Billing.DAL.Models;

namespace Billing.DAL.Repositories.MemoryRepositories
{
    public class UsersRepository
    {
        private readonly MemoryContext context;
        public UsersRepository(MemoryContext context)
        {
            this.context = context;
        }

        public async Task Update(User user)
        {
            var oldUser = context.Users.FirstOrDefault(x => x.Id == user.Id);
            if (oldUser == null)
                throw new KeyNotFoundException($"Key is {user.Id}");

            var newUser = new User(user.Name, user.Rating) { Id = oldUser.Id, Coins = oldUser.Coins };

            context.Users.Remove(oldUser);
            context.Users.Add(newUser);
        }

        public async Task<User?> Get(long id)
        { 
            var user = context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return default;

            return new User(user.Name, user.Rating);
        }

        public async Task<IEnumerable<User>> Get() => 
            context.Users.Select(x => new User(x.Name, x.Rating) { Id = x.Id});
    }
}
