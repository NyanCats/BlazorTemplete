using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTemplate.Server.Entities.Identities
{
    public class ApplicationUser
    {
        public ApplicationUser(string name)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            NormalizedUserName = Name.Normalize();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
