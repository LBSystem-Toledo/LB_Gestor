using LB_Gestor_API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LB_Gestor_API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UFController : ControllerBase
    {
        private readonly IUF _query;

        public UFController(IUF query) { _query = query; }

        [HttpGet, Route("GetAsync")]
        [Authorize]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var retorno = await _query.GetAsync(Request.Headers["cnpj"].ToString());
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
    }
}
