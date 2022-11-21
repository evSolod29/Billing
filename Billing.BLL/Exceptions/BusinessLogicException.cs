namespace Billing.BLL.Exceptions
{
    public abstract class BusinessLogicException:Exception
    {
        public BusinessLogicException(string message):base(message)
        {
        }
    }
}
