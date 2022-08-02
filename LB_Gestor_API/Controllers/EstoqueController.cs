using LB_Gestor_API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LB_Gestor_API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueController : ControllerBase
    {
        private readonly IEstoque _query;
        public EstoqueController(IEstoque query) { _query = query; }

        [HttpGet, Route("GetKPIAsync")]
        [Authorize]
        public async Task<IActionResult> GetKPIAsync()
        {
            try
            {
                var retorno = await _query.GetKPIEstoqueAsync(Request.Headers["cnpj"].ToString());
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }

        [HttpGet, Route("GetProdutosAbaixoMinimoAsync")]
        [Authorize]
        public async Task<IActionResult> GetProdutosAbaixoMinimoAsync()
        {
            try
            {
                var retorno = await _query.GetProdutosAbaixoMinimoAsync(Request.Headers["cnpj"].ToString());
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }

        [HttpGet, Route("GetProdutosAbaixoMargemLucroAsync")]
        [Authorize]
        public async Task<IActionResult> GetProdutosAbaixoMargemLucroAsync()
        {
            try
            {
                var retorno = await _query.GetProdutosAbaixoMargemLucroAsync(Request.Headers["cnpj"].ToString());
                return Ok(retorno);
            }
            catch { return NotFound(); }
        }
    }
}
