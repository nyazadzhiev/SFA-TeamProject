using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class InputOutOfBoundsException : Exception
    {
        public InputOutOfBoundsException(string message) : base(message)
        {

        }
    }
}
