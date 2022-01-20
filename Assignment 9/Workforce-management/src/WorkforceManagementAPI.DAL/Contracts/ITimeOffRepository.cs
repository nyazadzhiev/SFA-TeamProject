﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.DAL.Contracts
{
    public interface ITimeOffRepository
    {
        Task CreateTimeOffAsync(TimeOff timeOff);
        void DeleteTimeOffAsync(TimeOff timeOff);
        Task<List<TimeOff>> GetAllAsync();
        Task<List<TimeOff>> GetMyTimeOffsAsync(string userId);
        Task<TimeOff> GetTimeOffAsync(Guid id);
        Task SaveChangesAsync();
    }
}