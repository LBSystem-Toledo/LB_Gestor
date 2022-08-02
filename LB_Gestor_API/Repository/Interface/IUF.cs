using LB_Gestor_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.Interface
{
    public interface IUF
    {
        Task<IEnumerable<UF>> GetAsync(string token);
    }
}
