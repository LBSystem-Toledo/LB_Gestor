using Dapper;
using LB_Gestor_API.DataBase;
using LB_Gestor_API.Models;
using LB_Gestor_API.Repository.Interface;
using LB_Gestor_API.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.DAO
{
    public class CaixaDAO: ICaixa
    {
        readonly IConfiguration _config;
        public CaixaDAO(IConfiguration config) { _config = config; }

        public async Task<bool> GravarAsync(string token, Caixa caixa)
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
                        //Gravar caixa
                        DynamicParameters p = new DynamicParameters();
                        p.Add("@p_CD_EMPRESA", cd_empresa);
                        p.Add("@p_CD_CONTAGER", caixa.Cd_contager);
                        p.Add("@P_DT_LANCTO", caixa.Dt_lancto);
                        p.Add("@P_CD_HISTORICO", caixa.Cd_historico);
                        p.Add("@P_TP_MOV", "P");
                        p.Add("@P_COMPLHISTORICO", caixa.CompHistorico);
                        p.Add("@P_NM_CLIFOR", null);
                        p.Add("@P_VALOR", caixa.Valor);
                        p.Add("@P_ST_TITULO", "N");
                        p.Add("@P_ST_ESTORNO", "N");
                        p.Add("@P_CD_LANCTOCAIXA", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                        p.Add("@P_LOGIN", caixa.Login);
                        p.Add("@P_ST_AVULSO", "S");
                        p.Add("@P_LOGINAUDITAVULSO", null);
                        p.Add("@P_DT_AUDITAVULSO", null);
                        p.Add("@P_ANEXO", null, dbType: DbType.Binary);
                        p.Add("@P_EXT_ANEXO", null);
                        await conexao._conexao.ExecuteAsync("IA_FIN_CAIXA", param: p, transaction: t, commandType: CommandType.StoredProcedure);
                        decimal lanctocaixa = p.Get<decimal>("@P_CD_LANCTOCAIXA");
                        if(caixa.Id_plano > 0)
                        {
                            p = new DynamicParameters();
                            p.Add("@P_CD_EMPRESA", cd_empresa);
                            p.Add("@P_ID_LANCTO", dbType: DbType.Int32, direction: ParameterDirection.Output);
                            p.Add("@P_ID_PLANO", caixa.Id_plano);
                            p.Add("@P_NR_LANCTO", null);
                            p.Add("@P_CD_CONTAGER", caixa.Cd_contager);
                            p.Add("@P_CD_LANCTOCAIXA", lanctocaixa);
                            p.Add("@P_ID_VENDARAPIDA", null);
                            p.Add("@P_CD_PRODUTO", null);
                            p.Add("@P_ID_LANCTOESTOQUE", null);
                            p.Add("@P_ID_DEVOLUCAO", null);
                            p.Add("@P_ID_ITEM", null);
                            p.Add("@P_VL_LANCTO", caixa.Valor);
                            p.Add("@P_DT_LANCTO", caixa.Dt_lancto);
                            p.Add("@P_OBS", null);
                            await conexao._conexao.ExecuteAsync("IA_FIN_LANCTOPLANOCONTAS", p, transaction: t, commandType: CommandType.StoredProcedure);
                        }
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
