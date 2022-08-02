using LB_Gestor_API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LB_Gestor_API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricoController : ControllerBase
    {
        private readonly IHistorico _query;
        public HistoricoController(IHistorico query) { _query = query; }

        [HttpGet, Route("GetAsync")]
        [Authorize]
        public async Task<IActionResult> GetAsync([FromQuery] string tp_mov)
        {
            try
            {
                var ret = await _query.GetAsync(Request.Headers["cnpj"].ToString(), tp_mov);
                return Ok(ret);
            }
            catch { return BadRequest(); }
        }
    }
}
