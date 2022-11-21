using Billing.API.Extensions;
using Billing.BLL.DataManagement.Interfaces;
using Billing.BLL.DTO;
using Billing.BLL.Exceptions;
using Billing.Extensions;
using Grpc.Core;

namespace Billing.API.Services
{
    public class BillingService : Billing.BillingBase
    {
        private readonly IUsersManagement usersManagement;
        private readonly ICoinsManagement coinsManagement;

        public BillingService(IUsersManagement usersManagement, ICoinsManagement coinsManagement)
        {
            this.usersManagement = usersManagement;
            this.coinsManagement = coinsManagement;
        }

        public override async Task ListUsers(None request,
                                             IServerStreamWriter<UserProfile> responseStream,
                                             ServerCallContext context)
        {
            foreach (UserDTO user in await usersManagement.Get())
            {
                await responseStream.WriteAsync(user.ToUserProfile());
            }
        }

        public override async Task<Response> CoinsEmission(EmissionAmount request, ServerCallContext context)
        {

            try
            {
                await coinsManagement.CoinsEmission(request.Amount);
            }
            catch (BusinessLogicException ex)
            {
                return new Response()
                {
                    Status = Response.Types.Status.Failed,
                    Comment = ex.Message
                };
            }
            catch(Exception)
            {
                return new Response()
                {
                    Status = Response.Types.Status.Unspecified,
                    Comment = "Internal Server Error."
                };
            }
            
            return new Response() 
            { 
                Status = Response.Types.Status.Ok, 
                Comment = "The operation was successful."
            };
        }

        public override async Task<Coin> LongestHistoryCoin(None request, ServerCallContext context)
        {
            return (await coinsManagement.LongestHistoryCoin()).ToCoin();
        }

        public override async Task<Response> MoveCoins(MoveCoinsTransaction request,
                                                       ServerCallContext context)
        {
            try
            {
                await coinsManagement.MoveCoinByUserName(request.SrcUser, request.DstUser, request.Amount);
            }
            catch (BusinessLogicException ex)
            {
                return new Response()
                {
                    Status = Response.Types.Status.Failed,
                    Comment = ex.Message
                };
            }
            catch (Exception)
            {
                return new Response()
                {
                    Status = Response.Types.Status.Unspecified,
                    Comment = "Internal Server Error."
                };
            }
            
            return new Response() 
            { 
                Status = Response.Types.Status.Ok, 
                Comment = "The operation was successful."
            };
        }
    }
}

