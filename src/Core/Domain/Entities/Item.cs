using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public record Item : AuditableBaseEntity
    {
        public Item()
        {
            InventoryLevels = new HashSet<InventoryLevel>();
            ShoppingCartItems = new HashSet<ShoppingCartItem>();
        }

        [Required]
        public string Name { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public Guid ShopId { get; set; }

        public virtual ICollection<InventoryLevel> InventoryLevels { get; set; }
        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("ShopId")]
        public virtual Shop Shop { get; set; }
    }
}
