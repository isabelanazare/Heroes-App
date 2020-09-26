using Fabrit.Heroes.Infrastructure.Common.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Fabrit.Heroes.Data.Entities.Hero
{
    public class Hero : IDataEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ally { get; set; }
        public Power MainPower { get; set; }
        public int OverallStrength { get; set; }
        public ICollection<HeroPower> Powers { get; set; }
        public HeroType Type { get; set; }
        public string ImgPath { get; set; }
        public DateTime Birthday { get; set; }
    }

    public class HeroConfiguration : IEntityTypeConfiguration<Hero>
    {
        public void Configure(EntityTypeBuilder<Hero> builder)
        {
            builder.HasKey(hero => hero.Id);
            builder.Property(hero => hero.Name).IsRequired();
        }
    }
}