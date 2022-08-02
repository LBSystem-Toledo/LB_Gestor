using LB_Gestor_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.Interface
{
    public interface ICidade
    {
        Task<IEnumerable<Cidade>> GetAsync(string token, string cd_cidade, string ds_cidade, string uf);
    }
}
