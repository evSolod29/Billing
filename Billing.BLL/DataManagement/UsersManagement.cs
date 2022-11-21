﻿using Billing.BLL.DataManagement.Interfaces;
using Billing.BLL.DTO;
using Billing.BLL.Extensions;
using Billing.DAL.Models;
using Billing.DAL.Repositories.Interfaces;

namespace Billing.BLL.DataManagement
{
    public class UsersManagement : IUsersManagement
    {
        private readonly IUsersRepository usersRepository;
        private readonly ICoinsRepository coinsRepository;
        private readonly IUnitOfWork repos;

        public UsersManagement(IUnitOfWork repos)
        {
            usersRepository = repos.UsersRepository;
            coinsRepository = repos.CoinsRepository;
            this.repos = repos;
        }

        public async Task<IEnumerable<UserDTO>> Get()
        {
            ICollection<UserDTO> userDTOs = new List<UserDTO>();
            IEnumerable<User> users = await usersRepository.GetAll();

            foreach (User user in users)
            {
                userDTOs.Add(user.ToDTO(await coinsRepository.GetUserCoinAmount(user.Id)));
            }

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
