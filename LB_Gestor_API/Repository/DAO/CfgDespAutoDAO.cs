using Dapper;
using LB_Gestor_API.DataBase;
using LB_Gestor_API.Models;
using LB_Gestor_API.Repository.Interface;
using LB_Gestor_API.Utils;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.DAO
{
    public class CfgDespAutoDAO: ICfgDespAuto
    {
        readonly IConfiguration _config;

        public CfgDespAutoDAO(IConfiguration config) { _config = config; }

        public async Task<CfgDespAuto> GetAsync(string token, string tp_despesa)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.cd_historico, a.id_plano, a.cd_clifor")
                    .AppendLine("from TB_FIN_CfgDespAuto a")
                    .AppendLine("inner join VTB_DIV_Empresa b")
                    .AppendLine("on a.CD_Empresa = b.CD_Empresa")
                    .AppendLine("where dbo.FVALIDA_NUMEROS(case when b.tp_pessoa = 'F' then b.NR_CPF else b.NR_CGC end) = '" + token.SoNumero() + "'")
                    .AppendLine("and a.tp_despesa = '" + tp_despesa.Trim() + "'");
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryFirstOrDefaultAsync<CfgDespAuto>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
    }
}
