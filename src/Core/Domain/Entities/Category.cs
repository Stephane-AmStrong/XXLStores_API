using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record Category : AuditableBaseEntity
    {
        public Category()
        {
            Items = new HashSet<Item>();
        }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
