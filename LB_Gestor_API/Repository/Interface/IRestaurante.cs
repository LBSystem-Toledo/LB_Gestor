using LB_Gestor_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.Interface
{
    public interface IRestaurante
    {
        Task<IEnumerable<PedidoRestaurante>> GetPedidosHojeAsync(string token);
        Task<IEnumerable<VendaGrupo>> GetVendasGrupoHojeAsync(string token);
        Task<IEnumerable<PedidoRestaurante>> GetPedidosOntemAsync(string token);
        Task<IEnumerable<VendaGrupo>> GetVendasGrupoOntemAsync(string token);
        Task<IEnumerable<PedidoRestaurante>> GetPedidosFimSemanaAsync(string token);
        Task<IEnumerable<PedidoRestaurante>> GetPedidosOutrosDiasAsync(string token);
        Task<IEnumerable<PedidoRestaurante>> GetEvolucao12MesesAsync(string token);
        Task<IEnumerable<PedidoRestaurante>> GetEvolucao3AnosAsync(string token);
    }
}
