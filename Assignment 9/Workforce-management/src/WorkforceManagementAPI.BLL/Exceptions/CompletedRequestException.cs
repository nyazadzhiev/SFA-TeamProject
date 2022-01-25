using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class CompletedRequestException : Exception
    {
        public CompletedRequestException(string message) : base(message)
        {

        }
    }
}
