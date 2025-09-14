using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanadoShared.Model.@enum;

namespace VanadoShared.Model
{
    public class Kvar
    {
        public int Id { get; set; }
        public int StrojId { get; set; }
        public string Opis { get; set; }
        public DateTime VrijemePocetka { get; set; }
        public DateTime? VrijemeZavrsetka { get; set; }
        public Prioritet Prioritet { get; set; }
        public Status Status { get; set; }
    }
}
