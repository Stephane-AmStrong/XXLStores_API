using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataTransfertObjects.Account
{
    public record AppUserResponse
    {
        public AppUserResponse()
        {
            Shops = new HashSet<Shop>();
        }

        public string ImgLink { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }

        public virtual ICollection<Shop> Shops { get; set; }


        public List<RefreshToken> RefreshTokens { get; set; }
        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
