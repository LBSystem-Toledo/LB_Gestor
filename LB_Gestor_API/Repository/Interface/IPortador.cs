using LB_Gestor_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.Interface
{
    public interface IPortador
    {
        Task<IEnumerable<Portador>> GetAsync(string token);
    }
}
