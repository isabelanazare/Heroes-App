using Fabrit.Heroes.Business.Services.Contracts;
using Fabrit.Heroes.Data.Business.Hero;
using Fabrit.Heroes.Web.Infrastructure.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fabrit.Heroes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class HeroesController : ApiController
    {
        private readonly IHeroService _heroService;

        public HeroesController(IHeroService heroService)
        {
            _heroService = heroService;
        }
        
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_heroService.GetAll());
        }

        [Authorize]
        [HttpGet("types")]
        public IActionResult GetAllTypes()
        {
            return Ok(_heroService.GetAllTypes());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHeroAsync(int id)
        {
            var hero = await _heroService.GetHero(id);

            return Ok(hero);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddHeroAsync(HeroDto hero)
        {
            var addedHero = await _heroService.AddHero(hero);

            return Ok(addedHero);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateHeroAsync(HeroDto hero)
        {
            var updatedHero = await _heroService.UpdateHero(hero);

            return Ok(updatedHero);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHeroAsync(int id)
        {
            await _heroService.DeleteHero(id);

            return NoContent();
        }

        [Authorize]
        [HttpGet("frequencies")]
        public async Task<IActionResult> GetPowerFrequencies()
        {
            return Ok(await _heroService.GetPowerFrequencies());
        }

        [Authorize]
        [HttpGet("heroChartData")]
        public async Task<IActionResult> GetHeroChartData()
        {
            return Ok(await _heroService.GetHeroChartData());
        }

        [HttpPost("upload/{heroId}"), DisableRequestSizeLimit]
        public IActionResult Upload(int heroId)
        {
            var file = Request.Form.Files[0];

            return Ok(new { dbPath = _heroService.Uploadfile(file, heroId) });
        }
    }
}
