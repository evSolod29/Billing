using Billing.BLL.DTO;
using System.Diagnostics.CodeAnalysis;

namespace Billing.UnitTests.BLL.Comparers
{
    public class CoinDtoComparer : IEqualityComparer<CoinDTO>
    {
        public bool Equals(CoinDTO? x, CoinDTO? y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.History == y.History;
        }

        public int GetHashCode([DisallowNull] CoinDTO obj)
        {
            return obj.Id.GetHashCode() ^ obj.History.GetHashCode();
        }
    }
}
