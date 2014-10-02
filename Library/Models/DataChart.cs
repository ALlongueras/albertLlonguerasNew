using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class DataChart
    {
        public string Labels { get; set; }

        public List<DatasetsChart> Datasets { get; set; }
    }
}
