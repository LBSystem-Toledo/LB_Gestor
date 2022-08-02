using LB_Gestor_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.Interface
{
    public interface IKPIFaturamento
    {
        Task<KPIFaturamento> GetKPIFaturamentoAsync(string token, string login);
        Task<IEnumerable<Faturamento>> GetVendasPorMesAsync(string token, string login, int ano);
        Task<IEnumerable<Faturamento>> GetVendasPorAnoAsync(string token, string login);
        Task<IEnumerable<FaturamentoPortador>> GetVendasPortadorAsync(string token, string login, int mes, int ano);
        Task<IEnumerable<Faturamento>> GetVendasCidadeAsync(string token, string login, int mes, int ano);
        Task<IEnumerable<Faturamento>> GetVendasGrupoAsync(string token, string login, int mes, int ano);
        Task<IEnumerable<Faturamento>> GetVendasVendedoresAsync(string token, string login, int mes, int ano);
    }
}
