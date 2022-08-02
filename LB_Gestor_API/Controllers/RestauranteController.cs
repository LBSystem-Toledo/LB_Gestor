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
    public class RestauranteController : ControllerBase
    {
        private readonly IRestaurante _query;
        public RestauranteController(IRestaurante query) { _query = query; }

        [HttpGet, Route("GetPedidosHojeAsync")]
        [Authorize]
        public async Task<IActionResult> GetPedidosHojeAsync()
        {
            try
            {
                var retorno = await _query.GetPedidosHojeAsync(Request.Headers["cnpj"].ToString());
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
        [HttpGet, Route("GetVendasGrupoHojeAsync")]
        [Authorize]
        public async Task<IActionResult> GetVendasGrupoHojeAsync()
        {
            try
            {
                var retorno = await _query.GetVendasGrupoHojeAsync(Request.Headers["cnpj"].ToString());
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
        [HttpGet, Route("GetPedidosOntemAsync")]
        [Authorize]
        public async Task<IActionResult> GetPedidosOntemAsync()
        {
            try
            {
                var retorno = await _query.GetPedidosOntemAsync(Request.Headers["cnpj"].ToString());
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
        [HttpGet, Route("GetVendasGrupoOntemAsync")]
        [Authorize]
        public async Task<IActionResult> GetVendasGrupoOntemAsync()
        {
            try
            {
                var retorno = await _query.GetVendasGrupoOntemAsync(Request.Headers["cnpj"].ToString());
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
        [HttpGet, Route("GetPedidosFimSemanaAsync")]
        [Authorize]
        public async Task<IActionResult> GetPedidosFimSemanaAsync()
        {
            try
            {
                var retorno = await _query.GetPedidosFimSemanaAsync(Request.Headers["cnpj"].ToString());
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
        [HttpGet, Route("GetPedidosOutrosDiasAsync")]
        [Authorize]
        public async Task<IActionResult> GetPedidosOutrosDiasAsync()
        {
            try
            {
                var retorno = await _query.GetPedidosOutrosDiasAsync(Request.Headers["cnpj"].ToString());
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
        [HttpGet, Route("GetEvolucao12MesesAsync")]
        [Authorize]
        public async Task<IActionResult> GetEvolucao12MesesAsync()
        {
            try
            {
                var retorno = await _query.GetEvolucao12MesesAsync(Request.Headers["cnpj"].ToString());
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
        [HttpGet, Route("GetEvolucao3AnosAsync")]
        [Authorize]
        public async Task<IActionResult> GetEvolucao3AnosAsync()
        {
            try
            {
                var retorno = await _query.GetEvolucao3AnosAsync(Request.Headers["cnpj"].ToString());
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
    }
}
