using VanadoShared.Model;

namespace VanadoShared.Repository
{
    public interface IKvarRepository
    {
        Task<IEnumerable<Kvar>> DohvatiKvarove();
        Task<Kvar> DohvatiKvarId(int id);
        Task DodajKvar(Kvar kvar);
        Task UrediKvar(Kvar kvar);
        Task IzbrisiKvar(int id);
        Task PromijeniStatusKvara(int id, string noviStatus);
        Task<IEnumerable<Kvar>> DohvatiKvaroveSortirano(int offset, int limit);
    }
}