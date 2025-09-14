using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanadoShared.Model;
using VanadoShared.Repository;

namespace VanadoShared.Service
{
    public class KvarService : IKvarService
    {
        private readonly IKvarRepository _kvarRepository;
        

        public KvarService(IKvarRepository repo)
        {
            _kvarRepository = repo;
        }

        public async Task<IEnumerable<Kvar>> DohvatiKvarove()
        {
            return await _kvarRepository.DohvatiKvarove();
        }

        public async Task<Kvar> DohvatiKvarId(int id)
        {
            var kvar = await _kvarRepository.DohvatiKvarId(id);
            if (kvar == null)
                throw new Exception("Kvar nije pronađen.");
            return kvar;
        }

        public async Task DodajKvar(Kvar kvar)
        {
            if (string.IsNullOrWhiteSpace(kvar.Opis))
                throw new ArgumentException("Opis ne smije biti prazan.");
            
            await _kvarRepository.DodajKvar(kvar);
        }

        public async Task UrediKvar(Kvar kvar)
        {
            await _kvarRepository.UrediKvar(kvar);
        }

        public async Task ObrisiKvar(int id)
        {
            await _kvarRepository.IzbrisiKvar(id);
        }

        public async Task PromijeniStatusKvara(int id, string noviStatus)
        {
            await _kvarRepository.PromijeniStatusKvara(id, noviStatus);
        }

        public async Task<IEnumerable<Kvar>> DohvatiKvaroveSortirano(int offset, int limit)
        {
            return await _kvarRepository.DohvatiKvaroveSortirano(offset, limit);
        }
    }
}
