using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class NameExistException : Exception
    {
        public NameExistException(string message) : base(message)
        {

        }
    }
}
