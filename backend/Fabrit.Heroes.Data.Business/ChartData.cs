using System.Collections.Generic;

namespace Fabrit.Heroes.Data.Entities
{
    public class ChartData
    {
        public string Label { get; set; }
        public List<int> Data { get; set; }

        public char Stack = Constants.Stack;
    }
}