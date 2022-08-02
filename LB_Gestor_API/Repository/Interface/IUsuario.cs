using LB_Gestor_API.Models;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.Interface
{
    public interface IUsuario
    {
        Task<Token> ValidarUsuario(Usuario user);
    }
}
