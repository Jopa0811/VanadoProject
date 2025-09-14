using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using VanadoAPI.Hub;

namespace VanadoAPI
{
    public class ApiBase : ControllerBase
    {
        private readonly IHubContext<KvaroviHub> _hubContext;

        public ApiBase(IHubContext<KvaroviHub> hubContext)
        {
            _hubContext = hubContext;
        }

        protected async Task<IActionResult> ExecuteApiFunctionsAsync(Func<Task> func,
            Func<IHubContext<KvaroviHub>, Task>? hubAction = null)
        {
            try
            {
                await func();

                if (hubAction != null)
                {
                    await hubAction(_hubContext);
                }

                return Ok();
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ie)
            {
                return BadRequest(ie.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        protected async Task<ActionResult<T>> ExecuteApiFunctionsAsync<T>(Func<Task<T>> func,
            Func<IHubContext<KvaroviHub>, Task>? hubAction = null)
        {
            try
            {
                var result = await func();

                if (hubAction != null)
                {
                    await hubAction(_hubContext);
                }

                return Ok(result);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ie)
            {
                return BadRequest(ie.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
