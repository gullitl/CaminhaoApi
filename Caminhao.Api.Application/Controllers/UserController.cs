using Caminhao.Api.Application.Services;
using Caminhao.Api.Domain.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Caminhao.Api.Application.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) {
            _userService = userService;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<List<User>>> GetAll() => Ok(await _userService.GetAll());

        [HttpGet("getbyid")]
        public async Task<ActionResult<User>> GetById(string id) {
            User user = await _userService.GetById(id);
            if(user == null)
                return NotFound("Mawa trop");

            return user;
        }

        [HttpPost("create")]
        public async Task<ActionResult<User>> Create(User user) {
            if(await _userService.GetIfUsernameOrEmailExists(user.Username?? user.Email) == null)
                return await _userService.Create(user);
            else
                return Conflict("Mawa trop");
        }

        [HttpPut("update")]
        public async Task<ActionResult<User>> Update(User user) => await _userService.Update(user);

        [HttpPut("changeprofil")]
        public async Task<ActionResult<User>> ChangeProfile(User user) => await _userService.ChangeProfile(user);

        [HttpPut("changepassword")]
        public async Task<ActionResult<User>> ChangePassword(NewPassword newPassword) {
            if(newPassword.Token != null) 
                if(!_userService.IsChangePasswordTokenValid(newPassword.Token, newPassword.Username)) 
                    return null; 
             
            return await _userService.ChangePassword(newPassword.Username, newPassword.Password);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<bool>> Delete(string id) => await _userService.Delete(id);

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(User user) => await _userService.Login(user.Username, user.Email, user.Password); 
    }
}
