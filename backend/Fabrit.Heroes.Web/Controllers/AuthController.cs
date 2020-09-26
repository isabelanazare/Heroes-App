using Fabrit.Heroes.Business.Services.Contracts;
using Fabrit.Heroes.Data.Entities;
using Fabrit.Heroes.Web.Infrastructure.Controller;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fabrit.Heroes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiController
    {
        private IAuthService _authentificationService;

        public AuthController(IAuthService authentificationService)
        {
            _authentificationService = authentificationService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var authenticateResponse = await _authentificationService.Authenticate(model);

            return Ok(authenticateResponse);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _authentificationService.GetAll();

            return Ok(users);
        }
    }
}
