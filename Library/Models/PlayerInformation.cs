using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class PlayerInformation
    {
        public decimal GlobalPuntuation { get; set; }

        public decimal MonthPuntuation { get; set; }

        public decimal DRSPuntuation { get; set; }
        
        public decimal LastScore { get; set; }

        public int Position { get; set; }

        public decimal PorreroPuntuation { get; set; }

        public bool HasDRS { get; set; }

    }
}
