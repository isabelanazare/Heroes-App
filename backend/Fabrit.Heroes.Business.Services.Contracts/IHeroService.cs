using Fabrit.Heroes.Data.Business;
using Fabrit.Heroes.Data.Business.Hero;
using Fabrit.Heroes.Data.Entities;
using Fabrit.Heroes.Data.Entities.Hero;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrit.Heroes.Business.Services.Contracts
{
    public interface IHeroService
    {
        IAsyncEnumerable<HeroDto> GetAll();
        IAsyncEnumerable<HeroType> GetAllTypes();
        Task<HeroDto> GetHero(int id);
        Task<HeroDto> AddHero(HeroDto hero);
        Task DeleteHero(int id);
        Task<HeroDto> UpdateHero(HeroDto hero);
        Task<Dictionary<int, int>> GetPowerFrequencies();
        Task<ChartDataDto> GetHeroChartData();
        string Uploadfile(IFormFile file, int heroId);
    }
}