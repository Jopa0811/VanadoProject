using Microsoft.AspNetCore.Mvc;
using VanadoShared.Model.dto;
using VanadoShared.Model;
using VanadoShared.Repository;
using VanadoShared.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using VanadoAPI.Hub;

namespace VanadoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StrojController : ApiBase
    {
        private readonly IStrojService _strojService;

        public StrojController(IStrojService strojService, IHubContext<KvaroviHub> hubContext)
            : base(hubContext)
        {
            _strojService = strojService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stroj>>> GetStrojevi()
        {
            return await ExecuteApiFunctionsAsync(() => _strojService.DohvatiStrojeve());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StrojDetailsDto>> GetStrojDetails(int id)
        {
            return await ExecuteApiFunctionsAsync(() => _strojService.DohvatiStroj(id));
        }

        [HttpPost]
        public async Task<IActionResult> DodajStroj([FromBody] Stroj stroj)
        {
            return await ExecuteApiFunctionsAsync(() => _strojService.DodajStroj(stroj));
        }

        [HttpPut]
        public async Task<IActionResult> UrediStroj([FromBody] Stroj stroj)
        {
            return await ExecuteApiFunctionsAsync(() => _strojService.UrediStroj(stroj));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ObrisiStroj(int id)
        {
            return await ExecuteApiFunctionsAsync(() => _strojService.ObrisiStroj(id));
            
        }

        
    }
}
