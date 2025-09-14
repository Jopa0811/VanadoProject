using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanadoShared.DatabaseAccess;
using VanadoShared.Model.dto;
using VanadoShared.Model;
using System.Data;

namespace VanadoShared.Repository
{
    public class StrojRepository : IStrojRepository
    {
        private readonly IPostgresDataAccess _db;

        public StrojRepository(IPostgresDataAccess db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Stroj>> DohvatiStrojeve()
        {
            var sql = "SELECT * FROM Strojevi ORDER BY Naziv";
            return await _db.LoadDataAsync<Stroj, dynamic>(sql, new { });
        }

        public async Task<Stroj> DohvatiStrojId(int strojId)
        {
            var sql = "SELECT * FROM Strojevi WHERE Id = @Id";
            return (await _db.LoadDataAsync<Stroj, dynamic>(sql, new { Id = strojId })).FirstOrDefault();
        }

        public async Task<IEnumerable<Kvar>> DohvatiKvaroveZaStroj(int strojId)
        {
            var sql = "SELECT * FROM Kvarovi WHERE StrojId = @Id";
            return await _db.LoadDataAsync<Kvar, dynamic>(sql, new { Id = strojId });
        }

        public async Task DodajStroj(Stroj stroj)
        {
            await _db.SaveDataAsync("prc_dodaj_stroj", new { p_naziv = stroj.Naziv }, CommandType.StoredProcedure);
        }

        public async Task IzbrisiStroj(int id)
        {
            await _db.SaveDataAsync("prc_izbrisi_stroj_pesimisticki", new { Id = id }, CommandType.StoredProcedure);
        }

        public async Task UrediStroj(Stroj stroj)
        {
            var parameters = new
            {
                p_id = stroj.Id,
                p_naziv = stroj.Naziv
            };

            await _db.SaveDataAsync("prc_uredi_stroj_pesimistic", parameters, CommandType.StoredProcedure);
        }

    }
}
