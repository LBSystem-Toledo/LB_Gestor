using LB_Gestor_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.Interface
{
    public interface ICliente
    {
        Task<IEnumerable<Cliente>> GetAsync(string token, string Cd_clifor, string Nome);
        Task<Cliente> GetFornecedorCNPJAsync(string token, string cnpj);
        Task<string> GravarAsync(string Token, Cliente cliente);
    }
}
