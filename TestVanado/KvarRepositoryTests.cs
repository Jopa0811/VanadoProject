using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanadoShared.DatabaseAccess;
using VanadoShared.Model.@enum;
using VanadoShared.Model;
using VanadoShared.Repository;
using System.Data;

namespace TestVanado
{
    public class KvarRepositoryTests
    {
        [Fact]
        public async Task DohvatiSveKvarove_Test()
        {
            var mockDb = new Mock<IPostgresDataAccess>();

            var mockRezultat = new List<Kvar>
            {
                new Kvar { StrojId = 1, Opis = "Mock 1", Prioritet = Prioritet.nizak, Status = Status.otvoren }
            };

            mockDb.Setup(db => db.LoadDataAsync<Kvar, dynamic>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CommandType>()))
                  .ReturnsAsync(mockRezultat);

            var repo = new KvarRepository(mockDb.Object);

            var rezultat = await repo.DohvatiKvarove();

            Assert.Single(rezultat);
            Assert.Equal("Mock 1", rezultat.First().Opis);
        }

        [Fact]
        public async Task DodajKvar_Test()
        {
            var mockDb = new Mock<IPostgresDataAccess>();

            var repo = new KvarRepository(mockDb.Object);

            var kvar = new Kvar
            {
                StrojId = 5,
                Opis = "Testni kvar",
                VrijemePocetka = DateTime.UtcNow,
                VrijemeZavrsetka = null,
                Prioritet = Prioritet.srednji,
                Status = Status.otvoren
            };

            await repo.DodajKvar(kvar);

            mockDb.Verify(db => db.SaveDataAsync(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<CommandType>()),
                Times.Once);
        }
    }
}
