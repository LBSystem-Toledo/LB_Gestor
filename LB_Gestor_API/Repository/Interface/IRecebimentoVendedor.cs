using LB_Gestor_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.Interface
{
    public interface IRecebimentoVendedor
    {
        Task<IEnumerable<RecebimentoVendedor>> GetRecebimentoVendedorAsync(string token, string login, int mes, int ano);
    }
}
