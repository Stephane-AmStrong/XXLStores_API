using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record ShoppingCart : AuditableBaseEntity
    {
        public ShoppingCart()
        {
            ShoppingCartItems = new HashSet<ShoppingCartItem>();
        }

        public DateTime Date { get; set; }
        public int Total { get; set; }
        public int DeliveryFees { get; set; }
        [Required]
        public Guid CustomerId { get; set; }

        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
