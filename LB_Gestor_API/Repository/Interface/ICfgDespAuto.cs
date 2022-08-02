using LB_Gestor_API.Models;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.Interface
{
    public interface ICfgDespAuto
    {
        Task<CfgDespAuto> GetAsync(string token, string tp_despesa);
    }
}
