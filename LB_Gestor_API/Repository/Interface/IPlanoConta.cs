using LB_Gestor_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.Interface
{
    public interface IPlanoConta
    {
        Task<IEnumerable<PlanoConta>> GetAsync(string token, string tp_mov);
    }
}
