using Billing.BLL.DTO;
using Billing.DAL.Models;

namespace Billing.BLL.Extensions
{
    public static class UserExtensions
    {
        public static UserDTO ToDTO(this User user, long amount)
        {
            return new UserDTO(user.Id, user.Name, amount);
        }
    }
}
