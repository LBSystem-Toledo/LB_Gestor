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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuario _query;
        public UsuarioController(IUsuario query) { _query = query; }

        [HttpPost, Route("ValidarLoginAsync")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidarLoginAsync([FromBody]Usuario user)
        {
            try
            {
                var ret = await _query.ValidarUsuario(user);
                return Ok(ret);
            }
            catch { return BadRequest(); }
        }
    }
}
