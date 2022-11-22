using Billing.BLL.DataManagement.Interfaces;
using Billing.BLL.DTO;
using Billing.BLL.Extensions;
using Billing.DAL.Models;
using Billing.DAL.Repositories.Interfaces;

namespace Billing.BLL.DataManagement
{
    /// <summary>
    /// Класс управления пользователями
    /// </summary>
    public class UsersManagement : IUsersManagement
    {
        private readonly IUsersRepository usersRepository;
        private readonly IUnitOfWork repos;

        public UsersManagement(IUnitOfWork repos)
        {
            usersRepository = repos.UsersRepository;
            this.repos = repos;
        }

        /// <summary>
        /// Получение списка пользователей.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UserDTO>> Get()
        {
            ICollection<UserDTO> userDTOs = new List<UserDTO>();
            IEnumerable<User> users = await usersRepository.GetAllWithInclude(x => x.Coins);

            foreach (User user in users)
                userDTOs.Add(user.ToDTO());

            return userDTOs;
        }

        public async Task InitializeTestValues()
        {
            await usersRepository.Add(new User() { Name = "boris", Rating = 5000 });
            await usersRepository.Add(new User() { Name = "maria", Rating = 1000 });
            await usersRepository.Add(new User() { Name = "oleg", Rating = 800 });
            await repos.SaveChanges();
        }
    }
}
