using LB_Gestor_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.Interface
{
    public interface IHistorico
    {
        Task<IEnumerable<Historico>> GetAsync(string token, string tp_mov);
    }
}
