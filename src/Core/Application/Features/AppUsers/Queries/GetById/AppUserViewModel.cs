using Application.Features.ShoppingCarts.Queries.GetPagedList;
using Application.Features.Shops.Queries.GetPagedList;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.Features.AppUsers.Queries.GetById
{
    public record AppUserViewModel
    {
        public string Id { get; set; }
        public string ImgLink { get; set; }
        public string Firstname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }

        public virtual ICollection<ShopsViewModel> Shops { get; set; }
        public virtual ICollection<ShoppingCartsViewModel> ShoppingCarts { get; set; }
    }
}
