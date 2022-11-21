using Billing.BLL.DTO;

namespace Billing.Extensions
{
    public static class UserDtoExtensions
    {
        public static UserProfile ToUserProfile(this UserDTO user)
        {
            return new UserProfile() { Name = user.Name, Amount = user.Amount };
        }
    }
}

