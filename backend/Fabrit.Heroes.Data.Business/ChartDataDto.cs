using Fabrit.Heroes.Data.Entities;
using System.Collections.Generic;

namespace Fabrit.Heroes.Data.Business
{
    public class ChartDataDto
    {
        public IEnumerable<ChartData> Powers { get; set; }
    }
}
