using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        //public AppUser()
        //{
        //    Shops = new HashSet<Shop>();
        //    ShoppingCarts = new HashSet<ShoppingCart>();
        //}

        public string ImgLink { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }
        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }

        //public virtual ICollection<Shop> Shops { get; set; }
        //public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
