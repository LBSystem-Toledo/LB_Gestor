using LB_Gestor_API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LB_Gestor_API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class KPIFaturamentoController : ControllerBase
    {
        private readonly IKPIFaturamento _query;
        public KPIFaturamentoController(IKPIFaturamento query) { _query = query; }

        [HttpGet, Route("GetKPIAsync")]
        [Authorize]
        public async Task<IActionResult> GetKPIAsync()
        {
            try
            {
                var retorno = await _query.GetKPIFaturamentoAsync(Request.Headers["cnpj"].ToString(), Request.Headers["login"].ToString());
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
        [HttpGet, Route("GetVendasPorAnoAsync")]
        [Authorize]
        public async Task<IActionResult> GetVendasPorAnoAsync()
        {
            try
            {
                var retorno = await _query.GetVendasPorAnoAsync(Request.Headers["cnpj"].ToString(), Request.Headers["login"].ToString());
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
        [HttpGet, Route("GetVendasPorMesAsync")]
        [Authorize]
        public async Task<IActionResult> GetVendasPorMesAsync([FromQuery] int ano)
        {
            try
            {
                var retorno = await _query.GetVendasPorMesAsync(Request.Headers["cnpj"].ToString(), Request.Headers["login"].ToString(), ano);
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
        [HttpGet, Route("GetVendasPortadorAsync")]
        [Authorize]
        public async Task<IActionResult> GetVendasPortadorAsync([FromQuery] int mes, [FromQuery] int ano)
        {
            try
            {
                var retorno = await _query.GetVendasPortadorAsync(Request.Headers["cnpj"].ToString(), Request.Headers["login"].ToString(), mes, ano);
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
        [HttpGet, Route("GetVendasCidadeAsync")]
        [Authorize]
        public async Task<IActionResult> GetVendasCidadeAsync([FromQuery] int mes, [FromQuery] int ano)
        {
            try
            {
                var retorno = await _query.GetVendasCidadeAsync(Request.Headers["cnpj"].ToString(), Request.Headers["login"].ToString(), mes, ano);
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
        [HttpGet, Route("GetVendasGrupoAsync")]
        [Authorize]
        public async Task<IActionResult> GetVendasGrupoAsync([FromQuery] int mes, [FromQuery] int ano)
        {
            try
            {
                var retorno = await _query.GetVendasGrupoAsync(Request.Headers["cnpj"].ToString(), Request.Headers["login"].ToString(), mes, ano);
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
        [HttpGet, Route("GetVendasVendedoresAsync")]
        [Authorize]
        public async Task<IActionResult> GetVendasVendedoresAsync([FromQuery] int mes, [FromQuery] int ano)
        {
            try
            {
                var retorno = await _query.GetVendasVendedoresAsync(Request.Headers["cnpj"].ToString(), Request.Headers["login"].ToString(), mes, ano);
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
    }
}
