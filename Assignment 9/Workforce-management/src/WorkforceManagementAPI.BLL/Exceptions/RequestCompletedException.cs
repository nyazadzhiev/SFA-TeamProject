using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class RequestCompletedException : Exception
    {
        public RequestCompletedException(string message) : base(message)
        {

        }
    }
}
