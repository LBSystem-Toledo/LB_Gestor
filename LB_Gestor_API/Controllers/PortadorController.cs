﻿using LB_Gestor_API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LB_Gestor_API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PortadorController : ControllerBase
    {
        readonly IPortador _query;
        public PortadorController(IPortador query) { _query = query; }

        [HttpGet, Route("GetAsync")]
        [Authorize]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var ret = await _query.GetAsync(Request.Headers["cnpj"].ToString());
                return Ok(ret);
            }
            catch { return BadRequest(); }
        }
    }
}
