using Billing.DAL.Models;
using System.Diagnostics.CodeAnalysis;

namespace Billing.UnitTests.BLL.Comparers
{
    internal class UserComparer : IEqualityComparer<User>
    {
        public bool Equals(User? x, User? y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Name == y.Name && x.Rating == y.Rating;
        }

        public int GetHashCode([DisallowNull] User obj)
        {
            return obj.Id.GetHashCode() ^ obj.Name.GetHashCode() ^ obj.Rating.GetHashCode();
        }
    }
}
