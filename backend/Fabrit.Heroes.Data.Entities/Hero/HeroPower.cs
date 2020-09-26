using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fabrit.Heroes.Data.Entities.Hero
{
    public class HeroPower
    {
        public int HeroId { get; set; }
        public Hero Hero { get; set; }
        public int PowerId { get; set; }
        public Power Power { get; set; }
    }

    public class HeroPowerConfiguration : IEntityTypeConfiguration<HeroPower>
    {
        public void Configure(EntityTypeBuilder<HeroPower> builder)
        {
            builder
                .HasKey(hp => new {hp.HeroId, hp.PowerId});

            builder
                .HasOne(hp => hp.Hero)
                .WithMany(hero => hero.Powers)
                .HasForeignKey(hp => hp.HeroId);

            builder
                .HasOne(hp => hp.Power)
                .WithMany()
                .HasForeignKey(hp => hp.PowerId);
        }
    }
}