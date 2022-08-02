using Dapper;
using LB_Gestor_API.DataBase;
using LB_Gestor_API.Models;
using LB_Gestor_API.Repository.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.DAO
{
    public class ContaGerDAO : IContaGer
    {
        readonly IConfiguration _config;

        public ContaGerDAO(IConfiguration config) { _config = config; }

        public async Task<IEnumerable<ContaGer>> GetAsync(string token, string login)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.cd_contager, a.ds_contager,")
                    .AppendLine("Vl_saldo = ISNULL((select SUM(ISNULL(x.Vl_RECEBER, 0) - ISNULL(x.Vl_PAGAR, 0))")
                    .AppendLine("					from tb_fin_caixa x")
                    .AppendLine("					where x.CD_ContaGer = a.CD_ContaGer")
                    .AppendLine("					and ISNULL(x.ST_Estorno, 'N') <> 'S'), 0)")
                    .AppendLine("from TB_FIN_ContaGer a")
                    .AppendLine("inner join TB_DIV_Usuario_X_ContaGer b")
                    .AppendLine("on a.CD_ContaGer = b.CD_ContaGer")
                    .AppendLine("where ISNULL(a.ST_Registro, 'A') <> 'C'")
                    .AppendLine("and ISNULL(a.ST_ContaCompensacao, 'N') <> 'S'")
                    .AppendLine("and a.ST_ContaCartao = 1")
                    .AppendLine("and a.ST_ContaCF = 1")
                    .AppendLine("and not exists(select 1 from TB_FIN_ConfigADTO x")
                    .AppendLine("				where x.CD_ContaGerDEV_CV = a.CD_ContaGer)")
                    .AppendLine("and b.Login = '" + login.Trim() + "'");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<ContaGer>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }

        public async Task<ResumoPagarReceber> GetResumoPagarReceber(string token, string login)
        {
            ResumoPagarReceber ret = new ResumoPagarReceber();
            try
            {
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        //RECEBER MAIS DE 30 DIAS VENCIDAS
                        StringBuilder sql = new StringBuilder();
                        sql.AppendLine("select ISNULL(SUM(isnull(a.vl_receber, 0)), 0)")
                            .AppendLine("from VTB_FIN_FLUXOCAIXA a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("where convert(datetime, floor(convert(decimal(30,10), a.dt_vencto))) < ")
                            .AppendLine("convert(datetime, floor(convert(decimal(30,10), DATEADD(day, -30, getdate()))))")
                            .AppendLine("and b.Login = '" + login.Trim() + "'");
                        ret.Vl_rec_mais30dias_venc = await conexao._conexao.ExecuteScalarAsync<decimal>(sql.ToString());
                        //RECEBER 30 DIAS VENCIDAS
                        sql.Clear();
                        sql.AppendLine("select ISNULL(SUM(isnull(a.vl_receber, 0)), 0)")
                            .AppendLine("from VTB_FIN_FLUXOCAIXA a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("where convert(datetime, floor(convert(decimal(30,10), a.dt_vencto))) >= ")
                            .AppendLine("convert(datetime, floor(convert(decimal(30,10), DATEADD(day, -30, getdate()))))")
                            .AppendLine("and convert(datetime, floor(convert(decimal(30,10), a.dt_vencto))) < ")
                            .AppendLine("convert(datetime, floor(convert(decimal(30,10), getdate())))")
                            .AppendLine("and b.Login = '" + login.Trim() + "'");
                        ret.Vl_rec_30dias_venc = await conexao._conexao.ExecuteScalarAsync<decimal>(sql.ToString());
                        //RECEBER HOJE
                        sql.Clear();
                        sql.AppendLine("select ISNULL(SUM(isnull(a.vl_receber, 0)), 0)")
                            .AppendLine("from VTB_FIN_FLUXOCAIXA a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("where convert(datetime, floor(convert(decimal(30,10), a.dt_vencto))) =")
                            .AppendLine("convert(datetime, floor(convert(decimal(30,10), getdate())))")
                            .AppendLine("and b.Login = '" + login.Trim() + "'");
                        ret.Vl_rec_hoje = await conexao._conexao.ExecuteScalarAsync<decimal>(sql.ToString());
                        //RECEBER 30 DIAS
                        sql.Clear();
                        sql.AppendLine("select ISNULL(SUM(isnull(a.vl_receber, 0)), 0)")
                            .AppendLine("from VTB_FIN_FLUXOCAIXA a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("where convert(datetime, floor(convert(decimal(30,10), a.dt_vencto))) <= ")
                            .AppendLine("convert(datetime, floor(convert(decimal(30,10), DATEADD(day, 30, getdate()))))")
                            .AppendLine("and convert(datetime, floor(convert(decimal(30,10), a.dt_vencto))) > ")
                            .AppendLine("convert(datetime, floor(convert(decimal(30,10), getdate())))")
                            .AppendLine("and b.Login = '" + login.Trim() + "'");
                        ret.Vl_rec_30dias = await conexao._conexao.ExecuteScalarAsync<decimal>(sql.ToString());
                        //RECEBER MAIS DE 30 DIAS
                        sql.Clear();
                        sql.AppendLine("select ISNULL(SUM(isnull(a.vl_receber, 0)), 0)")
                            .AppendLine("from VTB_FIN_FLUXOCAIXA a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("where convert(datetime, floor(convert(decimal(30,10), a.dt_vencto))) > ")
                            .AppendLine("convert(datetime, floor(convert(decimal(30,10), DATEADD(day, 30, getdate()))))")
                            .AppendLine("and b.Login = '" + login.Trim() + "'");
                        ret.Vl_rec_mais30dias = await conexao._conexao.ExecuteScalarAsync<decimal>(sql.ToString());

                        //PAGAR MAIS DE 30 DIAS VENCIDAS
                        sql.Clear();
                        sql.AppendLine("select ISNULL(SUM(isnull(a.vl_pagar, 0)), 0)")
                            .AppendLine("from VTB_FIN_FLUXOCAIXA a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("where convert(datetime, floor(convert(decimal(30,10), a.dt_vencto))) < ")
                            .AppendLine("convert(datetime, floor(convert(decimal(30,10), DATEADD(day, -30, getdate()))))")
                            .AppendLine("and b.Login = '" + login.Trim() + "'");
                        ret.Vl_pag_mais30dias_venc = await conexao._conexao.ExecuteScalarAsync<decimal>(sql.ToString());
                        //PAGAR 30 DIAS VENCIDAS
                        sql.Clear();
                        sql.AppendLine("select ISNULL(SUM(isnull(a.vl_pagar, 0)), 0)")
                            .AppendLine("from VTB_FIN_FLUXOCAIXA a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("where convert(datetime, floor(convert(decimal(30,10), a.dt_vencto))) >= ")
                            .AppendLine("convert(datetime, floor(convert(decimal(30,10), DATEADD(day, -30, getdate()))))")
                            .AppendLine("and convert(datetime, floor(convert(decimal(30,10), a.dt_vencto))) < ")
                            .AppendLine("convert(datetime, floor(convert(decimal(30,10), getdate())))")
                            .AppendLine("and b.Login = '" + login.Trim() + "'");
                        ret.Vl_pag_30dias_venc = await conexao._conexao.ExecuteScalarAsync<decimal>(sql.ToString());
                        //PAGAR HOJE
                        sql.Clear();
                        sql.AppendLine("select ISNULL(SUM(isnull(a.vl_pagar, 0)), 0)")
                            .AppendLine("from VTB_FIN_FLUXOCAIXA a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("where convert(datetime, floor(convert(decimal(30,10), a.dt_vencto))) =")
                            .AppendLine("convert(datetime, floor(convert(decimal(30,10), getdate())))")
                            .AppendLine("and b.Login = '" + login.Trim() + "'");
                        ret.Vl_pag_hoje = await conexao._conexao.ExecuteScalarAsync<decimal>(sql.ToString());
                        //PAGAR 30 DIAS
                        sql.Clear();
                        sql.AppendLine("select ISNULL(SUM(isnull(a.vl_pagar, 0)), 0)")
                            .AppendLine("from VTB_FIN_FLUXOCAIXA a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("where convert(datetime, floor(convert(decimal(30,10), a.dt_vencto))) <= ")
                            .AppendLine("convert(datetime, floor(convert(decimal(30,10), DATEADD(day, 30, getdate()))))")
                            .AppendLine("and convert(datetime, floor(convert(decimal(30,10), a.dt_vencto))) > ")
                            .AppendLine("convert(datetime, floor(convert(decimal(30,10), getdate())))")
                            .AppendLine("and b.Login = '" + login.Trim() + "'");
                        ret.Vl_pag_30dias = await conexao._conexao.ExecuteScalarAsync<decimal>(sql.ToString());
                        //PAGAR MAIS DE 30 DIAS
                        sql.Clear();
                        sql.AppendLine("select ISNULL(SUM(isnull(a.vl_pagar, 0)), 0)")
                            .AppendLine("from VTB_FIN_FLUXOCAIXA a")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa b")
                            .AppendLine("on a.cd_empresa = b.CD_Empresa")
                            .AppendLine("where convert(datetime, floor(convert(decimal(30,10), a.dt_vencto))) > ")
                            .AppendLine("convert(datetime, floor(convert(decimal(30,10), DATEADD(day, 30, getdate()))))")
                            .AppendLine("and b.Login = '" + login.Trim() + "'");
                        ret.Vl_pag_mais30dias = await conexao._conexao.ExecuteScalarAsync<decimal>(sql.ToString());
                    }
                }
            }
            catch { }
            return ret;
        }
        
        public async Task<IEnumerable<ReceitaDespesa>> GetReceitasDespesasAsync(string token, string login, int mes, int ano, string tp_mov)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select tp_mov, ds_historico, sum(valor) as Valor")
                .AppendLine("from")
                .AppendLine("(")
                .AppendLine("select a.TP_MOV, c.DS_Historico, SUM(ISNULL(a.Vl_Parcela, 0)) as Valor ")
                .AppendLine("from VTB_FIN_PARCELA a ")
                .AppendLine("inner join TB_FIN_Duplicata b ")
                .AppendLine("on a.CD_Empresa = b.CD_Empresa ")
                .AppendLine("and a.Nr_Lancto = b.Nr_Lancto ")
                .AppendLine("and ISNULL(b.ST_Registro, 'A') <> 'C' ")
                .AppendLine("and a.status_parcela <> 'DESCONTADO' ")
                .AppendLine("and MONTH(a.DT_Vencto) = " + mes.ToString())
                .AppendLine("and YEAR(a.DT_Vencto) = " + ano.ToString())
                .AppendLine("inner join TB_FIN_Historico c ")
                .AppendLine("on a.CD_Historico = c.CD_Historico ")
                .AppendLine("inner join TB_DIV_Usuario_X_Empresa d")
                .AppendLine("on a.cd_empresa = d.CD_Empresa")
                .AppendLine("and d.login = '" + login.Trim() + "'");
                if (!string.IsNullOrWhiteSpace(tp_mov))
                    sql.AppendLine("where a.tp_mov = '" + tp_mov.Trim() + "'");
                sql.AppendLine("group by a.TP_MOV, c.DS_Historico ")
                .AppendLine("union all")
                .AppendLine("select c.TP_MOV, c.DS_Historico, SUM(isnull(abs(a.vl_pagar - a.vl_receber), 0))")
                .AppendLine("from TB_FIN_Caixa a")
                .AppendLine("inner join TB_FIN_ContaGer b")
                .AppendLine("on a.CD_ContaGer = b.CD_ContaGer")
                .AppendLine("and ISNULL(b.ST_ContaCompensacao, 'N') <> 'S'")
                .AppendLine("and ISNULL(b.ST_ContaCartao, 1) <> 0")
                .AppendLine("and ISNULL(b.ST_ContaCF, 1) <> 0")
                .AppendLine("and ISNULL(b.ST_ContaAplicacao, 'N') <> 'S'")
                .AppendLine("inner join TB_FIN_Historico c")
                .AppendLine("on a.CD_Historico = c.CD_Historico")
                .AppendLine("inner join TB_DIV_Usuario_X_Empresa d")
                .AppendLine("on a.cd_empresa = d.CD_Empresa")
                .AppendLine("and d.login = '" + login.Trim() + "'")
                .AppendLine("where MONTH(a.DT_Lancto) = " + mes.ToString())
                .AppendLine("and YEAR(a.DT_Lancto) = " + ano.ToString())
                .AppendLine("and isnull(a.ST_Estorno, 'N') <> 'S'")
                .AppendLine("and ISNULL(a.ST_Avulso, 'N') = 'S'");
                if (!string.IsNullOrWhiteSpace(tp_mov))
                    sql.AppendLine("and c.tp_mov = '" + tp_mov.Trim() + "'");
                sql.AppendLine("group by c.TP_MOV, c.DS_Historico")
                .AppendLine(")x")
                .AppendLine("group by x.TP_MOV, x.DS_Historico");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<ReceitaDespesa>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }

        public async Task<IEnumerable<ReceitaDespesa>> GetResultadoAnoAsync(string token, string login, int ano)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select mes, ano, sum(valor) as Valor")
                    .AppendLine("from")
                    .AppendLine("(")
                    .AppendLine("select MONTH(a.DT_Vencto) as Mes, YEAR(a.DT_Vencto) as Ano, SUM(case when a.tp_mov = 'P' then (-1) * ISNULL(a.Vl_Parcela, 0) else ISNULL(a.Vl_Parcela, 0) end) as Valor ")
                    .AppendLine("from VTB_FIN_PARCELA a ")
                    .AppendLine("inner join TB_FIN_Duplicata b ")
                    .AppendLine("on a.CD_Empresa = b.CD_Empresa ")
                    .AppendLine("and a.Nr_Lancto = b.Nr_Lancto ")
                    .AppendLine("and ISNULL(b.ST_Registro, 'A') <> 'C' ")
                    .AppendLine("and a.status_parcela <> 'DESCONTADO' ")
                    .AppendLine("and YEAR(a.DT_Vencto) = " + ano)
                    .AppendLine("inner join TB_DIV_Usuario_X_Empresa d")
                    .AppendLine("on a.cd_empresa = d.CD_Empresa")
                    .AppendLine("and d.login = '" + login.Trim() + "'")
                    .AppendLine("group by month(a.dt_vencto), year(a.dt_vencto)")
                    .AppendLine("union all")
                    .AppendLine("select month(a.dt_lancto) as Mes, year(a.dt_lancto) as Ano, SUM(case when isnull(a.vl_pagar, 0) > 0 then (-1) * isnull(a.vl_pagar, 0) else isnull(a.vl_receber, 0) end)")
                    .AppendLine("from TB_FIN_Caixa a")
                    .AppendLine("inner join TB_FIN_ContaGer b")
                    .AppendLine("on a.CD_ContaGer = b.CD_ContaGer")
                    .AppendLine("and ISNULL(b.ST_ContaCompensacao, 'N') <> 'S'")
                    .AppendLine("and ISNULL(b.ST_ContaCartao, 1) <> 0")
                    .AppendLine("and ISNULL(b.ST_ContaCF, 1) <> 0")
                    .AppendLine("and ISNULL(b.ST_ContaAplicacao, 'N') <> 'S'")
                    .AppendLine("inner join TB_DIV_Usuario_X_Empresa d")
                    .AppendLine("on a.cd_empresa = d.CD_Empresa")
                    .AppendLine("and d.login = '" + login.Trim() + "'")
                    .AppendLine("where YEAR(a.DT_Lancto) = " + ano)
                    .AppendLine("and isnull(a.ST_Estorno, 'N') <> 'S'")
                    .AppendLine("and ISNULL(a.ST_Avulso, 'N') = 'S'")
                    .AppendLine("group by month(a.dt_lancto), year(a.dt_lancto)")
                    .AppendLine(")x")
                    .AppendLine("group by x.Mes, x.Ano")
                    .AppendLine("order by x.Ano, x.Mes");
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<ReceitaDespesa>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }

        public async Task<IEnumerable<FluxoCaixa>> GetFluxoCaixaAsync(string token, string login, DateTime dt_final)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("with SaldoAnterior(dia, saldoAntCC)")
                    .AppendLine("as")
                    .AppendLine("(")
                    .AppendLine("select convert(datetime, floor(convert(decimal(30,10), getdate()))) as Dia, ")
                    .AppendLine("Saldo = ISNULL(SUM(ISNULL(a.vl_receber, 0) - ISNULL(a.vl_pagar, 0)), 0)")
                    .AppendLine("from TB_FIN_Caixa a")
                    .AppendLine("inner join TB_FIN_ContaGer b")
                    .AppendLine("on a.CD_ContaGer = b.CD_ContaGer")
                    .AppendLine("and ISNULL(a.ST_Estorno, 'N') <> 'S'")
                    .AppendLine("and ISNULL(b.ST_Registro, 'A') <> 'S'")
                    .AppendLine("and ISNULL(b.ST_ContaCompensacao, 'N') <> 'S'")
                    .AppendLine("and ISNULL(b.IgnorarFluxoCaixa, 0) <> 1")
                    .AppendLine("and b.ST_ContaCartao = 1")
                    .AppendLine("and b.ST_ContaCF = 1")
                    .AppendLine("inner join TB_DIV_Usuario_X_Empresa c")
                    .AppendLine("on a.cd_empresa = c.CD_Empresa")
                    .AppendLine("and c.login = '" + login.Trim() + "'")
                    .AppendLine("where convert(datetime, floor(convert(decimal(30,10), a.DT_Lancto))) <= convert(datetime, floor(convert(decimal(30,10), getdate())))")
                    .AppendLine(")")
                    .AppendLine("SELECT t1.cd_empresa, e.NM_Empresa, t1.dt_vencto, sd.saldoAntCC,")
                    .AppendLine("t1.vl_receber, t1.vl_pagar, SUM(T2.vl_receber - t2.vl_pagar) as Acumulado ")
                    .AppendLine("FROM VTB_FIN_FLUXOCAIXA AS t1 ")
                    .AppendLine("INNER JOIN VTB_FIN_FLUXOCAIXA AS t2 ")
                    .AppendLine("on t1.dt_vencto >= t2.dt_vencto ")
                    .AppendLine("inner join TB_DIV_Empresa e ")
                    .AppendLine("on t1.cd_empresa = e.CD_Empresa")
                    .AppendLine("inner join TB_DIV_Usuario_X_Empresa f")
                    .AppendLine("on t1.cd_empresa = f.CD_Empresa")
                    .AppendLine("and f.login = '" + login.Trim() + "'")
                    .AppendLine("inner join TB_DIV_Usuario_X_Empresa g")
                    .AppendLine("on t2.cd_empresa = g.CD_Empresa")
                    .AppendLine("and g.login = '" + login.Trim() + "'")
                    .AppendLine(", SaldoAnterior sd")
                    .AppendLine("where convert(datetime, floor(convert(decimal(30,10), t1.dt_vencto))) >= convert(datetime, floor(convert(decimal(30,10), getdate())))")
                    .AppendLine("and convert(datetime, floor(convert(decimal(30, 10), t2.dt_vencto))) >= convert(datetime, floor(convert(decimal(30, 10), getdate())))")
                    .AppendLine("and convert(datetime, floor(convert(decimal(30,10), t1.dt_vencto))) <= '" + dt_final.ToString("yyyyMMdd") + "'")
                    .AppendLine("group by t1.cd_empresa, e.NM_Empresa, t1.dt_vencto, sd.saldoAntCC, t1.vl_receber, t1.vl_pagar ")
                    .AppendLine("order by t1.dt_vencto");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<FluxoCaixa>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }

        public async Task<IEnumerable<DRE>> GetDREAsync(string token, string login, int ano)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.ID_ContaDRE, a.Classificacao, SPACE((a.nivel-1)*5)+a.DS_ContaDRE as DS_ContaDRE,")
                    .AppendLine("a.Tp_Conta, a.Operador, a.Deducao, a.Nivel, ")
                    .AppendLine("Vl_Janeiro = ABS(ISNULL((select ISNULL(SUM(ISNULL(x.Vl_Lancto * case when z.deducao = 1 then -1 else 1 end, 0)), 0)")
                    .AppendLine("				from TB_FIN_LanctoPlanoContas x")
                    .AppendLine("				inner join TB_FIN_PlanoContas y")
                    .AppendLine("				on x.ID_Plano = y.ID_Plano")
                    .AppendLine("				inner join TB_FIN_DRE z")
                    .AppendLine("				on y.ID_ContaDRE = z.ID_ContaDRE")
                    .AppendLine("               inner join TB_DIV_Usuario_X_Empresa w")
                    .AppendLine("               on x.cd_empresa = w.cd_empresa")
                    .AppendLine("				where YEAR(x.DT_Lancto) = " + ano + " ")
                    .AppendLine("               and w.login = '" + login.Trim() + "'")
                    .AppendLine("				and MONTH(x.DT_Lancto) = 1")
                    .AppendLine("				and z.Classificacao like a.classificacao + '%'), 0))");
                if (DateTime.Now.Month >= 2)
                    sql.AppendLine(",Vl_Fevereiro = ABS(ISNULL((select ISNULL(SUM(ISNULL(x.Vl_Lancto * case when z.deducao = 1 then -1 else 1 end, 0)), 0)")
                    .AppendLine("				from TB_FIN_LanctoPlanoContas x")
                    .AppendLine("				inner join TB_FIN_PlanoContas y")
                    .AppendLine("				on x.ID_Plano = y.ID_Plano")
                    .AppendLine("				inner join TB_FIN_DRE z")
                    .AppendLine("				on y.ID_ContaDRE = z.ID_ContaDRE")
                    .AppendLine("               inner join TB_DIV_Usuario_X_Empresa w")
                    .AppendLine("               on x.cd_empresa = w.cd_empresa")
                    .AppendLine("				where YEAR(x.DT_Lancto) = " + ano + " ")
                    .AppendLine("               and w.login = '" + login.Trim() + "'")
                    .AppendLine("				and MONTH(x.DT_Lancto) = 2")
                    .AppendLine("				and z.Classificacao like a.classificacao + '%'), 0))");
                if (DateTime.Now.Month >= 3)
                    sql.AppendLine(",Vl_Marco = ABS(ISNULL((select ISNULL(SUM(ISNULL(x.Vl_Lancto * case when z.deducao = 1 then -1 else 1 end, 0)), 0)")
                    .AppendLine("				from TB_FIN_LanctoPlanoContas x")
                    .AppendLine("				inner join TB_FIN_PlanoContas y")
                    .AppendLine("				on x.ID_Plano = y.ID_Plano")
                    .AppendLine("				inner join TB_FIN_DRE z")
                    .AppendLine("				on y.ID_ContaDRE = z.ID_ContaDRE")
                    .AppendLine("               inner join TB_DIV_Usuario_X_Empresa w")
                    .AppendLine("               on x.cd_empresa = w.cd_empresa")
                    .AppendLine("				where YEAR(x.DT_Lancto) = " + ano + " ")
                    .AppendLine("               and w.login = '" + login.Trim() + "'")
                    .AppendLine("				and MONTH(x.DT_Lancto) = 3")
                    .AppendLine("				and z.Classificacao like a.classificacao + '%'), 0))");
                if (DateTime.Now.Month >= 4)
                    sql.AppendLine(",Vl_Abril = ABS(ISNULL((select ISNULL(SUM(ISNULL(x.Vl_Lancto * case when z.deducao = 1 then -1 else 1 end, 0)), 0)")
                    .AppendLine("				from TB_FIN_LanctoPlanoContas x")
                    .AppendLine("				inner join TB_FIN_PlanoContas y")
                    .AppendLine("				on x.ID_Plano = y.ID_Plano")
                    .AppendLine("				inner join TB_FIN_DRE z")
                    .AppendLine("				on y.ID_ContaDRE = z.ID_ContaDRE")
                    .AppendLine("               inner join TB_DIV_Usuario_X_Empresa w")
                    .AppendLine("               on x.cd_empresa = w.cd_empresa")
                    .AppendLine("				where YEAR(x.DT_Lancto) = " + ano + " ")
                    .AppendLine("               and w.login = '" + login.Trim() + "'")
                    .AppendLine("				and MONTH(x.DT_Lancto) = 4")
                    .AppendLine("				and z.Classificacao like a.classificacao + '%'), 0))");
                if (DateTime.Now.Month >= 5)
                    sql.AppendLine(",Vl_Maio = ABS(ISNULL((select ISNULL(SUM(ISNULL(x.Vl_Lancto * case when z.deducao = 1 then -1 else 1 end, 0)), 0)")
                    .AppendLine("				from TB_FIN_LanctoPlanoContas x")
                    .AppendLine("				inner join TB_FIN_PlanoContas y")
                    .AppendLine("				on x.ID_Plano = y.ID_Plano")
                    .AppendLine("				inner join TB_FIN_DRE z")
                    .AppendLine("				on y.ID_ContaDRE = z.ID_ContaDRE")
                    .AppendLine("               inner join TB_DIV_Usuario_X_Empresa w")
                    .AppendLine("               on x.cd_empresa = w.cd_empresa")
                    .AppendLine("				where YEAR(x.DT_Lancto) = " + ano + " ")
                    .AppendLine("               and w.login = '" + login.Trim() + "'")
                    .AppendLine("				and MONTH(x.DT_Lancto) = 5")
                    .AppendLine("				and z.Classificacao like a.classificacao + '%'), 0))");
                if (DateTime.Now.Month >= 6)
                    sql.AppendLine(",Vl_Junho = ABS(ISNULL((select ISNULL(SUM(ISNULL(x.Vl_Lancto * case when z.deducao = 1 then -1 else 1 end, 0)), 0)")
                    .AppendLine("				from TB_FIN_LanctoPlanoContas x")
                    .AppendLine("				inner join TB_FIN_PlanoContas y")
                    .AppendLine("				on x.ID_Plano = y.ID_Plano")
                    .AppendLine("				inner join TB_FIN_DRE z")
                    .AppendLine("				on y.ID_ContaDRE = z.ID_ContaDRE")
                    .AppendLine("               inner join TB_DIV_Usuario_X_Empresa w")
                    .AppendLine("               on x.cd_empresa = w.cd_empresa")
                    .AppendLine("				where YEAR(x.DT_Lancto) = " + ano + " ")
                    .AppendLine("               and w.login = '" + login.Trim() + "'")
                    .AppendLine("				and MONTH(x.DT_Lancto) = 6")
                    .AppendLine("				and z.Classificacao like a.classificacao + '%'), 0))");
                if (DateTime.Now.Month >= 7)
                    sql.AppendLine(",Vl_Julho = ABS(ISNULL((select ISNULL(SUM(ISNULL(x.Vl_Lancto * case when z.deducao = 1 then -1 else 1 end, 0)), 0)")
                    .AppendLine("				from TB_FIN_LanctoPlanoContas x")
                    .AppendLine("				inner join TB_FIN_PlanoContas y")
                    .AppendLine("				on x.ID_Plano = y.ID_Plano")
                    .AppendLine("				inner join TB_FIN_DRE z")
                    .AppendLine("				on y.ID_ContaDRE = z.ID_ContaDRE")
                    .AppendLine("               inner join TB_DIV_Usuario_X_Empresa w")
                    .AppendLine("               on x.cd_empresa = w.cd_empresa")
                    .AppendLine("				where YEAR(x.DT_Lancto) = " + ano + " ")
                    .AppendLine("               and w.login = '" + login.Trim() + "'")
                    .AppendLine("				and MONTH(x.DT_Lancto) = 7")
                    .AppendLine("				and z.Classificacao like a.classificacao + '%'), 0))");
                if (DateTime.Now.Month >= 8)
                    sql.AppendLine(",Vl_Agosto = ABS(ISNULL((select ISNULL(SUM(ISNULL(x.Vl_Lancto * case when z.deducao = 1 then -1 else 1 end, 0)), 0)")
                    .AppendLine("				from TB_FIN_LanctoPlanoContas x")
                    .AppendLine("				inner join TB_FIN_PlanoContas y")
                    .AppendLine("				on x.ID_Plano = y.ID_Plano")
                    .AppendLine("				inner join TB_FIN_DRE z")
                    .AppendLine("				on y.ID_ContaDRE = z.ID_ContaDRE")
                    .AppendLine("               inner join TB_DIV_Usuario_X_Empresa w")
                    .AppendLine("               on x.cd_empresa = w.cd_empresa")
                    .AppendLine("				where YEAR(x.DT_Lancto) = " + ano + " ")
                    .AppendLine("               and w.login = '" + login.Trim() + "'")
                    .AppendLine("				and MONTH(x.DT_Lancto) = 8")
                    .AppendLine("				and z.Classificacao like a.classificacao + '%'), 0))");
                if (DateTime.Now.Month >= 9)
                    sql.AppendLine(",Vl_Setembro = ABS(ISNULL((select ISNULL(SUM(ISNULL(x.Vl_Lancto * case when z.deducao = 1 then -1 else 1 end, 0)), 0)")
                    .AppendLine("				from TB_FIN_LanctoPlanoContas x")
                    .AppendLine("				inner join TB_FIN_PlanoContas y")
                    .AppendLine("				on x.ID_Plano = y.ID_Plano")
                    .AppendLine("				inner join TB_FIN_DRE z")
                    .AppendLine("				on y.ID_ContaDRE = z.ID_ContaDRE")
                    .AppendLine("               inner join TB_DIV_Usuario_X_Empresa w")
                    .AppendLine("               on x.cd_empresa = w.cd_empresa")
                    .AppendLine("				where YEAR(x.DT_Lancto) = " + ano + " ")
                    .AppendLine("               and w.login = '" + login.Trim() + "'")
                    .AppendLine("				and MONTH(x.DT_Lancto) = 9")
                    .AppendLine("				and z.Classificacao like a.classificacao + '%'), 0))");
                if (DateTime.Now.Month >= 10)
                    sql.AppendLine(",Vl_Outubro = ABS(ISNULL((select ISNULL(SUM(ISNULL(x.Vl_Lancto * case when z.deducao = 1 then -1 else 1 end, 0)), 0)")
                    .AppendLine("				from TB_FIN_LanctoPlanoContas x")
                    .AppendLine("				inner join TB_FIN_PlanoContas y")
                    .AppendLine("				on x.ID_Plano = y.ID_Plano")
                    .AppendLine("				inner join TB_FIN_DRE z")
                    .AppendLine("				on y.ID_ContaDRE = z.ID_ContaDRE")
                    .AppendLine("               inner join TB_DIV_Usuario_X_Empresa w")
                    .AppendLine("               on x.cd_empresa = w.cd_empresa")
                    .AppendLine("				where YEAR(x.DT_Lancto) = " + ano + " ")
                    .AppendLine("               and w.login = '" + login.Trim() + "'")
                    .AppendLine("				and MONTH(x.DT_Lancto) = 10")
                    .AppendLine("				and z.Classificacao like a.classificacao + '%'), 0))");
                if (DateTime.Now.Month >= 11)
                    sql.AppendLine(",Vl_Novembro = ABS(ISNULL((select ISNULL(SUM(ISNULL(x.Vl_Lancto * case when z.deducao = 1 then -1 else 1 end, 0)), 0)")
                    .AppendLine("				from TB_FIN_LanctoPlanoContas x")
                    .AppendLine("				inner join TB_FIN_PlanoContas y")
                    .AppendLine("				on x.ID_Plano = y.ID_Plano")
                    .AppendLine("				inner join TB_FIN_DRE z")
                    .AppendLine("				on y.ID_ContaDRE = z.ID_ContaDRE")
                    .AppendLine("               inner join TB_DIV_Usuario_X_Empresa w")
                    .AppendLine("               on x.cd_empresa = w.cd_empresa")
                    .AppendLine("				where YEAR(x.DT_Lancto) = " + ano + " ")
                    .AppendLine("               and w.login = '" + login.Trim() + "'")
                    .AppendLine("				and MONTH(x.DT_Lancto) = 11")
                    .AppendLine("				and z.Classificacao like a.classificacao + '%'), 0))");
                if(DateTime.Now.Month >= 12)
                    sql.AppendLine(",Vl_Dezembro = ABS(ISNULL((select ISNULL(SUM(ISNULL(x.Vl_Lancto * case when z.deducao = 1 then -1 else 1 end, 0)), 0)")
                    .AppendLine("				from TB_FIN_LanctoPlanoContas x")
                    .AppendLine("				inner join TB_FIN_PlanoContas y")
                    .AppendLine("				on x.ID_Plano = y.ID_Plano")
                    .AppendLine("				inner join TB_FIN_DRE z")
                    .AppendLine("				on y.ID_ContaDRE = z.ID_ContaDRE")
                    .AppendLine("               inner join TB_DIV_Usuario_X_Empresa w")
                    .AppendLine("               on x.cd_empresa = w.cd_empresa")
                    .AppendLine("				where YEAR(x.DT_Lancto) = " + ano + " ")
                    .AppendLine("               and w.login = '" + login.Trim() + "'")
                    .AppendLine("				and MONTH(x.DT_Lancto) = 12")
                    .AppendLine("				and z.Classificacao like a.classificacao + '%'), 0))")
                    .AppendLine("from TB_FIN_DRE a")
                    .AppendLine("order by a.Classificacao");
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<DRE>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }

        public async Task<IEnumerable<RecebimentoPortador>> GetRecebimentoPortadorAsync(string token, string login, int mes, int ano)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select case when isnull(g.TP_Cartao, '') <> '' then 'CARTAO ' + case when g.TP_Cartao = 'D' then 'DEBITO' else 'CREDITO' end else d.ds_portador end as Portador, ISNULL(sum(isnull(c.Vl_Liquidacao, 0) + ISNULL(c.Vl_JuroAcrescimo, 0) - isnull(c.Vl_DescontoBonus, 0)), 0) as Valor")
                    .AppendLine("from TB_FIN_Duplicata a")
                    .AppendLine("inner join TB_FIN_TPDuplicata b")
                    .AppendLine("on a.TP_Duplicata = b.TP_Duplicata")
                    .AppendLine("and ISNULL(a.ST_Registro, 'A') <> 'C'")
                    .AppendLine("and b.TP_MOV = 'R'")
                    .AppendLine("inner join VTB_FIN_LIQUIDACAO c")
                    .AppendLine("on a.CD_Empresa = c.CD_Empresa")
                    .AppendLine("and a.Nr_Lancto = c.Nr_Lancto")
                    .AppendLine("and ISNULL(c.ST_Registro, 'A') <> 'C'")
                    .AppendLine("inner join TB_FIN_Portador d")
                    .AppendLine("on c.CD_Portador = d.CD_Portador")
                    .AppendLine("left join TB_FIN_FaturaCartao_X_Caixa e")
                    .AppendLine("on e.CD_ContaGer = c.CD_ContaGer")
                    .AppendLine("and e.CD_LanctoCaixa = c.CD_LanctoCaixa")
                    .AppendLine("left join TB_FIN_FaturaCartao f")
                    .AppendLine("on e.ID_Fatura = f.ID_Fatura")
                    .AppendLine("left join TB_FIN_BandeiraCartao g")
                    .AppendLine("on f.ID_Bandeira = g.ID_Bandeira")
                    .AppendLine("inner join TB_DIV_Usuario_X_Empresa h")
                    .AppendLine("on a.CD_Empresa = h.CD_Empresa")
                    .AppendLine("and h.Login = '" + login.Trim() + "'")
                    .AppendLine("where MONTH(c.DT_Liquidacao) = " + mes)
                    .AppendLine("and YEAR(c.DT_Liquidacao) = " + ano)
                    .AppendLine("group by d.DS_Portador, g.TP_Cartao");
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<RecebimentoPortador>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
    }
}
