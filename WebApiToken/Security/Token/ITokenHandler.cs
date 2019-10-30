using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiToken.Models;

namespace WebApiToken.Security.Token
{
   public interface ITokenHandler
    {
        AccessToken CreateAccessToken(AppUser user);
        void RevokerefreshTOken(AppUser user);
    }
}
