using Billing.BLL.DTO;

namespace Billing.BLL.DataManagement.Interfaces
{
    public interface IUsersManagement
    {
        Task<IEnumerable<UserDTO>> Get();
    }
}