using Billing.BLL.DataManagement;
using Billing.BLL.DTO;
using Billing.DAL.Models;
using Billing.UnitTests.BLL.Comparers;
using Billing.UnitTests.BLL.Fixtures;

namespace Billing.UnitTests.BLL.DataManagement
{
    public class UsersManagementTests: IDisposable
    {
        private readonly DatabaseFixture database;

        public UsersManagementTests()
        {
            database = new DatabaseFixture(nameof(UsersManagementTests));
        }

        public void Dispose()
        {
            database.Dispose();
        }

        [Fact]
        public async void Get_ReturnCollectionOfUserDto()
        {
            database.DbContext.AttachRange(new List<object>()
            {
                new User() { Name = "boris", Rating = 1 },
                new User() { Name = "maria", Rating = 1 },
                new Coin() { UserId  = 1 }
            });
            database.DbContext.SaveChanges();

            IEqualityComparer<UserDTO> comparer = new UserDtoComparer();
            IEnumerable<UserDTO> expected = new List<UserDTO>()
            {
                new UserDTO(1, "boris", 1),
                new UserDTO(2, "maria", 0)
            };

            UsersManagement management = new UsersManagement(database.UnitOfWork);
            IEnumerable<UserDTO> actual = await management.Get();

            Assert.Equal(expected, actual, comparer);
        }
    }
}
