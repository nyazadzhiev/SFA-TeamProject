﻿using System;
using System.ComponentModel.DataAnnotations;
using WorkforceManagementAPI.DAL.Entities.Enums;

namespace WorkforceManagementAPI.DTO.Models.Requests
{
    public class EditTimeOffRequestDTO
    {
        [Required]
        public string Reason { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
