using LB_Gestor_API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LB_Gestor_API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CidadeController : ControllerBase
    {
        private readonly ICidade _query;

        public CidadeController(ICidade query) { _query = query; }

        [HttpGet, Route("GetAsync")]
        [Authorize]
        public async Task<IActionResult> GetAsync([FromQuery] string Cd_cidade,
                                                  [FromQuery] string Ds_cidade,
                                                  [FromQuery] string Uf)
        {
            try
            {
                var retorno = await _query.GetAsync(Request.Headers["cnpj"].ToString(), Cd_cidade, Ds_cidade, Uf);
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
    }
}
