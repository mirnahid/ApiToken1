using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiToken.Models;

namespace WebApiToken.Services
{
    public class BaseService:ControllerBase
    {
        protected UserManager<AppUser> userManager { get; }
        protected SignInManager<AppUser> signInManager { get; }
        protected RoleManager<AppRole> roleManager { get; }
        public BaseService(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }
    }
}
