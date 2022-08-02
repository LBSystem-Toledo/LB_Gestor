using LB_Gestor.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LB_Gestor.Interface
{
    public interface IDataService
    {
        Task<Token> ValidarLoginAsync(Usuario user);
        Task<IEnumerable<ContaGer>> GetSaldoContaAsync();
        Task<ResumoPagarReceber> GetResumoPagarReceberAsync();
        Task<IEnumerable<ReceitaDespesa>> GetReceitaDespesaAsync(int mes, int ano, string tp_mov);
        Task<IEnumerable<ReceitaDespesa>> GetResultadoAnoAsync(int ano);
        Task<IEnumerable<FluxoCaixa>> GetFluxoCaixaAsync(DateTime dt_final);
        Task<IEnumerable<DRE>> GetDREAsync(int ano);
        Task<IEnumerable<Duplicata>> GetDuplicatasAsync(string cd_clifor, string tp_mov, decimal valor);
        Task<bool> GravarDuplicataAsync(Duplicata duplicata);
        Task<bool> ProrrogarVenctoAsync(Duplicata duplicata);
        Task<bool> QuitarDuplicataAsync(Duplicata duplicata);
        Task<bool> GravarCaixaAsync(Caixa caixa);
        Task<IEnumerable<Historico>> GetHistoricosAsync(string tp_mov);
        Task<IEnumerable<PlanoConta>> GetPlanoContaAsync(string tp_mov);
        Task<IEnumerable<Portador>> GetPortadorAsync();
        Task<IEnumerable<Cliente>> GetClientesAsync(string cd_clifor, string nm_clifor);
        Task<Cliente> GetFornecedorCNPJAsync(string cnpj);
        Task<string> GravarClienteAsync(Cliente cliente);
        Task<TEndereco_CEPRest> GetCEPAsync(string cep);
        Task<ConsultaCNPJ> GetClienteCNPJAsync(string cnpj);
        Task<IEnumerable<Cidade>> GetCidadesAsync(string Cd_cidade, string Ds_cidade, string Uf);
        Task<IEnumerable<UF>> GetUFsAsync();
        Task<CfgDespAuto> GetCfgDespAutoAsync(string tp_despesa);
        Task<IEnumerable<RecebimentoPortador>> GetRecebimentoPortadorAsync(int mes, int ano);
        Task<IEnumerable<RecebimentoVendedor>> GetRecebimentoVendedorAsync(int mes, int ano);

        Task<KPIFaturamento> GetKPIFaturamentoAsync();
        Task<IEnumerable<Faturamento>> GetVendasPorAnoAsync();
        Task<IEnumerable<Faturamento>> GetVendasPorMesAsync(int ano);
        Task<IEnumerable<FaturamentoPortador>> GetVendasPortadorAsync(int mes, int ano);
        Task<IEnumerable<Faturamento>> GetVendasCidadeAsync(int mes, int ano);
        Task<IEnumerable<Faturamento>> GetVendasGrupoAsync(int mes, int ano);
        Task<IEnumerable<Faturamento>> GetVendasVendedoresAsync(int mes, int ano);

        #region Restaurante
        Task<IEnumerable<PedidoRestaurante>> GetPedidosHojeAsync();
        Task<IEnumerable<VendaGrupo>> GetVendasGrupoHojeAsync();
        Task<IEnumerable<PedidoRestaurante>> GetPedidosOntemAsync();
        Task<IEnumerable<VendaGrupo>> GetVendasGrupoOntemAsync();
        Task<IEnumerable<PedidoRestaurante>> GetPedidosFimSemanaAsync();
        Task<IEnumerable<PedidoRestaurante>> GetPedidosOutrosDiasAsync();
        Task<IEnumerable<PedidoRestaurante>> GetEvolucao12MesesAsync();
        Task<IEnumerable<PedidoRestaurante>> GetEvolucao3AnosAsync();
        #endregion

        #region Estoque
        Task<KPIEstoque> GetKPIEstoqueAsync();
        Task<IEnumerable<ProdutoAbaixoMinimo>> GetProdutosAbaixoMinimoAsync();
        Task<IEnumerable<ProdutoAbaixoMargemLucro>> GetProdutosAbaixoMargemLucroAsync();
        #endregion
    }
}
