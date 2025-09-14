using Moq;
using VanadoShared.Model;
using VanadoShared.Model.@enum;
using VanadoShared.Repository;
using VanadoShared.Service;

namespace TestVanado
{
    public class KvarServiceTests
    {
        [Fact]
        public async Task DohvatiSveKvarove_Test()
        {
            var mockRepo = new Mock<IKvarRepository>();
            mockRepo.Setup(repo => repo.DohvatiKvarove()).ReturnsAsync(new List<Kvar>
            {
                new Kvar { StrojId = 1, Opis = "Test 1", Prioritet = Prioritet.nizak, Status = Status.otvoren },
                new Kvar { StrojId = 2, Opis = "Test 2", Prioritet = Prioritet.visok, Status = Status.zatvoren }
            });

            var service = new KvarService(mockRepo.Object);

            var rezultat = await service.DohvatiKvarove();

            Assert.NotNull(rezultat);
            Assert.Equal(2, (rezultat as List<Kvar>)?.Count);
        }

        [Fact]
        public async Task DodajNoviKvar_Test()
        {
            var mockRepo = new Mock<IKvarRepository>();
            var service = new KvarService(mockRepo.Object);

            var noviKvar = new Kvar { StrojId = 3, Opis = "Novi kvar", Prioritet = Prioritet.srednji, Status = Status.otvoren };

            await service.DodajKvar(noviKvar);

            mockRepo.Verify(r => r.DodajKvar(It.Is<Kvar>(k => k.StrojId == 3 && k.Opis == "Novi kvar")), Times.Once);
        }
    }
}