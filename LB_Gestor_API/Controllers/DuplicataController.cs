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
    public class DuplicataController : ControllerBase
    {
        private readonly IDuplicata _query;
        public DuplicataController(IDuplicata query) { _query = query; }

        [HttpGet, Route("GetAsync")]
        [Authorize]
        public async Task<IActionResult> GetAsync([FromQuery] string cd_clifor, 
                                                  [FromQuery] string tp_mov,
                                                  [FromQuery] decimal valor)
        {
            try
            {
                var ret = await _query.GetAsync(Request.Headers["cnpj"].ToString(), cd_clifor, tp_mov, valor);
                return Ok(ret);
            }
            catch { return BadRequest(); }
        }
        [HttpPost, Route("GravarAsync")]
        [Authorize]
        public async Task<IActionResult> GravarAsync([FromBody] Duplicata duplicata)
        {
            try
            {
                var ret = await _query.GravarAsync(Request.Headers["cnpj"].ToString(), duplicata);
                return Ok(ret);
            }
            catch(Exception ex) { return BadRequest(ex.Message.Trim()); }
        }
        [HttpPost, Route("ProrrogarVenctoAsync")]
        [Authorize]
        public async Task<IActionResult> ProrrogarVenctoAsync([FromBody] Duplicata duplicata)
        {
            try
            {
                var ret = await _query.ProrrogarVenctoAsync(Request.Headers["cnpj"].ToString(), duplicata);
                return Ok(ret);
            }
            catch (Exception ex) { return BadRequest(ex.Message.Trim()); }
        }
        [HttpPost, Route("QuitarDuplicataAsync")]
        [Authorize]
        public async Task<IActionResult> QuitarDuplicataAsync([FromBody] Duplicata duplicata)
        {
            try
            {
                var ret = await _query.QuitarDuplicataAsync(Request.Headers["cnpj"].ToString(), Request.Headers["login"].ToString(), duplicata);
                return Ok(ret);
            }
            catch (Exception ex) { return BadRequest(ex.Message.Trim()); }
        }
    }
}
