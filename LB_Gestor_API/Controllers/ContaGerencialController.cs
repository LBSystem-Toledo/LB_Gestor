using LB_Gestor_API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LB_Gestor_API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContaGerencialController : ControllerBase
    {
        readonly IContaGer _query;
        public ContaGerencialController(IContaGer query) { _query = query; }
        [HttpGet, Route("GetAsync")]
        [Authorize]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var ret = await _query.GetAsync(Request.Headers["cnpj"].ToString(), Request.Headers["login"].ToString());
                return Ok(ret);
            }
            catch { return BadRequest(); }
        }
        [HttpGet, Route("GetResumoPagarReceberAsync")]
        public async Task<IActionResult> GetResumoPagarReceberAsync()
        {
            try
            {
                var ret = await _query.GetResumoPagarReceber(Request.Headers["cnpj"].ToString(), Request.Headers["login"].ToString());
                return Ok(ret);
            }
            catch { return BadRequest(); }
        }
        [HttpGet, Route("GetReceitasDespesasAsync")]
        public async Task<IActionResult> GetReceitasDespesasAsync([FromQuery] int mes, [FromQuery] int ano, [FromQuery] string tp_mov)
        {
            try
            {
                var ret = await _query.GetReceitasDespesasAsync(Request.Headers["cnpj"].ToString(), Request.Headers["login"].ToString(), mes, ano, tp_mov);
                return Ok(ret);
            }
            catch { return BadRequest(); }
        }
        [HttpGet, Route("GetResultadoAnoAsync")]
        public async Task<IActionResult> GetResultadoAnoAsync([FromQuery] int ano)
        {
            try
            {
                var ret = await _query.GetResultadoAnoAsync(Request.Headers["cnpj"].ToString(), Request.Headers["login"].ToString(), ano);
                return Ok(ret);
            }
            catch { return BadRequest(); }
        }
        [HttpGet, Route("GetFluxoCaixaAsync")]
        public async Task<IActionResult> GetFluxoCaixaAsync([FromQuery] string dt_final)
        {
            try
            {
                var ret = await _query.GetFluxoCaixaAsync(Request.Headers["cnpj"].ToString(), Request.Headers["login"].ToString(), DateTime.Parse(dt_final));
                return Ok(ret);
            }
            catch { return BadRequest(); }
        }
        [HttpGet, Route("GetDREAsync")]
        public async Task<IActionResult> GetDREAsync([FromQuery] int ano)
        {
            try
            {
                var ret = await _query.GetDREAsync(Request.Headers["cnpj"].ToString(), Request.Headers["login"].ToString(), ano);
                return Ok(ret);
            }
            catch { return BadRequest(); }
        }
        [HttpGet, Route("GetRecebimentoPortadorAsync")]
        public async Task<IActionResult> GetRecebimentoPortadorAsync([FromQuery] int mes, [FromQuery] int ano)
        {
            try
            {
                var ret = await _query.GetRecebimentoPortadorAsync(Request.Headers["cnpj"].ToString(), Request.Headers["login"].ToString(), mes, ano);
                return Ok(ret);
            }
            catch { return BadRequest(); }
        }
    }
}
