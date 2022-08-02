using LB_Gestor_API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.Interface
{
    public interface IContaGer
    {
        Task<IEnumerable<ContaGer>> GetAsync(string token, string login);
        Task<ResumoPagarReceber> GetResumoPagarReceber(string token, string login);
        Task<IEnumerable<ReceitaDespesa>> GetReceitasDespesasAsync(string token, string login, int mes, int ano, string tp_mov);
        Task<IEnumerable<ReceitaDespesa>> GetResultadoAnoAsync(string token, string login, int ano);
        Task<IEnumerable<FluxoCaixa>> GetFluxoCaixaAsync(string token, string login, DateTime dt_final);
        Task<IEnumerable<DRE>> GetDREAsync(string token, string login, int ano);
        Task<IEnumerable<RecebimentoPortador>> GetRecebimentoPortadorAsync(string token, string login, int mes, int ano);
    }
}
