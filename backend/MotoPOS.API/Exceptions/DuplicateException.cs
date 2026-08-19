namespace MotoPOS.API.Exceptions;

public class DuplicateException : InvalidOperationException
{
    public DuplicateException(string message) : base(message)
    {
    }
}