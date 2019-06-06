using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTemplate.Server.Entities.Identities
{
    public class ApplicationRole
    {
        public ApplicationRole(string name)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            NormalizedRoleName = Name.Normalize();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedRoleName { get; set; }
}
}
