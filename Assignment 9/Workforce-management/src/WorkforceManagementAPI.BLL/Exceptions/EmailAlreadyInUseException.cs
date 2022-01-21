﻿using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class EmailAlreadyInUseException : Exception
    {
        public EmailAlreadyInUseException(string message) : base(message)
        {

        }
    }
}
