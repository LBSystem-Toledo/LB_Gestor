using LB_Gestor_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.Interface
{
    public interface IDuplicata
    {
        Task<IEnumerable<Duplicata>> GetAsync(string token, string cd_clifor, string tp_mov, decimal valor);
        Task<bool> GravarAsync(string token, Duplicata duplicata);
        Task<bool> ProrrogarVenctoAsync(string token, Duplicata duplicata);
        Task<bool> QuitarDuplicataAsync(string token, string login, Duplicata duplicata);
    }
}
