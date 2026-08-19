using MotoPOS.API.Exceptions;

namespace MotoPOS.API.Services;

public abstract class BaseService
{
    protected void ValidateEntityExists<T>(T? entity, string errorMessage)
    {
        if (entity == null)
        {
            throw new NotFoundException(errorMessage);
        }
    }

    protected void ValidateDuplicate(bool exists, string errorMessage)
    {
        if (exists)
        {
            throw new DuplicateException(errorMessage);
        }
    }

    protected void ValidateExists(bool exists, string errorMessage)
    {
        if (!exists)
        {
            throw new NotFoundException(errorMessage);
        }
    }
}