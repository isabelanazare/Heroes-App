using System;
using System.Collections.Generic;

namespace Fabrit.Heroes.Data.Business.Hero
{
    public class HeroDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ally { get; set; }
        public int OverallStrength { get; set; }
        public string MainPower { get; set; }
        public IEnumerable<string> Powers { get; set; }
        public string Type { get; set; }
        public string ImgPath { get; set; }
        public DateTime Birthday { get; set; }
    }
}