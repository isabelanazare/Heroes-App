using Fabrit.Heroes.Data.Business.Hero;
using Fabrit.Heroes.Data.Entities;
using Fabrit.Heroes.Data.Entities.Hero;
using System.Collections.Generic;
using System.Linq;
using Fabrit.Heroes.Data;
using Fabrit.Heroes.Data.Entities.Exception;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Fabrit.Heroes.Data.Mapper
{
    public class HeroMapper : IHeroMapper
    {
        private readonly HeroesDbContext _context;

        public HeroMapper(HeroesDbContext context)
        {
            _context = context;
        }

        public HeroDto ToHeroDto(Hero hero)
        {
            if (hero != null)
            {
                return new HeroDto()
                {
                    Id = hero.Id,
                    Name = hero.Name,
                    Ally = hero.Ally,
                    MainPower = hero.MainPower.Name != null
                    ? hero.MainPower.Name
                    : throw new InvalidEntityException("Main power can't be null"),
                    Powers = hero.Powers.Where(hp => hp.Power != null).Select(hp => hp.Power.Name) ?? null,
                    Type = hero.Type.Name ?? null,
                    ImgPath = hero.ImgPath,
                    Birthday = hero.Birthday
                };
            }
            return null;
        }

        public async Task<Hero> ToEntity(HeroDto heroDto)
        {
            var dbType = await _context.HeroTypes.FirstOrDefaultAsync(ht => ht.Name == heroDto.Type);
            var dbMainPower = await _context.Powers.FirstOrDefaultAsync(power => power.Name == heroDto.MainPower);

            var hero = new Hero
            {
                Id = heroDto.Id,
                Name = heroDto.Name,
                Ally = heroDto.Ally,
                Type = dbType,
                MainPower = dbMainPower != null
                ? dbMainPower
                : throw new EntityNotFoundException("Power doesn't exist"),
                OverallStrength = heroDto.OverallStrength,
                ImgPath = heroDto.ImgPath,
                Birthday = heroDto.Birthday
            };

            hero.Powers = new List<HeroPower>();
            if(heroDto.Powers != null)
            {
                foreach (string power in heroDto.Powers)
                {
                    var dbPower = await _context.Powers.FirstOrDefaultAsync(p => p.Name == power);
                    if (dbPower != null)
                    {
                        hero.Powers.Add(
                        new HeroPower
                        {
                            Hero = hero,
                            Power = dbPower
                        });
                    }
                    else
                    {
                        throw new EntityNotFoundException("Power doesn't exist");
                    }
                }
            }
            else
            {
                hero.Powers = null;
            }

            return hero;
        }
    }
}
