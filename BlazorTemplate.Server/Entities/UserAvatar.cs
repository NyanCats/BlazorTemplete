using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTemplate.Server.Entities
{
    public class UserAvatar
    {
        [Key, Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid AvatarId { get; set; }
    }
}
