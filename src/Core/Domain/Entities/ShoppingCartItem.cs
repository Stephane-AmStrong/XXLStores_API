using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record ShoppingCartItem : AuditableBaseEntity
    {
        public Guid ShoppingCartId { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int Total { get; set; }
        public Guid ItemId { get; set; }

        [ForeignKey("ShoppingCartId")]
        public virtual ShoppingCart ShoppingCart { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
    }
}
