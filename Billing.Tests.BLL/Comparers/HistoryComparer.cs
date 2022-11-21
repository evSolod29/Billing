using Billing.DAL.Models;
using System.Diagnostics.CodeAnalysis;

namespace Billing.Tests.BLL.Comparers
{
    internal class HistoryComparer : IEqualityComparer<History>
    {
        private readonly IEqualityComparer<User> userComparer;
        private readonly IEqualityComparer<Coin> coinComparer;

        public HistoryComparer()
        {
            userComparer = new UserComparer();
            coinComparer = new CoinComparer(userComparer);
        }

        public HistoryComparer(IEqualityComparer<User> userComparer, IEqualityComparer<Coin> coinComparer)
        {
            this.userComparer = userComparer;
            this.coinComparer = coinComparer;
        }

        public bool Equals(History? x, History? y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.CoinId == y.CoinId && x.FromUserId == y.FromUserId 
                && x.ToUserId == y.ToUserId && coinComparer.Equals(x.Coin, y.Coin)
                && userComparer.Equals(x.FromUser, y.FromUser) && userComparer.Equals(x.ToUser, y.ToUser);
        }

        public int GetHashCode([DisallowNull] History obj)
        {
            return obj.Id.GetHashCode() ^ coinComparer.GetHashCode(obj.Coin) 
                ^ (obj.FromUser == null ? 0 : userComparer.GetHashCode(obj.FromUser)) 
                ^ userComparer.GetHashCode(obj.ToUser);
        }
    }
}
