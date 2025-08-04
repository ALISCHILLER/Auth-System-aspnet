using System;

namespace AuthSystem.Domain.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException(string message) : base(message)
        {
        }

        public static InvalidPasswordException ForInvalidPassword()
        {
            return new InvalidPasswordException("رمز عبور باید حداقل ۸ کاراکتر و شامل حروف بزرگ، کوچک، عدد و نماد باشد.");
        }
    }
}
