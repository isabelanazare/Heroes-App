using Fabrit.Heroes.Business.Services.Contracts;
using Fabrit.Heroes.Data.Entities;
using Fabrit.Heroes.Web.Infrastructure.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fabrit.Heroes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PowersController : ApiController
    {
        private readonly IPowerService _powerService;

        public PowersController(IPowerService powerService)
        {
            _powerService = powerService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_powerService.GetAll());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPowerAsync(int id)
        {
            var power = await _powerService.GetPower(id);

            return Ok(power);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPowerAsync(Power power)
        {
            var addedPower = await _powerService.AddPower(power);

            return Ok(addedPower);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdatePowerAsync(Power power)
        {
            var updatedPower = await _powerService.UpdatePower(power);

            return Ok(updatedPower);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePowerAsync(int id)
        {
            await _powerService.DeletePower(id);

            return NoContent();
        }

        [Authorize]
        [HttpGet("getPowerByName/{name}")]
        public async Task<IActionResult> GetPowerByNameAsync(string name)
        {
            var power = await _powerService.GetPowerByName(name);

            return Ok(power);
        }
    }
}
