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
    public class PlanoContaDAO : IPlanoConta
    {
        readonly IConfiguration _config;

        public PlanoContaDAO(IConfiguration config) { _config = config; }

        public async Task<IEnumerable<PlanoConta>> GetAsync(string token, string tp_mov)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.ID_Plano, a.DS_Conta, a.TP_Registro")
                    .AppendLine("from TB_FIN_PlanoContas a")
                    .AppendLine("where ISNULL(a.Cancelado, 0) <> 1")
                    .AppendLine("and a.Sintetico <> 1");
                if (!string.IsNullOrWhiteSpace(tp_mov))
                    sql.AppendLine("and a.tp_registro = '" + tp_mov.Trim().ToUpper() + "'");
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<PlanoConta>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
    }
}
