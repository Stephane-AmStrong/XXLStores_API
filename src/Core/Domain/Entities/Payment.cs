using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record Payment : AuditableBaseEntity
    {
        public int MoneyAmount { get; set; }
        public DateTime? PaidAt { get; set; }
        public Guid ShoppingCartId { get; set; }

        [ForeignKey("ShoppingCartId")]
        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}
