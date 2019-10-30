using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiToken.Domain.Response;
using WebApiToken.Domain.Services;
using WebApiToken.Models;
using WebApiToken.ResourceViewModel;
using WebApiToken.Security.Token;

namespace WebApiToken.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly ITokenHandler _tokenHandle;
        private readonly CustomTokenOptions _tokenOptions;
        private readonly IUserService _userService;

        public AuthenticationService(IUserService userService, ITokenHandler tokenHandle,IOptions<CustomTokenOptions> tokenOptions ,UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager)
            : base(_userManager, _signInManager, _roleManager)
        {
            _tokenHandle = tokenHandle;
            _tokenOptions = tokenOptions.Value;
            _userService = userService;
        }

        public async Task<BaseResponse<AccessToken>> CreateAccessTokenByRefreshToken(RefleshTokenViewModelResource refleshTokenViewModel)
        {
            var userClaim = await _userService.GetUserByrefreshToken(refleshTokenViewModel.RefreshToken);
            if (userClaim.Item1!=null)
            {
                AccessToken acessToken = _tokenHandle.CreateAccessToken(userClaim.Item1);
                Claim refreshTokenClaim = new Claim("refreshToken", acessToken.RefreshToken);
                Claim refreshTokenEndDateClaim = new Claim("refreshTokenEndDate", DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpiration).ToString());
                await userManager.ReplaceClaimAsync(userClaim.Item1, userClaim.Item2[0],refreshTokenClaim);
                await userManager.ReplaceClaimAsync(userClaim.Item1, userClaim.Item2[1], refreshTokenEndDateClaim);
                return new BaseResponse<AccessToken>(acessToken);
            }
            else
            {
                return new BaseResponse<AccessToken>("Bele bir istifadeci yoxdur");
            }
        }

        public async Task<BaseResponse<AccessToken>> RevokeRefreshToken(RefleshTokenViewModelResource refleshTokenViewModel)
        {
            bool result = await _userService.RevokeRefreshToken(refleshTokenViewModel.RefreshToken);
            if (result)
            {
                return new BaseResponse<AccessToken>(new AccessToken());
            }
            else
            {
                return new BaseResponse<AccessToken>("Refresh Token tapilmadi");
            }
        }

        public async Task<BaseResponse<AccessToken>> SignIn(SigninViewModelResource signinViewModel)
        {
            AppUser user = await userManager.FindByEmailAsync(signinViewModel.Email);
            if (user != null)
            {
                bool isUser = await userManager.CheckPasswordAsync(user, signinViewModel.Password);
                if (isUser)
                {
                    AccessToken acessToken = _tokenHandle.CreateAccessToken(user);
                    Claim refreshTokenClaim = new Claim("refreshToken",acessToken.RefreshToken);
                    Claim refreshTokenEndDateClaim = new Claim("refreshTokenEndDate", DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpiration).ToString());
                    List<Claim> refreshClaimList = userManager.GetClaimsAsync(user).Result.Where(x => x.Type.Contains("refreshToken")).ToList();
                    if (refreshClaimList.Any())
                    {
                        await userManager.ReplaceClaimAsync(user, refreshClaimList[0], refreshTokenClaim);
                        await userManager.ReplaceClaimAsync(user, refreshClaimList[1], refreshTokenEndDateClaim);
                    }
                    else
                    {
                        await userManager.AddClaimsAsync(user, new[] { refreshTokenClaim, refreshTokenEndDateClaim });
                    }
                    return new BaseResponse<AccessToken>(acessToken);
                }
                return new BaseResponse<AccessToken>("Email ve ya sifre sehvdir");
            }
            return new BaseResponse<AccessToken>("Email ve ya sifre sehvdir");
        }

        public async Task<BaseResponse<UserViewModelResource>> SignUp(UserViewModelResource userViewModel)
        {
            AppUser user = new AppUser {UserName=userViewModel.UserName,Email=userViewModel.Email };
            IdentityResult result = await userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                return new BaseResponse<UserViewModelResource>(user.Adapt<UserViewModelResource>());
            }
            else
            {
                return new BaseResponse<UserViewModelResource>(result.Errors.First().Description);
            }
        }
    }
}
