﻿using Fabrit.Heroes.Infrastructure.Common.Data;

namespace Fabrit.Heroes.Data.Entities.Hero
{
    public class HeroType : IDataEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}