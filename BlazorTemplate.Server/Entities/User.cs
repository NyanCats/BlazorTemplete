using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTemplate.Server.Entities
{
    public class User : IdentityUser<Guid>
    {/*
        [Required]
        public DateTime RegistrationDateTime { get; set; }
        [Required]
        public string RegistrationIpAddress { get; set; }
        [Required]
        public DateTime LastUpdate { get; set; }
        */
    }
}
