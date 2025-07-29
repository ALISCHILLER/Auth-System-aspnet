using System;

namespace AuthSystem.Domain.Exceptions
{
    public class InvalidRoleNameException : DomainException
    {
        public InvalidRoleNameException()
            : base("نام نقش نمی‌تواند خالی باشد") { }

        public InvalidRoleNameException(string message)
            : base(message) { }
    }
}
