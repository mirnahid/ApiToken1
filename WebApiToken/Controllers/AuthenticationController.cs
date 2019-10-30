using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiToken.Domain.Response;
using WebApiToken.Domain.Services;
using WebApiToken.ResourceViewModel;

namespace WebApiToken.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpGet]
        public IActionResult IsAuthentication()
        {
            return Ok(User.Identity.IsAuthenticated);
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModelResource userViewModelResource)
        {
          BaseResponse<UserViewModelResource> response=  await _authenticationService.SignUp(userViewModelResource);
            if (response.Success)
            {
                return Ok(response.Extra);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SigninViewModelResource signinViewModelResource)
        {
          var response =  await _authenticationService.SignIn(signinViewModelResource);
            if (response.Success)
            {
                return Ok(response.Extra);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccessTokenByRefreshToken(RefleshTokenViewModelResource refleshTokenViewModelResource)
        {
            var response = await _authenticationService.CreateAccessTokenByRefreshToken(refleshTokenViewModelResource);
            if (response.Success)
            {
                return Ok(response.Extra);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> RevokeRefreshToken(RefleshTokenViewModelResource refleshTokenViewModelResource)
        {
            var response = await _authenticationService.RevokeRefreshToken(refleshTokenViewModelResource);
            if (response.Success)
            {
                return Ok();
            }
            else
            {
                return BadRequest(response.Message);
            }
        }
    }
}