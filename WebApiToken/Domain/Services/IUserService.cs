﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiToken.Domain.Response;
using WebApiToken.Models;
using WebApiToken.ResourceViewModel;

namespace WebApiToken.Domain.Services
{
   public interface IUserService
    {
        Task<BaseResponse<UserViewModelResource>> UpdateUser(UserViewModelResource userViewModel, string userName);
        Task<AppUser> GetUserByUserName(string userName);
        Task<BaseResponse<AppUser>> UploadUserPicture(string picturePath,string userName);
        Task<Tuple<AppUser, IList<Claim>>> GetUserByrefreshToken(string refreshToken);
        Task<bool> RevokeRefreshToken(string refreshToken);
    }
}
