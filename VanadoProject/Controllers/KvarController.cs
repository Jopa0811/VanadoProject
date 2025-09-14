using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using VanadoAPI.Hub;
using VanadoShared.Model;
using VanadoShared.Service;

namespace VanadoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class KvarController : ApiBase
    {
        private readonly IKvarService _serviceKvar;
        private readonly IHubContext<KvaroviHub> _hubContext;

        public KvarController(IKvarService serviceKvar, IHubContext<KvaroviHub> hubContext)
            : base(hubContext)
        {
            _serviceKvar = serviceKvar;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kvar>>> Get()
        {
            return await ExecuteApiFunctionsAsync(() => _serviceKvar.DohvatiKvarove());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Kvar>> Get(int id)
        {
            return await ExecuteApiFunctionsAsync(() => _serviceKvar.DohvatiKvarId(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Kvar kvar)
        {
            return await ExecuteApiFunctionsAsync(
                () => _serviceKvar.DodajKvar(kvar),
                hub => hub.Clients.All.SendAsync("KvarDodan", kvar));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Kvar kvar)
        {
            return await ExecuteApiFunctionsAsync(() => _serviceKvar.UrediKvar(kvar),
                hub => hub.Clients.All.SendAsync("KvarStatusPromijenjen", new
                {
                    Id = kvar.Id,
                    NoviStatus = kvar.Status
                }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await ExecuteApiFunctionsAsync(() => _serviceKvar.ObrisiKvar(id));
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> PromijeniStatus(int id, [FromBody] string noviStatus)
        {
            return await ExecuteApiFunctionsAsync(() => _serviceKvar.PromijeniStatusKvara(id, noviStatus));
        }

        [HttpGet("sortirano")]
        public async Task<ActionResult<IEnumerable<Kvar>>> GetSortirano([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            return await ExecuteApiFunctionsAsync(() => _serviceKvar.DohvatiKvaroveSortirano(offset, limit));
        }
    }
}
