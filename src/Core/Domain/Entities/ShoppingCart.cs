﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public record ShoppingCart : AuditableBaseEntity
    {
        public ShoppingCart()
        {
            ShoppingCartItems = new HashSet<ShoppingCartItem>();
        }

        public DateTime OrderAt { get; set; }
        public int Total { get; set; }
        public int DeliveryFees { get; set; }
        [Required]
        public string CustomerId { get; set; }

        //[ForeignKey("CustomerId")]
        //public virtual AppUser Customer { get; set; }

        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
