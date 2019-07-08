using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTemplate.Server.Entities
{
    public class Avatar<TKey>
    {
        [Required]
        [Key]
        public TKey OwnerId { get; set; }
        public byte[] Image { get; set; }
        public DateTime? LastUpdated { get; set; }

    }
}
