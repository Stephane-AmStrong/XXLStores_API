using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public record RefreshTokenResponse
    {
        public string AccessToken { get; set; }
        public UserToken RefreshToken { get; set; }
    }
}
