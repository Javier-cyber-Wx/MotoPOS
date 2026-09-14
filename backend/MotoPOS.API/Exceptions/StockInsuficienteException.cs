namespace MotoPOS.API.Exceptions
{
    public class StockInsuficienteException : Exception
    {
        public StockInsuficienteException(string message) : base(message)
        {
        }
    }
}
