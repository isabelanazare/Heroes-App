using Fabrit.Heroes.Business.Services.Contracts;
using Fabrit.Heroes.Data;
using Fabrit.Heroes.Data.Entities.Hero;
using Fabrit.Heroes.Web.Infrastructure.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fabrit.Heroes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAll());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            var user = await _userService.GetUser(id);

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync(User user)
        {
            var addedUser = await _userService.AddUser(user);

            return Ok(addedUser);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(User user)
        {
            var updatedUser = await _userService.UpdateUser(user);

            return Ok(updatedUser);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            await _userService.DeleteUser(id);

            return NoContent();
        }

        [HttpGet("activate/{id}")]
        public async Task<IActionResult> ActivateAccount(int id)
        {
            await _userService.ActivateAccount(id);

            return Ok(new { message = "Activation successful" });
        }

        [HttpPost("confirm")]
        public IActionResult SendConfirmationEmail(User user)
        {
            _userService.SendConfirmationEmail(user);

            return Ok(new { message = "Send confirmation email successful" });
        }

        [HttpGet("resetPassword/{email}")]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var updatedUser = await _userService.ResetPassword(email);

            return Ok(updatedUser);
        }

        [HttpPost("upload/{id}"), DisableRequestSizeLimit]
        public IActionResult Upload(int id)
        {
            var file = Request.Form.Files[0];

            return Ok(new { dbPath = _userService.Uploadfile(file, id) });
        }
    }
}
