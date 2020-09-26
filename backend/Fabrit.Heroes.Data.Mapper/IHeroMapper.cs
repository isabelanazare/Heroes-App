using Fabrit.Heroes.Data.Business.Hero;
using Fabrit.Heroes.Data.Entities.Hero;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fabrit.Heroes.Data.Mapper
{
    public interface IHeroMapper
    {
        public HeroDto ToHeroDto(Hero hero);
        public Task<Hero> ToEntity(HeroDto hero);
    }
}
