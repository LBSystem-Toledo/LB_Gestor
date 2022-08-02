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
    public class KPIFaturamentoDAO: IKPIFaturamento
    {
        readonly IConfiguration _config;
        public KPIFaturamentoDAO(IConfiguration config) { _config = config; }

        public async Task<KPIFaturamento> GetKPIFaturamentoAsync(string token, string login)
        {
            try
            {
                KPIFaturamento kpi = new KPIFaturamento();
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        StringBuilder sql = new StringBuilder();
                        //Detalhes Mes
                        sql.AppendLine("select isnull(sum(isnull(a.Vl_SubTotal, 0) - isnull(a.Vl_Desconto, 0) + isnull(a.Vl_Acrescimo, 0) + isnull(a.vl_juro_fin, 0) + isnull(a.vl_frete, 0)), 0) as AcumuladoMes,")
                            .AppendLine("ISNULL(SUM(ISNULL(a.Vl_cmv, 0)), 0) as CmvMes,")
                            .AppendLine("isnull(sum(case when a.Fiscal = 'S' then isnull(a.Vl_SubTotal, 0) - isnull(a.Vl_Desconto, 0) + isnull(a.Vl_Acrescimo, 0) + isnull(a.vl_juro_fin, 0) + isnull(a.vl_frete, 0) else 0 end), 0) as VendaMesFiscal")
                            .AppendLine("from VTB_FAT_VENDAS_VR_NF a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("and b.Login = '" + login.Trim() + "'")
                            .AppendLine("where MONTH(a.dt_emissao) = MONTH(GETDATE())")
                            .AppendLine("and YEAR(a.dt_emissao) = YEAR(GETDATE())");
                        kpi = await conexao._conexao.QueryFirstOrDefaultAsync<KPIFaturamento>(sql.ToString());
                        if (kpi != null)
                        {
                            //Vendas Hoje
                            sql.Clear();
                            sql.AppendLine("select isnull(sum(isnull(a.Vl_SubTotal, 0) - isnull(a.Vl_Desconto, 0) + isnull(a.Vl_Acrescimo, 0) + isnull(a.vl_juro_fin, 0) + isnull(a.vl_frete, 0)), 0) as Valor")
                                .AppendLine("from VTB_FAT_VENDAS_VR_NF a")
                                .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                                .AppendLine("on a.cd_empresa = b.CD_Empresa")
                                .AppendLine("and b.Login = '" + login.Trim() + "'")
                                .AppendLine("where convert(datetime, floor(convert(decimal(30,10), a.dt_emissao))) = ")
                                .AppendLine("convert(datetime, floor(convert(decimal(30,10), getdate())))");
                            kpi.VendasHoje = await conexao._conexao.ExecuteScalarAsync<decimal>(sql.ToString());
                            //Vendas Ontem
                            sql.Clear();
                            sql.AppendLine("select isnull(sum(isnull(a.Vl_SubTotal, 0) - isnull(a.Vl_Desconto, 0) + isnull(a.Vl_Acrescimo, 0) + isnull(a.vl_juro_fin, 0) + isnull(a.vl_frete, 0)), 0) as Valor")
                                .AppendLine("from VTB_FAT_VENDAS_VR_NF a")
                                .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                                .AppendLine("on a.cd_empresa = b.CD_Empresa")
                                .AppendLine("and b.Login = '" + login.Trim() + "'")
                                .AppendLine("where convert(datetime, floor(convert(decimal(30,10), a.dt_emissao))) = ")
                                .AppendLine("convert(datetime, floor(convert(decimal(30,10), dateadd(day, -1, getdate()))))");
                            kpi.VendasOntem = await conexao._conexao.ExecuteScalarAsync<decimal>(sql.ToString());
                            //Acumulado Semana
                            sql.Clear();
                            sql.AppendLine("select isnull(sum(isnull(a.Vl_SubTotal, 0) - isnull(a.Vl_Desconto, 0) + isnull(a.Vl_Acrescimo, 0) + isnull(a.vl_juro_fin, 0) + isnull(a.vl_frete, 0)), 0) as Valor")
                                .AppendLine("from VTB_FAT_VENDAS_VR_NF a")
                                .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                                .AppendLine("on a.cd_empresa = b.CD_Empresa")
                                .AppendLine("and b.Login = '" + login.Trim() + "'")
                                .AppendLine("where DATEPART(WEEK, a.dt_emissao) = DATEPART(WEEK, getdate())")
                                .AppendLine("and YEAR(a.dt_emissao) = YEAR(GETDATE())");
                            kpi.AcumuladoSemana = await conexao._conexao.ExecuteScalarAsync<decimal>(sql.ToString());
                        }
                        return kpi;
                    }
                    else return null;
                }
            }
            catch { return null; }
        }

        public async Task<IEnumerable<Faturamento>> GetVendasPorAnoAsync(string token, string login)
        {
            try
            {
                KPIFaturamento kpi = new KPIFaturamento();
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        StringBuilder sql = new StringBuilder();
                        //Detalhes Mes
                        sql.AppendLine("select YEAR(a.dt_emissao) as Ano, isnull(sum(isnull(a.Vl_SubTotal, 0) - isnull(a.Vl_Desconto, 0) + isnull(a.Vl_Acrescimo, 0) + isnull(a.vl_juro_fin, 0) + isnull(a.vl_frete, 0)), 0) as Vl_venda")
                            .AppendLine("from VTB_FAT_VENDAS_VR_NF a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("and b.Login = '" + login.Trim() + "'")
                            .AppendLine("group by YEAR(a.dt_emissao)")
                            .AppendLine("order by YEAR(a.dt_emissao)");
                        return await conexao._conexao.QueryAsync<Faturamento>(sql.ToString());
                    }
                    else return null;
                }
            }
            catch { return null; }
        }

        public async Task<IEnumerable<Faturamento>> GetVendasPorMesAsync(string token, string login, int ano)
        {
            try
            {
                KPIFaturamento kpi = new KPIFaturamento();
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        StringBuilder sql = new StringBuilder();
                        //Detalhes Mes
                        sql.AppendLine("select MONTH(a.dt_emissao) as Mes, YEAR(a.dt_emissao) as Ano, isnull(sum(isnull(a.Vl_SubTotal, 0) - isnull(a.Vl_Desconto, 0) + isnull(a.Vl_Acrescimo, 0) + isnull(a.vl_juro_fin, 0) + isnull(a.vl_frete, 0)), 0) as Vl_venda")
                            .AppendLine("from VTB_FAT_VENDAS_VR_NF a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("and b.Login = '" + login.Trim() + "'")
                            .AppendLine("where YEAR(a.dt_emissao) = " + ano)
                            .AppendLine("group by MONTH(a.dt_emissao), YEAR(a.dt_emissao)")
                            .AppendLine("order by MONTH(a.dt_emissao)");
                        return await conexao._conexao.QueryAsync<Faturamento>(sql.ToString());
                    }
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<FaturamentoPortador>> GetVendasPortadorAsync(string token, string login, int mes, int ano)
        {
            try
            {
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        StringBuilder sql = new StringBuilder();
                        //Detalhes Mes
                        sql.AppendLine("select case when ISNULL(a.tp_cartao, '') <> '' then case when a.tp_cartao = 'D' then 'DEBITO' else 'CREDITO' end else a.ds_portador end as Portador , isnull(sum(isnull(a.valor, 0)), 0) as Valor")
                            .AppendLine("from VTB_FAT_VENDAS_PORTADOR a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("and b.Login = '" + login.Trim() + "'")
                            .AppendLine("where YEAR(a.dt_emissao) = " + ano)
                            .AppendLine("and MONTH(a.dt_emissao) = " + mes)
                            .AppendLine("group by a.ds_portador, a.tp_cartao");
                        return await conexao._conexao.QueryAsync<FaturamentoPortador>(sql.ToString());
                    }
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<Faturamento>> GetVendasCidadeAsync(string token, string login, int mes, int ano)
        {
            try
            {
                KPIFaturamento kpi = new KPIFaturamento();
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        StringBuilder sql = new StringBuilder();
                        //Detalhes Mes
                        sql.AppendLine("select c.DS_Cidade,")
                            .AppendLine("isnull(sum(isnull(a.Vl_SubTotal, 0) - isnull(a.Vl_Desconto, 0) + isnull(a.Vl_Acrescimo, 0) + isnull(a.vl_juro_fin, 0) + isnull(a.vl_frete, 0)), 0) as Vl_venda")
                            .AppendLine("from VTB_FAT_VENDAS_VR_NF a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("inner join VTB_FIN_ENDERECO c")
                            .AppendLine("on a.cd_clifor = c.CD_Clifor")
                            .AppendLine("and a.cd_endereco = c.CD_Endereco")
                            .AppendLine("and b.Login = '" + login.Trim() + "'")
                            .AppendLine("where MONTH(a.dt_emissao) = " + mes)
                            .AppendLine("and YEAR(a.dt_emissao) = " + ano)
                            .AppendLine("group by c.DS_Cidade");
                        return await conexao._conexao.QueryAsync<Faturamento>(sql.ToString());
                    }
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<Faturamento>> GetVendasGrupoAsync(string token, string login, int mes, int ano)
        {
            try
            {
                KPIFaturamento kpi = new KPIFaturamento();
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        StringBuilder sql = new StringBuilder();
                        //Detalhes Mes
                        sql.AppendLine("select LTRIM(RTRIM(d.DS_Grupo)) as DS_Grupo,")
                            .AppendLine("isnull(sum(isnull(a.Vl_SubTotal, 0) - isnull(a.Vl_Desconto, 0) + isnull(a.Vl_Acrescimo, 0) + isnull(a.vl_juro_fin, 0) + isnull(a.vl_frete, 0)), 0) as Vl_venda")
                            .AppendLine("from VTB_FAT_VENDAS_VR_NF a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("and b.Login = '" + login.Trim() + "'")
                            .AppendLine("inner join TB_EST_Produto c")
                            .AppendLine("on a.CD_Produto = c.CD_Produto")
                            .AppendLine("inner join TB_EST_GrupoProduto d")
                            .AppendLine("on c.CD_Grupo = d.CD_Grupo")
                            .AppendLine("where MONTH(a.dt_emissao) = " + mes)
                            .AppendLine("and YEAR(a.dt_emissao) = " + ano)
                            .AppendLine("group by d.DS_Grupo");
                        return await conexao._conexao.QueryAsync<Faturamento>(sql.ToString());
                    }
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<Faturamento>> GetVendasVendedoresAsync(string token, string login, int mes, int ano)
        {
            try
            {
                KPIFaturamento kpi = new KPIFaturamento();
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        StringBuilder sql = new StringBuilder();
                        //Detalhes Mes
                        sql.AppendLine("select c.NM_Clifor as Vendedor,")
                            .AppendLine("isnull(sum(isnull(a.Vl_SubTotal, 0) - isnull(a.Vl_Desconto, 0) + isnull(a.Vl_Acrescimo, 0) + isnull(a.vl_juro_fin, 0) + isnull(a.vl_frete, 0)), 0) as Vl_venda")
                            .AppendLine("from VTB_FAT_VENDAS_VR_NF a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("and b.Login = '" + login.Trim() + "'")
                            .AppendLine("inner join TB_FIN_Clifor c")
                            .AppendLine("on a.cd_vendedor = c.CD_Clifor")
                            .AppendLine("where MONTH(a.dt_emissao) = " + mes)
                            .AppendLine("and YEAR(a.dt_emissao) = " + ano)
                            .AppendLine("group by c.NM_Clifor");
                        return await conexao._conexao.QueryAsync<Faturamento>(sql.ToString());
                    }
                    else return null;
                }
            }
            catch { return null; }
        }
    }
}
