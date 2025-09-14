using VanadoShared.Model.dto;
using VanadoShared.Model;

namespace VanadoShared.Repository
{
    public interface IStrojRepository
    {
        Task<IEnumerable<Stroj>> DohvatiStrojeve();
        Task<Stroj> DohvatiStrojId(int strojId);
        Task<IEnumerable<Kvar>> DohvatiKvaroveZaStroj(int strojId);
        Task DodajStroj(Stroj stroj);
        Task IzbrisiStroj(int id);
        Task UrediStroj(Stroj stroj);
    }
}