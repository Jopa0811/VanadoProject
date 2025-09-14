using VanadoShared.Model.dto;
using VanadoShared.Model;

namespace VanadoShared.Service
{
    public interface IStrojService
    {
        Task<IEnumerable<Stroj>> DohvatiStrojeve();
        Task<StrojDetailsDto> DohvatiStroj(int id);
        Task DodajStroj(Stroj stroj);
        Task UrediStroj(Stroj stroj);
        Task ObrisiStroj(int id);
    }
}