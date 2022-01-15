using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagementAPI.Common
{
    public static class Constants
    {
        public const string NotFound = "{0} not found";

        public const string NameAlreadyInUse = "{0} is already in use.";

        public const string InvalidLength = "The field {0} must have a minimum length of {1}";

        public const string InvalidEmail = "The email must be valid";

        public const string EmailAreadyInUse = "The email already exists";

        public const string Created = "{0} was created";

        public const string Deleted = "{0} was deleted";

        public const string OperationFailed = "Error!! Operation Failed";

        public const string InvalidInput = "Invalid input";

        public const string InputOutOfBounds = "{0} input is exceeds predifined boundaries";
    }
}
