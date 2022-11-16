using Billing.DAL.Repositories.Interfaces;

namespace Billing.BLL.DataManagement
{
    public class CoinsManagement
    {
        private readonly ICoinsRepository coinsRepo;
        public CoinsManagement(ICoinsRepository coinsRepo)
        {
            this.coinsRepo = coinsRepo;
            
        }
    }
}
