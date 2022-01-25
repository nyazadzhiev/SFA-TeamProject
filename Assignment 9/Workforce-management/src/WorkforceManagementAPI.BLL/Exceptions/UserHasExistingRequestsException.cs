using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    internal class UserHasExistingRequestsException : Exception
    {
        public UserHasExistingRequestsException(string message) : base (message)
        {

        }
    }
}
