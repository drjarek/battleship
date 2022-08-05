using GameEngine.Domain.Exception;

namespace GameEngine.Domain.Validator
{
    public abstract class AbstractValidator<TModel>
    {
        protected abstract bool IsValid(TModel model);

        private static void ThrowException(string message)
        {
            throw new ValidationException(message);
        }

        public void ThrowExceptionIfInvalid(TModel model)
        {
            if (!IsValid(model))
            {
                ThrowException("Invalid model.");
            }
        }
    }
}

