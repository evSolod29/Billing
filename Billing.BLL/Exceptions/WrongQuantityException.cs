namespace Billing.BLL.Exceptions
{
    public class WrongQuantityException : BusinessLogicException
    {
        public WrongQuantityException(string message) : base(message)
        {
        }
    }
}
