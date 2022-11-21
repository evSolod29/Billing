using Billing.DAL.Models;
using System.Diagnostics.CodeAnalysis;

namespace Billing.Tests.BLL.Comparers
{
    internal class CoinComparer : IEqualityComparer<Coin>
    {
        private readonly IEqualityComparer<User> userComparer;

        public CoinComparer()
        {
            userComparer = new UserComparer();
        }
        public CoinComparer(IEqualityComparer<User> userComparer)
        {
            this.userComparer = userComparer;
        }

        public bool Equals(Coin? x, Coin? y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.UserId == y.UserId && new UserComparer().Equals(x.User, y.User);
        }

        public int GetHashCode([DisallowNull] Coin obj)
        {
            return obj.Id.GetHashCode() ^ userComparer.GetHashCode(obj.User);
        }
    }
}
