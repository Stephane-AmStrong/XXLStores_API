﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record Shop : AuditableBaseEntity
    {
        public Shop()
        {
            Items = new HashSet<Item>();
        }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]    
        public double Latitude { get; set; }
        [Required]
        public Guid OwnerId { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
