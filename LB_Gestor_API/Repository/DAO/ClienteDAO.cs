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
    public class ClienteDAO: ICliente
    {
        readonly IConfiguration _config;
        public ClienteDAO(IConfiguration config) { _config = config; }

        public async Task<IEnumerable<Cliente>> GetAsync(string Token, string Cd_clifor, string Nome)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.CD_Clifor, a.CD_Cidade, a.DS_Cidade , a.UF, ")
                    .AppendLine("a.NM_Clifor, a.NM_Fantasia, a.CD_Endereco, ")
                    .AppendLine("a.NR_CGC, a.NR_CPF, dbo.FVALIDA_NUMEROS(a.cep) as cep, a.DS_Endereco, ")
                    .AppendLine("a.numero, a.bairro, a.ds_complemento, a.proximo")
                    .AppendLine("from VTB_FIN_CLIFOR a ")
                    .AppendLine("left join TB_FIS_CondFiscal_Clifor e ")
                    .AppendLine("on a.cd_condfiscal_clifor = e.cd_condfiscal_clifor ")
                    .AppendLine("where isnull(a.st_registro, 'A') <> 'C'");
                if (!string.IsNullOrWhiteSpace(Cd_clifor))
                    sql.AppendLine("and a.cd_clifor = '" + Cd_clifor.Trim() + "'");
                if (!string.IsNullOrWhiteSpace(Nome))
                    sql.AppendLine("and (a.nm_clifor like '%" + Nome + "%' or a.nm_fantasia like '%" + Nome + "%')");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(Token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<Cliente>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<Cliente> GetFornecedorCNPJAsync(string token, string cnpj)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.CD_Clifor, a.CD_Cidade, a.DS_Cidade , a.UF, ")
                    .AppendLine("a.NM_Clifor, a.NM_Fantasia, a.CD_Endereco, ")
                    .AppendLine("a.NR_CGC, a.NR_CPF, dbo.FVALIDA_NUMEROS(a.cep) as cep, a.DS_Endereco, ")
                    .AppendLine("a.numero, a.bairro, a.ds_complemento, a.proximo")
                    .AppendLine("from VTB_FIN_CLIFOR a ")
                    .AppendLine("left join TB_FIS_CondFiscal_Clifor e ")
                    .AppendLine("on a.cd_condfiscal_clifor = e.cd_condfiscal_clifor ")
                    .AppendLine("where isnull(a.st_registro, 'A') <> 'C'")
                    .AppendLine("and not exists(select 1 from tb_div_empresa x where x.cd_clifor = a.cd_clifor)");
                if (!string.IsNullOrWhiteSpace(cnpj))
                    sql.AppendLine("and dbo.FVALIDA_NUMEROS(a.nr_cgc) in(" + cnpj.Trim() + ")");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryFirstOrDefaultAsync<Cliente>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<string> GravarAsync(string Token, Cliente cliente)
        {
            SqlTransaction t = null;
            try
            {
                using (TConexao conexao = new TConexao(_config.GetConnectionString(Token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        t = conexao._conexao.BeginTransaction(IsolationLevel.ReadCommitted);
                        //Verificar se cliente ja esta cadastrado para CNPJ/CPF
                        if (string.IsNullOrWhiteSpace(cliente.Cd_clifor))
                        {
                            StringBuilder sql = new StringBuilder();
                            sql.AppendLine("select CD_Clifor ")
                                .AppendLine("from VTB_FIN_CLIFOR")
                                .AppendLine("where ISNULL(ST_Registro, 'A') <> 'C'");
                            if (!string.IsNullOrWhiteSpace(cliente.Nr_cpf.SoNumero()))
                                sql.AppendLine("and dbo.FVALIDA_NUMEROS(NR_CPF) = '" + cliente.Nr_cpf.SoNumero() + "'");
                            else sql.AppendLine("and dbo.FVALIDA_NUMEROS(NR_CGC) = '" + cliente.Nr_cgc.SoNumero() + "'");
                            cliente.Cd_clifor = await conexao._conexao.ExecuteScalarAsync<string>(sql.ToString(), transaction: t);
                        }
                        DynamicParameters p;
                        if (string.IsNullOrWhiteSpace(cliente.Cd_clifor))
                        {
                            StringBuilder sql = new StringBuilder();
                            sql.AppendLine("DECLARE @V_CD_CLIFOR VARCHAR(10)")
                                .AppendLine("EXEC P_FORMATAZERO  'TB_FIN_Clifor','CD_Clifor', '', @V_CD_CLIFOR, @V_CD_CLIFOR OUTPUT")
                                .AppendLine("INSERT INTO TB_FIN_Clifor(CD_Clifor, NM_Clifor, TP_Pessoa, Cd_CondFiscal_Clifor, ID_CategoriaClifor, ST_Registro, dt_cad, dt_alt)")
                                .AppendLine("VALUES(@V_CD_CLIFOR, @NM_CLIFOR, @TP_PESSOA, @CD_CONDFISCAL_CLIFOR, @ID_CATEGORIACLIFOR, 'A', getdate(), getdate())");
                            p = new DynamicParameters();
                            p.Add("@TP_PESSOA", cliente.Nr_cgc.SoNumero().Length.Equals(14) ? "J" : "F");
                            string cd_cond = await conexao._conexao.ExecuteScalarAsync<string>("select Cd_CondFiscal_Clifor from TB_FIS_CondFiscal_Clifor", transaction: t);
                            p.Add("@CD_CONDFISCAL_CLIFOR", cd_cond);
                            p.Add("@NM_CLIFOR", string.IsNullOrWhiteSpace(cliente.Nm_clifor) ? string.Empty : cliente.Nm_clifor.ToUpper());
                            string id_cat = await conexao._conexao.ExecuteScalarAsync<string>("select ID_CategoriaClifor from TB_FIN_CategoriaClifor", transaction: t);
                            p.Add("@ID_CATEGORIACLIFOR", id_cat);
                            await conexao._conexao.ExecuteAsync(sql.ToString(), param: p, transaction: t, commandType: CommandType.Text);
                            sql = new StringBuilder();
                            sql.AppendLine("select max(cd_clifor) from tb_fin_clifor");
                            cliente.Cd_clifor = await conexao._conexao.ExecuteScalarAsync<string>(sql.ToString(), transaction: t);
                            if (!cliente.Nr_cgc.SoNumero().Length.Equals(14))
                            {
                                sql.Clear();
                                sql.AppendLine("insert into TB_FIN_Clifor_PF(CD_Clifor, NR_CPF, DT_Cad, DT_Alt)")
                                    .AppendLine("values(@CD_CLIFOR, @NR_CPF, GETDATE(), GETDATE())");
                                p = new DynamicParameters();
                                p.Add("@CD_CLIFOR", cliente.Cd_clifor);
                                p.Add("@NR_CPF", cliente.Nr_cpf);
                                await conexao._conexao.ExecuteAsync(sql.ToString(), param: p, transaction: t, commandType: CommandType.Text);
                            }
                            else
                            {
                                sql.Clear();
                                sql.AppendLine("insert into TB_FIN_Clifor_PJ(CD_Clifor, NR_CGC, NM_Fantasia, DT_Cad, DT_Alt)")
                                    .AppendLine("values(@CD_CLIFOR, @NR_CGC, @NM_FANTASIA, GETDATE(), GETDATE())");
                                p = new DynamicParameters();
                                p.Add("@CD_CLIFOR", cliente.Cd_clifor);
                                p.Add("@NR_CGC", cliente.Nr_cgc);
                                p.Add("@NM_FANTASIA", string.IsNullOrWhiteSpace(cliente.Nm_fantasia) ? string.Empty : cliente.Nm_fantasia.ToUpper());
                                await conexao._conexao.ExecuteAsync(sql.ToString(), param: p, transaction: t, commandType: CommandType.Text);
                            }
                            p = new DynamicParameters();
                            p.Add("@P_CD_ENDERECO", dbType: DbType.String, size: 4, direction: ParameterDirection.Output);
                            p.Add("@P_CD_CLIFOR", cliente.Cd_clifor);
                            p.Add("@P_CD_CIDADE", cliente.Cd_cidade);
                            p.Add("@P_CD_PAIS", "1058");
                            p.Add("@P_DS_ENDERECO", string.IsNullOrWhiteSpace(cliente.Ds_endereco) ? string.Empty : cliente.Ds_endereco.ToUpper());
                            p.Add("@P_DS_COMPLEMENTO", null);
                            p.Add("@P_NUMERO", string.IsNullOrWhiteSpace(cliente.Numero) ? string.Empty : cliente.Numero.ToUpper());
                            p.Add("@P_BAIRRO", string.IsNullOrWhiteSpace(cliente.Bairro) ? string.Empty : cliente.Bairro.ToUpper());
                            p.Add("@P_PROXIMO", null);
                            p.Add("@P_CEP", cliente.Cep);
                            p.Add("@P_CP", null);
                            p.Add("@P_FONE", null);
                            p.Add("@P_INSC_ESTADUAL", null);
                            p.Add("@P_ST_NAOCONTRIBUINTE", null);
                            p.Add("@P_ST_ENDENTREGA", null);
                            p.Add("@P_ST_ENDCOBRANCA", null);
                            p.Add("@P_ST_REGISTRO", "A");
                            p.Add("@P_LATITUDE", null);
                            p.Add("@P_LONGITUDE", null);
                            p.Add("@P_CD_INTEGRACAO", null);
                            await conexao._conexao.ExecuteAsync("IA_FIN_ENDERECO", p, transaction: t, commandType: CommandType.StoredProcedure);
                            cliente.Cd_endereco = p.Get<string>("@P_CD_ENDERECO");
                        }
                        else
                        {
                            StringBuilder sql = new StringBuilder();
                            sql.AppendLine("update tb_fin_clifor set TP_PESSOA = @TP_PESSOA, ")
                                .AppendLine("NM_CLIFOR = @NM_CLIFOR, ")
                                .AppendLine("DT_ALT = GETDATE() ")
                                .AppendLine("WHERE CD_CLIFOR = @CD_CLIFOR");
                            p = new DynamicParameters();
                            p.Add("@CD_CLIFOR", cliente.Cd_clifor);
                            p.Add("@TP_PESSOA", cliente.Nr_cgc.SoNumero().Length.Equals(14) ? "J" : "F");
                            p.Add("@NM_CLIFOR", string.IsNullOrWhiteSpace(cliente.Nm_clifor) ? string.Empty : cliente.Nm_clifor.ToUpper());
                            int ret = await conexao._conexao.ExecuteAsync(sql.ToString(), param: p, transaction: t, commandType: CommandType.Text);
                            sql = new StringBuilder();
                            sql.AppendLine("select max(cd_clifor) from tb_fin_clifor");
                            if (!cliente.Nr_cgc.SoNumero().Length.Equals(14))
                            {
                                bool existe = await conexao._conexao.ExecuteScalarAsync<bool>("select 1 from tb_fin_clifor_pf where cd_clifor = '" + cliente.Cd_clifor + "'", transaction: t, commandType: CommandType.Text);
                                if (existe)
                                {
                                    sql.Clear();
                                    sql.AppendLine("UPDATE TB_FIN_CLIFOR_PF SET NR_CPF = @NR_CPF, DT_ALT = GETDATE()")
                                        .AppendLine("WHERE CD_CLIFOR = @CD_CLIFOR");
                                    p = new DynamicParameters();
                                    p.Add("@CD_CLIFOR", cliente.Cd_clifor);
                                    p.Add("@NR_CPF", cliente.Nr_cpf);
                                    await conexao._conexao.ExecuteAsync(sql.ToString(), param: p, transaction: t, commandType: CommandType.Text);
                                }
                                else
                                {
                                    //Deleta PJ
                                    await conexao._conexao.ExecuteAsync("delete tb_fin_clifor_pj where cd_clifor = '" + cliente.Cd_clifor.Trim() + "'", transaction: t, commandType: CommandType.Text);
                                    //Inclui PF
                                    sql.Clear();
                                    sql.AppendLine("insert into TB_FIN_Clifor_PF(CD_Clifor, NR_CPF, DT_Cad, DT_Alt)")
                                        .AppendLine("values(@CD_CLIFOR, @NR_CPF, GETDATE(), GETDATE())");
                                    p = new DynamicParameters();
                                    p.Add("@CD_CLIFOR", cliente.Cd_clifor);
                                    p.Add("@NR_CPF", cliente.Nr_cpf);
                                    await conexao._conexao.ExecuteAsync(sql.ToString(), param: p, transaction: t, commandType: CommandType.Text);
                                }
                            }
                            else
                            {
                                bool existe = await conexao._conexao.ExecuteScalarAsync<bool>("select 1 from tb_fin_clifor_pj where cd_clifor = '" + cliente.Cd_clifor + "'", transaction: t, commandType: CommandType.Text);
                                if (existe)
                                {
                                    sql.Clear();
                                    sql.AppendLine("UPDATE TB_FIN_CLIFOR_PJ SET NR_CGC = @NR_CGC, NM_FANTASIA = @NM_FANTASIA, DT_ALT = GETDATE()")
                                        .AppendLine("WHERE CD_CLIFOR = @CD_CLIFOR");
                                    p = new DynamicParameters();
                                    p.Add("@CD_CLIFOR", cliente.Cd_clifor);
                                    p.Add("@NR_CGC", cliente.Nr_cgc);
                                    p.Add("@NM_FANTASIA", string.IsNullOrWhiteSpace(cliente.Nm_fantasia) ? string.Empty : cliente.Nm_fantasia.ToUpper());
                                    await conexao._conexao.ExecuteAsync(sql.ToString(), param: p, transaction: t, commandType: CommandType.Text);
                                }
                                else
                                {
                                    //Deleta PF
                                    await conexao._conexao.ExecuteAsync("delete tb_fin_clifor_pf where cd_clifor = '" + cliente.Cd_clifor.Trim() + "'", transaction: t, commandType: CommandType.Text);
                                    //Incluir PJ
                                    sql.Clear();
                                    sql.AppendLine("insert into TB_FIN_Clifor_PJ(CD_Clifor, NR_CGC, NM_Fantasia, DT_Cad, DT_Alt)")
                                        .AppendLine("values(@CD_CLIFOR, @NR_CGC, @NM_FANTASIA, GETDATE(), GETDATE())");
                                    p = new DynamicParameters();
                                    p.Add("@CD_CLIFOR", cliente.Cd_clifor);
                                    p.Add("@NR_CGC", cliente.Nr_cgc);
                                    p.Add("@NM_FANTASIA", string.IsNullOrWhiteSpace(cliente.Nm_fantasia) ? string.Empty : cliente.Nm_fantasia.ToUpper());
                                    await conexao._conexao.ExecuteAsync(sql.ToString(), param: p, transaction: t, commandType: CommandType.Text);
                                }
                            }
                            p = new DynamicParameters();
                            p.Add("@P_CD_ENDERECO", cliente.Cd_endereco);
                            p.Add("@P_CD_CLIFOR", cliente.Cd_clifor);
                            p.Add("@P_CD_CIDADE", cliente.Cd_cidade);
                            p.Add("@P_CD_PAIS", "1058");
                            p.Add("@P_DS_ENDERECO", string.IsNullOrWhiteSpace(cliente.Ds_endereco) ? string.Empty : cliente.Ds_endereco.ToUpper());
                            p.Add("@P_DS_COMPLEMENTO", null);
                            p.Add("@P_NUMERO", string.IsNullOrWhiteSpace(cliente.Numero) ? string.Empty : cliente.Numero.ToUpper());
                            p.Add("@P_BAIRRO", string.IsNullOrWhiteSpace(cliente.Bairro) ? string.Empty : cliente.Bairro.ToUpper());
                            p.Add("@P_PROXIMO", null);
                            p.Add("@P_CEP", cliente.Cep);
                            p.Add("@P_CP", null);
                            p.Add("@P_FONE", null);
                            p.Add("@P_INSC_ESTADUAL", null);
                            p.Add("@P_ST_NAOCONTRIBUINTE", null);
                            p.Add("@P_ST_ENDENTREGA", null);
                            p.Add("@P_ST_ENDCOBRANCA", null);
                            p.Add("@P_ST_REGISTRO", "A");
                            p.Add("@P_LATITUDE", null);
                            p.Add("@P_LONGITUDE", null);
                            p.Add("@P_CD_INTEGRACAO", null);
                            await conexao._conexao.ExecuteAsync("IA_FIN_ENDERECO", p, transaction: t, commandType: CommandType.StoredProcedure);
                            cliente.Cd_endereco = p.Get<string>("@P_CD_ENDERECO");
                        }

                        t.Commit();
                        return cliente.Cd_clifor + "|" + cliente.Cd_endereco;
                    }
                    else return string.Empty;
                }
            }
            catch(Exception ex)
            {
                if (t != null)
                    t.Dispose();
                throw new Exception(ex.Message.Trim());
            }
        }
    }
}
