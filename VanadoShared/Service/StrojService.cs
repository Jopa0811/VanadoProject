using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanadoShared.Model.dto;
using VanadoShared.Model;
using VanadoShared.Repository;

namespace VanadoShared.Service
{
    public class StrojService : IStrojService
    {
        private readonly IStrojRepository _strojRepository;

        public StrojService(IStrojRepository repo)
        {
            _strojRepository = repo;
        }

        public async Task<IEnumerable<Stroj>> DohvatiStrojeve()
        {
            return await _strojRepository.DohvatiStrojeve();
        }

        public async Task<StrojDetailsDto> DohvatiStroj(int id)
        {
            var stroj = await _strojRepository.DohvatiStrojId(id);
            if (stroj == null)
                throw new Exception("Stroj nije pronađen.");

            var kvarovi = (await _strojRepository.DohvatiKvaroveZaStroj(id)).ToList();

            var prosjecnoTrajanje = kvarovi
                .Where(k => k.VrijemeZavrsetka.HasValue)
                .Select(k => k.VrijemeZavrsetka.Value - k.VrijemePocetka)
                .DefaultIfEmpty()
                .Average(t => t.Ticks);

            return new StrojDetailsDto
            {
                Id = stroj.Id,
                Naziv = stroj.Naziv,
                Kvarovi = kvarovi,
                ProsjecnoTrajanjeKvarova = prosjecnoTrajanje > 0 ? TimeSpan.FromTicks((long)prosjecnoTrajanje) : null
            };
        }

        public async Task DodajStroj(Stroj stroj)
        {
            if (string.IsNullOrWhiteSpace(stroj.Naziv))
                throw new ArgumentException("Naziv ne smije biti prazan.");

            await _strojRepository.DodajStroj(stroj);
        }

        public async Task UrediStroj(Stroj stroj)
        {
            if (string.IsNullOrWhiteSpace(stroj.Id.ToString()))
                throw new ArgumentException("Id ne smije biti prazan.");

            if (string.IsNullOrWhiteSpace(stroj.Naziv))
                throw new ArgumentException("Naziv ne smije biti prazan.");

            await _strojRepository.UrediStroj(stroj);
        }

        public async Task ObrisiStroj(int id)
        {
            await _strojRepository.IzbrisiStroj(id);
        }
    }
}
