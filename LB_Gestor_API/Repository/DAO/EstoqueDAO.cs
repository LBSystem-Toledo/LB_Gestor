using Dapper;
using LB_Gestor_API.DataBase;
using LB_Gestor_API.Models;
using LB_Gestor_API.Repository.Interface;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.DAO
{
    public class EstoqueDAO: IEstoque
    {
        readonly IConfiguration _config;
        public EstoqueDAO(IConfiguration config) { _config = config; }

        public async Task<KPIEstoque> GetKPIEstoqueAsync(string token)
        {
            try
            {
                KPIEstoque kpi = new KPIEstoque();
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.AppendLine("select isnull(SUM(isnull(a.Vl_SaldoEstoque, 0)), 0) as ValorEstoque,")
                            .AppendLine("COUNT(case when ISNULL(a.QT_Min_Estoque, 0) > a.Tot_Saldo then 1 end) as ProdutosAbaixoMinimo")
                            .AppendLine("from VTB_EST_VLESTOQUE a")
                            .AppendLine("inner join VTB_DIV_EMPRESA b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("where dbo.FVALIDA_NUMEROS(case when b.tp_pessoa = 'F' then b.NR_CPF else b.NR_CGC end) = '" + token +"'");
                        kpi = await conexao._conexao.QueryFirstOrDefaultAsync<KPIEstoque>(sql.ToString());
                        if (kpi == null)
                            kpi = new KPIEstoque();
                        kpi.ProdutosAbaixoMargemLucro =
                            await conexao._conexao.ExecuteScalarAsync<int>("select count(*) " +
                                                                           "from VTB_EST_PRECOVENDA a " +
                                                                           "inner join VTB_DIV_EMPRESA b " +
                                                                           "on a.CD_Empresa = b.CD_Empresa " +
                                                                           "where dbo.FVALIDA_NUMEROS(case when b.tp_pessoa = 'F' then b.NR_CPF else b.NR_CGC end) = '" + token + "' " +
                                                                           "and case when a.vl_precovenda > 0 then 100 - ((dbo.F_FAT_ULTIMACOMPRA(a.CD_Empresa, a.CD_Produto) / a.vl_precovenda) * 100) else 100 end < 20");
                        return kpi;
                    }
                    else return null;
                }
            }
            catch { return null; }
        }

        public async Task<IEnumerable<ProdutoAbaixoMinimo>> GetProdutosAbaixoMinimoAsync(string token)
        {
            try
            {
                KPIEstoque kpi = new KPIEstoque();
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.AppendLine("select c.DS_Produto, a.Tot_Saldo, a.QT_Min_Estoque")
                            .AppendLine("from VTB_EST_VLESTOQUE a")
                            .AppendLine("inner join VTB_DIV_EMPRESA b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("inner join TB_EST_Produto c")
                            .AppendLine("on a.cd_produto = c.CD_Produto")
                            .AppendLine("where dbo.FVALIDA_NUMEROS(case when b.tp_pessoa = 'F' then b.NR_CPF else b.NR_CGC end) = '" + token + "'")
                            .AppendLine("and ISNULL(a.QT_Min_Estoque, 0) > a.Tot_Saldo");
                        return await conexao._conexao.QueryAsync<ProdutoAbaixoMinimo>(sql.ToString());
                    }
                    else return null;
                }
            }
            catch { return null; }
        }

        public async Task<IEnumerable<ProdutoAbaixoMargemLucro>> GetProdutosAbaixoMargemLucroAsync(string token)
        {
            try
            {
                KPIEstoque kpi = new KPIEstoque();
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.AppendLine("select a.DS_TabelaPreco, a.DS_Produto, a.vl_precovenda, dbo.F_FAT_ULTIMACOMPRA(a.CD_Empresa, a.CD_Produto) UltimaCompra")
                            .AppendLine("from VTB_EST_PRECOVENDA a")
                            .AppendLine("inner join VTB_DIV_EMPRESA b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("where dbo.FVALIDA_NUMEROS(case when b.tp_pessoa = 'F' then b.NR_CPF else b.NR_CGC end) = '" + token + "'")
                            .AppendLine("and case when a.vl_precovenda > 0 then 100 - ((dbo.F_FAT_ULTIMACOMPRA(a.CD_Empresa, a.CD_Produto) / a.vl_precovenda) * 100) else 100 end < 20");
                        return await conexao._conexao.QueryAsync<ProdutoAbaixoMargemLucro>(sql.ToString());
                    }
                    else return null;
                }
            }
            catch { return null; }
        }
    }
}
