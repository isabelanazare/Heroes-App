using Fabrit.Heroes.Business.Services.Contracts;
using Fabrit.Heroes.Data;
using Fabrit.Heroes.Data.Business.Hero;
using Fabrit.Heroes.Data.Entities.Hero;
using Fabrit.Heroes.Data.Entities;
using Fabrit.Heroes.Data.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fabrit.Heroes.Data.Entities.Exception;
using Fabrit.Heroes.Data.Business;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http.Headers;

namespace Fabrit.Heroes.Business.Services
{
    public class HeroService : IHeroService
    {
        private readonly HeroesDbContext _context;

        private readonly IHeroMapper _mapper;

        public HeroService(HeroesDbContext context, IHeroMapper autoMapper)
        {
            _context = context;
            _mapper = autoMapper;
        }

        public IAsyncEnumerable<HeroDto> GetAll()
        {
            return _context.Heroes
                .Include(h => h.Powers).ThenInclude(hp => hp.Power)
                .Include(h => h.Type)
                .Select(hero => new HeroDto
                {
                    Id = hero.Id,
                    Name = hero.Name,
                    Ally = hero.Ally,
                    MainPower = hero.MainPower.Name,
                    Powers = hero.Powers.Where(hp => hp.Power != null).Select(hp => hp.Power.Name),
                    Type = hero.Type.Name,
                    ImgPath = hero.ImgPath,
                    Birthday = hero.Birthday
                })
                .AsAsyncEnumerable();
        }

        public IAsyncEnumerable<HeroType> GetAllTypes()
        {
            return _context.HeroTypes
                .AsAsyncEnumerable();
        }

        public async Task<HeroDto> GetHero(int id)
        {
            Hero hero = await _context.Heroes
                               .Include(h => h.Powers).ThenInclude(hp => hp.Power)
                               .Include(h => h.Type)
                               .FirstOrDefaultAsync(e => e.Id == id);
            if (hero == null)
            {
                throw new EntityNotFoundException("Hero not found");
            }

            return _mapper.ToHeroDto(hero);
        }

        public async Task<HeroDto> AddHero(HeroDto heroDto)
        {
            ValidateHeroDto(heroDto);
            Hero hero = await _mapper.ToEntity(heroDto);
            await _context.Heroes.AddAsync(hero);
            await _context.SaveChangesAsync();
            heroDto.Id = hero.Id;
            return heroDto;
        }

        private void ValidateHeroDto(HeroDto heroDto)
        {
            if (string.IsNullOrWhiteSpace(heroDto.Name))
            {
                throw new InvalidEntityException("Hero is invalid");
            }
        }

        public async Task<HeroDto> UpdateHero(HeroDto heroDto)
        {
            var dbHero = await _context.Heroes.AsNoTracking().FirstOrDefaultAsync(hero => hero.Id == heroDto.Id);

            if (dbHero == null)
            {
                throw new EntityNotFoundException("hero doesn't exist");
            }

            ValidateHeroDto(heroDto);

            Hero hero = await _mapper.ToEntity(heroDto);

            _context.Entry(hero).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return heroDto;
        }

        public async Task DeleteHero(int id)
        {
            var hero = await _context.Heroes.FirstOrDefaultAsync(h => h.Id == id);

            if (hero == null)
            {
                throw new EntityNotFoundException("hero doesn't exist");
            }

            _context.Heroes.Remove(hero);
            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<int, int>> GetPowerFrequencies()
        {
            Dictionary<int, int> frequencies = new Dictionary<int, int>();

            await foreach (Power power in _context.Powers)
            {
                frequencies.Add(power.Id, 0);
            }

            await foreach (HeroPower heroPower in _context.HeroPowers)
            {

                frequencies[heroPower.PowerId]++;
            }

            return frequencies;
        }

        public async Task<ChartDataDto> GetHeroChartData()
        {
            var heroes = await _context.Heroes.Include(h => h.Powers).ThenInclude(p => p.Power).ToArrayAsync();
            var powers = await _context.Powers.OrderByDescending(power => power.Strength).ToArrayAsync();

            ChartDataDto chartDataDto = new ChartDataDto();
            List<ChartData> result = new List<ChartData>();

            foreach (var power in powers)
            {
                var chartData = new ChartData
                {
                    Data = new List<int>(),
                    Label = power.Name,
                };

                foreach (var hero in heroes)
                {
                    var heroPower =  hero.Powers.FirstOrDefault(hp => hp.Power.Id == power.Id);
                    if (heroPower != null)
                    {
                        chartData.Data.Add(heroPower.Power.Strength);
                    }
                    else
                    {
                        chartData.Data.Add(0);
                    }
                }
                result.Add(chartData);
            }

            chartDataDto.Powers = result;
            return chartDataDto;
        }

        public void ValidateFileName(string fileName)
        {
            var extension = fileName.Substring(fileName.Length - 3);

            if (Array.IndexOf(Constants.ImgExtensions, extension) == -1)
            {
                throw new InvalidFileException("Bad file extension");
            }
        }

        public string Uploadfile(IFormFile file, int heroId)
        {
            var folderName = Path.Combine(Constants.PicturesFolderName, Constants.PictureHeroesSubFolderName, heroId.ToString());
            Directory.CreateDirectory(folderName);

            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            string dbPath;

            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"').ToString();
                var fullPath = Path.Combine(pathToSave, fileName);

                ValidateFileName(fileName);

                dbPath = Path.Combine(folderName, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return dbPath;
            }
            else throw new InvalidFileException("File length < 0");
        }
    }
}
