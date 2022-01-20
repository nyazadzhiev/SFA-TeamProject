using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class TimeOffAlreadyExistsException : Exception
    {
        public TimeOffAlreadyExistsException(string message) : base(message)
        {

        }
    }
}
