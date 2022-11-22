using Billing.BLL.DTO;
using System.Diagnostics.CodeAnalysis;

namespace Billing.UnitTests.BLL.Comparers
{
    public class UserDtoComparer : IEqualityComparer<UserDTO>
    {
        public bool Equals(UserDTO? x, UserDTO? y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Name == y.Name && x.Amount == y.Amount;
        }

        public int GetHashCode([DisallowNull] UserDTO obj)
        {
            return obj.Id.GetHashCode() ^ obj.Name.GetHashCode() ^ obj.Amount.GetHashCode();
        }
    }
}
