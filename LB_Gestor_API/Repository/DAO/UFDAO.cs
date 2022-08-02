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
    public class UFDAO: IUF
    {
        readonly IConfiguration _config;
        public UFDAO(IConfiguration config) { _config = config; }

        public async Task<IEnumerable<UF>> GetAsync(string token)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.UF")
                    .AppendLine("from TB_FIN_UF a");
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<UF>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
    }
}
