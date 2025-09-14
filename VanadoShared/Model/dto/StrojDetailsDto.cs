using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanadoShared.Model;

namespace VanadoShared.Model.dto
{
    public class StrojDetailsDto
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public List<Kvar> Kvarovi { get; set; } = new();
        public TimeSpan? ProsjecnoTrajanjeKvarova { get; set; }
    }
}
