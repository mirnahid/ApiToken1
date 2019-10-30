using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Identity;
using WebApiToken.Domain.Response;
using WebApiToken.Domain.Services;
using WebApiToken.Models;
using WebApiToken.ResourceViewModel;

namespace WebApiToken.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager)
            : base(_userManager, _signInManager, _roleManager)
        {
        }

        public async Task<Tuple<AppUser, IList<Claim>>> GetUserByrefreshToken(string refreshToken)
        {
            Claim claimRefreshToken = new Claim("refreshToken", refreshToken);
            var users = await userManager.GetUsersForClaimAsync(claimRefreshToken);
            if (users.Any())
            {
                var user = users.First();
                IList<Claim> userClaim = await userManager.GetClaimsAsync(user);
                string refleshTokenEndDate = userClaim.First(x => x.Type == "refreshTokenEndDate").Value;
                if (DateTime.Parse(refleshTokenEndDate)>DateTime.Now)
                {
                    return new Tuple<AppUser, IList<Claim>>(user, userClaim);
                }
                else
                {
                    return new Tuple<AppUser, IList<Claim>>(null, null);
                }
            }

            return new Tuple<AppUser, IList<Claim>>(null, null);
        }

        public async Task<AppUser> GetUserByUserName(string userName)
        {
            return await userManager.FindByNameAsync(userName);
        }

        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            var result = await GetUserByrefreshToken(refreshToken);
            if (result.Item1==null)
            {
                return false;
            }
            IdentityResult response = await userManager.RemoveClaimsAsync(result.Item1,result.Item2);
            if (response.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<BaseResponse<UserViewModelResource>> UpdateUser(UserViewModelResource userViewModel, string userName)
        {
            AppUser user = await userManager.FindByNameAsync(userName);
            if ((userManager.Users.Count(x=>x.PhoneNumber==userViewModel.PhoneNumber))>1)
            {
                return new BaseResponse<UserViewModelResource>("Bu telefon nomresi basqa birine aiddir");
            }
            user.BirthDay = userViewModel.BirthDay;
            user.City = userViewModel.City;
            user.Gender = (int)userViewModel.Gender;
            user.Email = userViewModel.Email;
            user.UserName = userViewModel.UserName;
            user.PhoneNumber = userViewModel.PhoneNumber;
            IdentityResult result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new BaseResponse<UserViewModelResource>(user.Adapt<UserViewModelResource>());
            }
            else
            {
                return new BaseResponse<UserViewModelResource>(result.Errors.First().Description);    
            }
        }

        public async Task<BaseResponse<AppUser>> UploadUserPicture(string picturePath, string userName)
        {
            AppUser user = await userManager.FindByNameAsync(userName);
            user.Picture = picturePath;
            IdentityResult result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new BaseResponse<AppUser>(user);
            }
            return new BaseResponse<AppUser>(result.Errors.First().Description);
        }
    }
}
