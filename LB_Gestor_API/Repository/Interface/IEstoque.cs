using LB_Gestor_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.Interface
{
    public interface IEstoque
    {
        Task<KPIEstoque> GetKPIEstoqueAsync(string token);
        Task<IEnumerable<ProdutoAbaixoMinimo>> GetProdutosAbaixoMinimoAsync(string token);
        Task<IEnumerable<ProdutoAbaixoMargemLucro>> GetProdutosAbaixoMargemLucroAsync(string token);
    }
}
