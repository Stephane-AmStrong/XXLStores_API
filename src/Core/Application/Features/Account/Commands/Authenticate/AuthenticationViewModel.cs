using Application.Features.Account.Commands.RefreshAccessToken;
using Application.Models;

namespace Application.Features.Account.Commands.Authenticate
{
    public class AuthenticationViewModel
    {
        public Dictionary<string, string> UserInfo { get; set; }
        public string AccessToken { get; set; }
        public UserTokenViewModel RefreshToken { get; set; }
        public DateTime? ExpireDate { get; set; }
        //public bool IsLockedOut { get; set; }
        //public bool IsSuccess { get; set; }
        //public string Message { get; set; }
        //public IEnumerable<string> ErrorDetails { get; set; }

    }
}
