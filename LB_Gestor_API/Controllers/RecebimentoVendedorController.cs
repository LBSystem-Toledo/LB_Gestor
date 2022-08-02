using LB_Gestor_API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LB_Gestor_API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RecebimentoVendedorController : ControllerBase
    {
        private readonly IRecebimentoVendedor _query;
        public RecebimentoVendedorController(IRecebimentoVendedor query) { _query = query; }

        [HttpGet, Route("GetRecebimentoVendedorAsync")]
        [Authorize]
        public async Task<IActionResult> GetRecebimentoVendedorAsync([FromQuery] int mes, [FromQuery] int ano)
        {
            try
            {
                var retorno = await _query.GetRecebimentoVendedorAsync(Request.Headers["cnpj"].ToString(), 
                                                                       Request.Headers["login"].ToString(),
                                                                       mes,
                                                                       ano);
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
    }
}
