using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class TimeOffOverlapExzception : Exception
    {
        public TimeOffOverlapExzception(string message) : base(message)
        {

        }
    }
}
