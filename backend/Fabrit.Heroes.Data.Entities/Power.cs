using Fabrit.Heroes.Infrastructure.Common.Data;

namespace Fabrit.Heroes.Data.Entities
{
    public class Power : IDataEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Element { get; set; }
        public string Details { get; set; }
        public int Strength { get; set; }
        public string MainTrait { get; set; }
    }
}