using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanadoShared.DatabaseAccess;
using VanadoShared.Model;

namespace VanadoShared.Repository
{
    public class KvarRepository : IKvarRepository
    {
        private readonly IPostgresDataAccess _db;

        public KvarRepository (IPostgresDataAccess db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Kvar>> DohvatiKvarove()
        {
            var sql = "SELECT * FROM Kvarovi";
            return await _db.LoadDataAsync<Kvar, dynamic>(sql, new { });
        }

        public async Task<Kvar> DohvatiKvarId(int id)
        {
            var sql = "SELECT * FROM Kvarovi WHERE Id = @Id";
            return (await _db.LoadDataAsync<Kvar, dynamic> (sql, new { Id = id })).FirstOrDefault();
        }

        public async Task DodajKvar(Kvar kvar)
        {
            var parameters = new
            {
                p_stroj_id = kvar.StrojId,
                p_opis = kvar.Opis,
                p_vrijeme_pocetka = kvar.VrijemePocetka,
                p_vrijeme_zavrsetka = kvar.VrijemeZavrsetka,
                p_prioritet = kvar.Prioritet.ToString().ToLower(), 
                p_status = kvar.Status.ToString().ToLower()
            };

            await _db.SaveDataAsync("prc_dodaj_kvar", parameters, CommandType.StoredProcedure);
        }

        public async Task UrediKvar(Kvar kvar)
        {
            var parameters = new
            {
                p_id = kvar.Id,
                p_stroj_id = kvar.StrojId,
                p_opis = kvar.Opis,
                p_vrijeme_pocetka = kvar.VrijemePocetka,
                p_vrijeme_zavrsetka = kvar.VrijemeZavrsetka,
                p_prioritet = kvar.Prioritet.ToString().ToLower(),
                p_status = kvar.Status.ToString().ToLower()
            };

            await _db.SaveDataAsync("prc_uredi_kvar_pesimisticki", parameters, CommandType.StoredProcedure);
        }

        public async Task IzbrisiKvar(int id)
        {
            using var connection = (NpgsqlConnection)_db.CreateConnection();
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            var lockSql = "SELECT 1 FROM Kvarovi WHERE Id = @Id FOR UPDATE";
            var exists = await connection.ExecuteScalarAsync<int?>(lockSql, new { Id = id }, transaction);

            if (exists == null)
                throw new InvalidOperationException($"Kvar s ID {id} ne postoji.");

            var deleteSql = "DELETE FROM Kvarovi WHERE Id = @Id";
            await connection.ExecuteAsync(deleteSql, new { Id = id }, transaction);

            await transaction.CommitAsync();
        }


        public async Task PromijeniStatusKvara(int id, string noviStatus)
        {
            var sql = "UPDATE Kvarovi SET Status = @Status::status_tip WHERE Id = @Id";
            await _db.SaveDataAsync(sql, new { Id = id, Status = noviStatus });
        }

        public async Task<IEnumerable<Kvar>> DohvatiKvaroveSortirano(int offset, int limit)
        {
            var sql = @"
            SELECT * FROM Kvarovi
            ORDER BY 
                CASE Prioritet 
                    WHEN 'nizak' THEN 1
                    WHEN 'srednji' THEN 2
                    WHEN 'visok' THEN 3
                END ASC,
                VrijemePocetka DESC
            OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

            return await _db.LoadDataAsync<Kvar, dynamic>(sql, new { Offset = offset, Limit = limit });
        }
    }
}
