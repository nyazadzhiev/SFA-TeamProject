﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagementAPI.BLL.Contracts
{
    public interface INotificationService
    {
        Task Send(string to, string subject, string message);
    }
}
