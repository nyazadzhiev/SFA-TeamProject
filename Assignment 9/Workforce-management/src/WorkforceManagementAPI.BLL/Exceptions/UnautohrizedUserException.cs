using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class UnautohrizedUserException : Exception
    {
        public UnautohrizedUserException(string message) : base(message)
        {

        }
    }
}
