using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public record UserToken
    {
        public string UserId { get; set; }
        public DateTime ExpiryTime { get; set; }
        public string Value { get; set; }

        [NotMapped]
        public bool IsActive => (DateTime.UtcNow <= ExpiryTime);
        //public virtual AppUser AppUser { get; set; }
    }
}
