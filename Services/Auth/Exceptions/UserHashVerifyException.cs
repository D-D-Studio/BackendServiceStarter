using System;

namespace BackendServiceStarter.Services.Auth.Exceptions
{
    public class UserHashVerifyException : Exception
    {
        public UserHashVerifyException()
        {
        }

        public UserHashVerifyException(string message) : base(message)
        {
        }
    }
}