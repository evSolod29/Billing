using System.Diagnostics.CodeAnalysis;
using Billing.DAL.Models;

namespace Billing.Tests.DAL.Comparers
{
    public class CoinEqualityComparer : IEqualityComparer<Coin?>
    {
        public bool Equals(Coin? x, Coin? y)
        {
            if(x == null || y == null)
                return false;
            
            if(x.Id != y.Id)
                return false;
            
            if(! new UserEqualityComparer().Equals(x.User,y.User))
                return false;

            return true;
        }

        public int GetHashCode([DisallowNull] Coin obj)
        {
            return obj.Id.GetHashCode() & new UserEqualityComparer().GetHashCode(obj.User);
        }
    }
}
