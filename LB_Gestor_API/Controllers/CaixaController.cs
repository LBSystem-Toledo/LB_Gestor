using LB_Gestor_API.Models;
using LB_Gestor_API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LB_Gestor_API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CaixaController : ControllerBase
    {
        private readonly ICaixa _query;
        public CaixaController(ICaixa query) { _query = query; }

        [HttpPost, Route("GravarAsync")]
        [Authorize]
        public async Task<IActionResult> GravarAsync([FromBody] Caixa caixa)
        {
            try
            {
                var retorno = await _query.GravarAsync(Request.Headers["cnpj"].ToString(), caixa);
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
    }
}
