namespace Billing.BLL.Exceptions
{
    public class NotFoundException:BusinessLogicException
    {
        public NotFoundException(string message):base(message)
        {
        }
    }
}
