using Dapper;
using LB_Gestor_API.DataBase;
using LB_Gestor_API.Models;
using LB_Gestor_API.Repository.Interface;
using LB_Gestor_API.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.DAO
{
    public class DuplicataDAO: IDuplicata
    {
        readonly IConfiguration _config;

        public DuplicataDAO(IConfiguration config) { _config = config; }

        public async Task<IEnumerable<Duplicata>> GetAsync(string token, string cd_clifor, string tp_mov, decimal valor)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.CD_Empresa, a.Nr_Lancto, b.cd_parcela, a.Nr_Docto,")
                    .AppendLine("c.NM_Clifor, a.CD_Historico, d.DS_Historico, b.TP_MOV, a.DT_Emissao,")
                    .AppendLine("a.TP_Duplicata, b.DT_Vencto, b.Vl_Parcela, b.Vl_Atual, b.status_parcela")
                    .AppendLine("from TB_FIN_Duplicata a")
                    .AppendLine("inner join VTB_FIN_PARCELA b")
                    .AppendLine("on a.CD_Empresa = b.CD_Empresa")
                    .AppendLine("and a.Nr_Lancto = b.Nr_Lancto")
                    .AppendLine("and ISNULL(a.ST_Registro, 'A') <> 'C'")
                    .AppendLine("inner join TB_FIN_Clifor c")
                    .AppendLine("on a.CD_Clifor = c.CD_Clifor")
                    .AppendLine("inner join TB_FIN_Historico d")
                    .AppendLine("on a.CD_Historico = d.CD_Historico")
                    .AppendLine("inner join VTB_DIV_EMPRESA e")
                    .AppendLine("on a.CD_Empresa = e.CD_Empresa")
                    .AppendLine("where dbo.FVALIDA_NUMEROS(case when e.tp_pessoa = 'F' then e.NR_CPF else e.NR_CGC end) = '" + token.SoNumero() + "'")
                    .AppendLine("and ISNULL(b.ST_Registro, 'A') in('A', 'P')");
                if (!string.IsNullOrWhiteSpace(cd_clifor))
                    sql.AppendLine("and a.cd_clifor = '" + cd_clifor.Trim() + "'");
                if (!string.IsNullOrWhiteSpace(tp_mov))
                    sql.AppendLine("and b.tp_mov = '" + tp_mov.Trim() + "'");
                if (valor > decimal.Zero)
                    sql.AppendLine("and b.vl_atual = " + valor.ToString("N2", new System.Globalization.CultureInfo("en-US", true)));

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<Duplicata>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }

        public async Task<bool> GravarAsync(string token, Duplicata duplicata)
        {
            SqlTransaction t = null;
            try
            {
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        t = conexao._conexao.BeginTransaction(IsolationLevel.ReadCommitted);
                        //Buscar empresa
                        string cd_empresa = await conexao._conexao.ExecuteScalarAsync<string>("select cd_empresa from vtb_div_empresa " +
                                                                    "where dbo.FVALIDA_NUMEROS(case when tp_pessoa = 'F' then NR_CPF else NR_CGC end) = '" + token.SoNumero() + "'", transaction: t);
                        //Condição Pagamento
                        string cd_cond = await conexao._conexao.ExecuteScalarAsync<string>("select CD_CondPGTO " +
                                                                                           "from TB_FIN_CondPGTO " +
                                                                                           "where ISNULL(ST_Registro, 'A') <> 'C' " +
                                                                                           "and QT_Parcelas = 1", transaction: t);
                        //Tipo duplicata
                        string tp_dup = await conexao._conexao.ExecuteScalarAsync<string>("select TP_Duplicata " +
                                                                                           "from TB_FIN_TPDuplicata " +
                                                                                           "where ISNULL(ST_Registro, 'A') <> 'C' " +
                                                                                           "and TP_MOV = 'P'", transaction: t);
                        //Moeda
                        string cd_moeda = await conexao._conexao.ExecuteScalarAsync<string>("select Vl_String " +
                                                                                            "from TB_CFG_ParamGer " +
                                                                                            "where DS_Parametro = 'CD_MOEDA_PADRAO'", transaction: t);
                        DynamicParameters p = new DynamicParameters();
                        p.Add("@P_CD_EMPRESA", cd_empresa);
                        p.Add("@P_NR_LANCTO", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                        p.Add("@P_CD_HISTORICO", duplicata.Cd_historico);
                        p.Add("@P_NR_DOCTO", duplicata.Nr_docto);
                        p.Add("@P_CD_CLIFOR ", duplicata.Cd_clifor);
                        p.Add("@P_CD_ENDERECO", duplicata.Cd_endereco);
                        p.Add("@P_VL_DOCUMENTO", duplicata.Vl_parcela);
                        p.Add("@P_CD_CONDPGTO", cd_cond);
                        p.Add("@P_CD_JURO", null);
                        p.Add("@P_CD_MOEDA", cd_moeda);
                        p.Add("@P_TP_DUPLICATA", tp_dup);
                        p.Add("@P_DT_EMISSAO", duplicata.Dt_emissao);
                        p.Add("@P_COMPLHISTORICO", duplicata.CompHistorico);
                        p.Add("@P_ST_REGISTRO", "A");
                        p.Add("@P_DS_OBSERVACAO", null);
                        p.Add("@P_CD_AVALISTA", null);
                        p.Add("@P_CD_ENDAVALISTA", null);
                        p.Add("@P_LOGINCANC", null);
                        p.Add("@P_MOTIVOCANC", null);
                        p.Add("@P_ST_AVULSO", true);
                        p.Add("@P_LOGINAUDITAVULSO", null);
                        p.Add("@P_DT_AUDITAVULSO", null);
                        p.Add("@P_QT_PARCELAS", 1);
                        p.Add("@P_CD_MOEDARESULT", null);
                        p.Add("@P_OPERADOR", null);
                        p.Add("@P_VL_COTACAO", 0);
                        p.Add("@P_DT_COTACAO", null);
                        p.Add("P_LOGINDUP", duplicata.LoginDup);
                        await conexao._conexao.ExecuteAsync("IA_FIN_DUPLICATA", p, transaction: t, commandType: CommandType.StoredProcedure);
                        duplicata.Nr_lancto = p.Get<decimal>("@P_NR_LANCTO");
                        //Gravar Parcela
                        p = new DynamicParameters();
                        p.Add("@P_CD_EMPRESA", cd_empresa);
                        p.Add("@P_NR_LANCTO", duplicata.Nr_lancto);
                        p.Add("@P_CD_PARCELA", 1);
                        p.Add("@P_DT_VENCTO", duplicata.Dt_vencto);
                        p.Add("@P_VL_PARCELA", duplicata.Vl_parcela);
                        p.Add("@P_DT_AGENDAMENTO", null);
                        p.Add("@P_SITUACAO_AGENDAMENTO", null);
                        p.Add("@P_DETALHE_SITUACAO", null);
                        p.Add("@P_DT_CADASTRO_BANCO", null);
                        p.Add("@P_NUMEROAGENDAMENTO", null);
                        p.Add("@P_NUMEROAUTENTICACAO", null);
                        p.Add("@P_COD_BARRA", null);
                        p.Add("@P_LINHA_DIGITAVEL", null);
                        p.Add("@P_ST_REGISTRO", "A");
                        await conexao._conexao.ExecuteAsync("IA_FIN_PARCELA", p, transaction: t, commandType: CommandType.StoredProcedure);
                        if (duplicata.Id_plano > 0)
                        {
                            p = new DynamicParameters();
                            p.Add("@P_CD_EMPRESA", cd_empresa);
                            p.Add("@P_ID_LANCTO", dbType: DbType.Int32, direction: ParameterDirection.Output);
                            p.Add("@P_ID_PLANO", duplicata.Id_plano);
                            p.Add("@P_NR_LANCTO", duplicata.Nr_lancto);
                            p.Add("@P_CD_CONTAGER", null);
                            p.Add("@P_CD_LANCTOCAIXA", null);
                            p.Add("@P_ID_VENDARAPIDA", null);
                            p.Add("@P_CD_PRODUTO", null);
                            p.Add("@P_ID_LANCTOESTOQUE", null);
                            p.Add("@P_ID_DEVOLUCAO", null);
                            p.Add("@P_ID_ITEM", null);
                            p.Add("@P_VL_LANCTO", duplicata.Vl_parcela);
                            p.Add("@P_DT_LANCTO", duplicata.Dt_emissao);
                            p.Add("@P_OBS", duplicata.CompHistorico);
                            await conexao._conexao.ExecuteAsync("IA_FIN_LANCTOPLANOCONTAS", p, transaction: t, commandType: CommandType.StoredProcedure);
                        }
                        t.Commit();
                        return true;
                    }
                    else return false;
                }
            }
            catch(Exception ex) 
            {
                if (t != null)
                    t.Dispose();
                throw new Exception(ex.Message.Trim());
            }
        }

        public async Task<bool> ProrrogarVenctoAsync(string token, Duplicata duplicata)
        {
            try
            {
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        DynamicParameters p = new DynamicParameters();
                        p.Add("@CD_EMPRESA", duplicata.Cd_empresa);
                        p.Add("@NR_LANCTO", duplicata.Nr_lancto);
                        p.Add("@CD_PARCELA", duplicata.Cd_parcela);
                        p.Add("@DT_VENCTO", duplicata.Dt_prorrogacao);
                        await conexao._conexao.ExecuteAsync("update tb_fin_parcela set dt_vencto = @DT_VENCTO, dt_alt = getdate() " +
                                                            "where cd_empresa = @CD_EMPRESA and nr_lancto = @NR_LANCTO and cd_parcela = @CD_PARCELA", param: p);
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception ex){ throw new Exception(ex.Message.Trim()); }
        }
        public async Task<bool> QuitarDuplicataAsync(string token, string login, Duplicata duplicata)
        {
            SqlTransaction t = null;
            try
            {
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        t = conexao._conexao.BeginTransaction(IsolationLevel.ReadCommitted);
                        //Gravar caixa
                        DynamicParameters p = new DynamicParameters();
                        p.Add("@p_CD_EMPRESA", duplicata.Cd_empresa);
                        p.Add("@p_CD_CONTAGER", duplicata.Cd_contager);
                        p.Add("@P_DT_LANCTO", duplicata.Dt_quitar.Date);
                        p.Add("@P_CD_HISTORICO", duplicata.Cd_historico);
                        p.Add("@P_TP_MOV", duplicata.Tp_mov);
                        p.Add("@P_COMPLHISTORICO", null);
                        p.Add("@P_NM_CLIFOR", duplicata.Nm_clifor);
                        p.Add("@P_VALOR", duplicata.Vl_quitar > duplicata.Vl_atual ? duplicata.Vl_quitar - duplicata.Vl_atual : duplicata.Vl_quitar);
                        p.Add("@P_ST_TITULO", "N");
                        p.Add("@P_ST_ESTORNO", "N");
                        p.Add("@P_CD_LANCTOCAIXA", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                        p.Add("@P_LOGIN", login);
                        p.Add("@P_ST_AVULSO", "N");
                        p.Add("@P_LOGINAUDITAVULSO", null);
                        p.Add("@P_DT_AUDITAVULSO", null);
                        p.Add("@P_ANEXO", null, dbType: DbType.Binary);
                        p.Add("@P_EXT_ANEXO", null);
                        await conexao._conexao.ExecuteAsync("IA_FIN_CAIXA", param: p, transaction: t, commandType: CommandType.StoredProcedure);
                        decimal cd_lanctocaixa = p.Get<decimal>("@P_CD_LANCTOCAIXA");
                        decimal cd_caixa_desc = decimal.Zero;
                        decimal cd_caixa_juro = decimal.Zero;
                        if(duplicata.Vl_desconto > decimal.Zero)
                        {
                            //Buscar historico desconto
                            string cd_hist_desc = await conexao._conexao.ExecuteScalarAsync<string>("select CD_Historico_Desconto from TB_FIN_TPDuplicata where TP_Duplicata = '" + duplicata.Tp_duplicata + "'", transaction: t);
                            if (!string.IsNullOrWhiteSpace(cd_hist_desc))
                            {
                                p = new DynamicParameters();
                                p.Add("@p_CD_EMPRESA", duplicata.Cd_empresa);
                                p.Add("@p_CD_CONTAGER", duplicata.Cd_contager);
                                p.Add("@P_DT_LANCTO", duplicata.Dt_quitar.Date);
                                p.Add("@P_CD_HISTORICO", cd_hist_desc);
                                p.Add("@P_TP_MOV", duplicata.Tp_mov.Trim().ToUpper().Equals("P") ? "R" : "P");
                                p.Add("@P_COMPLHISTORICO", null);
                                p.Add("@P_NM_CLIFOR", duplicata.Nm_clifor);
                                p.Add("@P_VALOR", duplicata.Vl_desconto);
                                p.Add("@P_ST_TITULO", "N");
                                p.Add("@P_ST_ESTORNO", "N");
                                p.Add("@P_CD_LANCTOCAIXA", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                                p.Add("@P_LOGIN", login);
                                p.Add("@P_ST_AVULSO", "N");
                                p.Add("@P_LOGINAUDITAVULSO", null);
                                p.Add("@P_DT_AUDITAVULSO", null);
                                p.Add("@P_ANEXO", null, dbType: DbType.Binary);
                                p.Add("@P_EXT_ANEXO", null);
                                await conexao._conexao.ExecuteAsync("IA_FIN_CAIXA", param: p, transaction: t, commandType: CommandType.StoredProcedure);
                                cd_caixa_desc = p.Get<decimal>("@P_CD_LANCTOCAIXA");
                            }
                            else throw new Exception("Não existe historico desconto configurado.");
                        }
                        if(duplicata.Vl_quitar > duplicata.Vl_atual)
                        {
                            //Buscar historico juro
                            string cd_hist_juro = await conexao._conexao.ExecuteScalarAsync<string>("select CD_Historico_Juro from TB_FIN_TPDuplicata where TP_Duplicata = '" + duplicata.Tp_duplicata + "'", transaction: t);
                            if (!string.IsNullOrWhiteSpace(cd_hist_juro))
                            {
                                p = new DynamicParameters();
                                p.Add("@p_CD_EMPRESA", duplicata.Cd_empresa);
                                p.Add("@p_CD_CONTAGER", duplicata.Cd_contager);
                                p.Add("@P_DT_LANCTO", duplicata.Dt_quitar.Date);
                                p.Add("@P_CD_HISTORICO", cd_hist_juro);
                                p.Add("@P_TP_MOV", duplicata.Tp_mov);
                                p.Add("@P_COMPLHISTORICO", null);
                                p.Add("@P_NM_CLIFOR", duplicata.Nm_clifor);
                                p.Add("@P_VALOR", duplicata.Vl_quitar - duplicata.Vl_atual);
                                p.Add("@P_ST_TITULO", "N");
                                p.Add("@P_ST_ESTORNO", "N");
                                p.Add("@P_CD_LANCTOCAIXA", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                                p.Add("@P_LOGIN", login);
                                p.Add("@P_ST_AVULSO", "N");
                                p.Add("@P_LOGINAUDITAVULSO", null);
                                p.Add("@P_DT_AUDITAVULSO", null);
                                p.Add("@P_ANEXO", null, dbType: DbType.Binary);
                                p.Add("@P_EXT_ANEXO", null);
                                await conexao._conexao.ExecuteAsync("IA_FIN_CAIXA", param: p, transaction: t, commandType: CommandType.StoredProcedure);
                                cd_caixa_juro = p.Get<decimal>("@P_CD_LANCTOCAIXA");
                            }
                            else throw new Exception("Não existe historico juro configurado.");
                        }
                        //Gravar liquidacao
                        p = new DynamicParameters();
                        p.Add("@P_CD_EMPRESA", duplicata.Cd_empresa);
                        p.Add("@P_NR_LANCTO", duplicata.Nr_lancto);
                        p.Add("@P_CD_PARCELA", duplicata.Cd_parcela);
                        p.Add("@P_ID_LIQUID", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                        p.Add("@P_CD_PORTADOR", duplicata.Cd_portador);
                        p.Add("@P_ID_LOTECTB", null);
                        p.Add("@P_LOGINCANC", null);
                        p.Add("@P_CD_CONTAGER", duplicata.Cd_contager);
                        p.Add("@P_CD_LANCTOCAIXA", cd_lanctocaixa);
                        p.Add("@P_CD_LANCTOCAIXA_JURO", cd_caixa_juro > decimal.Zero ? cd_caixa_juro : null);
                        p.Add("@P_CD_LANCTOCAIXA_DESC", cd_caixa_desc > decimal.Zero ? cd_caixa_desc : null);
                        p.Add("@P_CD_LANCTOCAIXA_TROCOCH", null);
                        p.Add("@P_CD_LANCTOCAIXA_TROCODH", null);
                        p.Add("@P_CD_LANCTOCAIXA_PERDADUP", null);
                        p.Add("@P_CD_LANCTOCAIXA_ADTO", null);
                        p.Add("@P_DT_LIQUIDACAO", duplicata.Dt_quitar.Date);
                        p.Add("@P_VL_LIQUIDACAO", duplicata.Vl_quitar > duplicata.Vl_atual ? duplicata.Vl_quitar - duplicata.Vl_atual : duplicata.Vl_quitar);
                        p.Add("@P_VL_DESCONTOBONUS", duplicata.Vl_desconto);
                        p.Add("@P_VL_JUROACRESCIMO", duplicata.Vl_quitar > duplicata.Vl_atual ? duplicata.Vl_quitar - duplicata.Vl_atual : decimal.Zero);
                        p.Add("@P_VL_TROCOCH", decimal.Zero);
                        p.Add("@P_VL_TROCODH", decimal.Zero);
                        p.Add("@P_VL_ADTO", decimal.Zero);
                        p.Add("@P_VL_COTACAO", decimal.Zero);
                        p.Add("@P_DT_COTACAO", null);
                        p.Add("@P_OPERADOR", null);
                        p.Add("@P_MOTIVOCANC", null);
                        p.Add("@P_ST_REGISTRO", "A");
                        await conexao._conexao.ExecuteAsync("IA_FIN_LIQUIDACAO", param: p, transaction: t, commandType: CommandType.StoredProcedure);
                        t.Commit();
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception ex)
            {
                if (t != null)
                    t.Dispose();
                throw new Exception(ex.Message.Trim());
            }
        }
    }
}
