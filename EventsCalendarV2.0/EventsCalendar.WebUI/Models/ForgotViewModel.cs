﻿using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.WebUI.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}