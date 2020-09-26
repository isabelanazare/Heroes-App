using Fabrit.Heroes.Data.Entities;
using Fabrit.Heroes.Data.Entities.Hero;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;

namespace Fabrit.Heroes.Data
{
    public class HeroesDbConfiguration
    {
        private readonly HeroesDbContext _context;

        public HeroesDbConfiguration(HeroesDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method is called at startup to ensure the database is created and to seed any data that is required on a first run (static data for example).
        /// </summary>
        public void Seed()
        {
            _context.Database.EnsureCreated();

            // seed functionality
            if (!_context.Heroes.Any()) AddHeroes();

            _context.SaveChanges();
        }

        private void AddHeroes()
        {
            var hulk = new Hero
            {
                Name = "Hulk",
                Type = new HeroType
                {
                    Name = "Real"
                },
                Ally = "Flash",
                OverallStrength = 100,
                MainPower = new Power
                {
                    Name = "Power1",
                    Element = "Earth",
                    Details = "details here",
                    MainTrait = "some main trait",
                    Strength = 100
                }
            };

            var flash = new Hero
            {
                Name = "Flash",
                Type = new HeroType
                {
                    Name = "Fictional"
                },
                Ally = "Hulk",
                OverallStrength = 100,
                MainPower = new Power
                {
                    Name = "Power2",
                    Element = "Water",
                    Details = "details here",
                    MainTrait = "some main trait",
                    Strength = 100
                }
            };

            hulk.Powers = new List<HeroPower>
            {
                new HeroPower
                {
                    Hero = hulk,
                    Power = new Power
                    {
                        Name = "Strength",
                        Element = "Water",
                        Details = "details here",
                        MainTrait = "some main trait",
                        Strength = 100
                    }
                },
                 new HeroPower
                {
                    Hero = hulk,
                    Power = new Power
                    {
                        Name = "Speed",
                        Element = "Air",
                        Details = "details here",
                        MainTrait = "some main trait",
                        Strength = 100
                    }
                }
            };

            flash.Powers = new List<HeroPower>
            {
                new HeroPower
                {
                    Hero = flash,
                    Power = new Power
                    {
                        Name = "Speed",
                        Element = "Water",
                        Details = "details here",
                        MainTrait = "some main trait",
                        Strength = 100
                    }
                }
            };

            _context.Add(hulk);
            _context.Add(flash);
        }
    }
}