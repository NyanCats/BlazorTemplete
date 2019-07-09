using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTemplate.Server.Entities
{
    public class Avatar
    {
        [Column(Order = 0)]
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Column(Order = 1)]
        [Required]
        [ForeignKey("User")]
        public User Owner{ get; set; }

        [Column(Order = 2)]
        public byte[] Image { get; set; }

        [Column(Order = 3)]
        public DateTime? LastUpdated { get; set; }

    }
}
