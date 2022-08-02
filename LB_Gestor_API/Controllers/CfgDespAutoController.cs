using LB_Gestor_API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LB_Gestor_API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CfgDespAutoController : ControllerBase
    {
        private readonly ICfgDespAuto _query;
        public CfgDespAutoController(ICfgDespAuto query) { _query = query; }

        [HttpGet, Route("GetAsync")]
        [Authorize]
        public async Task<IActionResult> GetAsync([FromQuery] string tp_despesa)
        {
            try
            {
                var retorno = await _query.GetAsync(Request.Headers["cnpj"].ToString(), tp_despesa);
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
    }
}
