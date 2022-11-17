using System.Diagnostics.CodeAnalysis;
using Billing.DAL.Models;

namespace Billing.Tests.DAL.Comparers
{
    public class UserEqualityComparer : IEqualityComparer<User?>
    {
        public bool Equals(User? x, User? y)
        {
            if(x == null || y == null)
                return false;
            
            bool isEqual = x.Id == y.Id;
            isEqual &= x.Name == y.Name;
            isEqual &= x.Rating == y.Rating;

            return isEqual;
        }

        public int GetHashCode([DisallowNull] User obj)
        {
            return obj.Id.GetHashCode() & obj.Name.GetHashCode() & obj.Rating.GetHashCode();
        }
    }
}

