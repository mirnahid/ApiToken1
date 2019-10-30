using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiToken.Domain.Response;
using WebApiToken.ResourceViewModel;
using WebApiToken.Security.Token;

namespace WebApiToken.Domain.Services
{
   public interface IAuthenticationService
    {
        Task<BaseResponse<UserViewModelResource>> SignUp(UserViewModelResource userViewModel);
        Task<BaseResponse<AccessToken>> SignIn(SigninViewModelResource signinViewModel);
        Task<BaseResponse<AccessToken>> CreateAccessTokenByRefreshToken(RefleshTokenViewModelResource refleshTokenViewModel);
        Task<BaseResponse<AccessToken>> RevokeRefreshToken(RefleshTokenViewModelResource refleshTokenViewModel);
    }
}
