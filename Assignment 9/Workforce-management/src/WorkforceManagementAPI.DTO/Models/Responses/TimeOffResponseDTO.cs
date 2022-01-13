﻿using System;
using WorkforceManagementAPI.DAL.Entities.Enums;

namespace WorkforceManagementAPI.DTO.Models.Responses
{
    public class TimeOffResponseDTO
    {
        public Guid Id { get; set; }

        public string Reason { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string CreatorName { get; set; }

        public string ModifierName { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }
    }
}
