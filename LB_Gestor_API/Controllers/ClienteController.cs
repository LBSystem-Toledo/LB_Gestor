using LB_Gestor_API.Models;
using LB_Gestor_API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LB_Gestor_API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        readonly ICliente _query;
        public ClienteController(ICliente query) { _query = query; }

        [HttpGet, Route("GetAsync")]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] string Cd_cliente,
                                             [FromQuery] string Nome)
        {
            try
            {
                var result = await _query.GetAsync(Request.Headers["cnpj"].ToString(), Cd_cliente, Nome);
                return Ok(result);
            }
            catch { return BadRequest(); }
        }
        [HttpGet, Route("GetFornecedorCNPJAsync")]
        [Authorize]
        public async Task<IActionResult> GetFornecedorCNPJAsync([FromQuery] string cnpj)
        {
            try
            {
                var result = await _query.GetFornecedorCNPJAsync(Request.Headers["cnpj"].ToString(), cnpj);
                return Ok(result);
            }
            catch { return BadRequest(); }
        }
        [HttpPost, Route("GravarAsync")]
        [Authorize]
        public async Task<IActionResult> GravarAsync([FromBody] Cliente cliente)
        {
            try
            {
                var result = await _query.GravarAsync(Request.Headers["cnpj"].ToString(), cliente);
                return Ok(result);
            }
            catch(Exception ex) { return BadRequest(ex.Message.Trim()); }
        }
    }
}
