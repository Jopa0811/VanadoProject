using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanadoShared.Model;

namespace VanadoShared.Service
{
    public interface IKvarService
    {
        Task<IEnumerable<Kvar>> DohvatiKvarove();
        Task<Kvar> DohvatiKvarId(int id);
        Task DodajKvar(Kvar kvar);
        Task UrediKvar(Kvar kvar);
        Task ObrisiKvar(int id);
        Task PromijeniStatusKvara(int id, string noviStatus);
        Task<IEnumerable<Kvar>> DohvatiKvaroveSortirano(int offset, int limit);
    }
}
