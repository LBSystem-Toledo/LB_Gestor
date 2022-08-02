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
    public class CidadeDAO : ICidade
    {
        readonly IConfiguration _config;

        public CidadeDAO(IConfiguration config) { _config = config; }

        public async Task<IEnumerable<Cidade>> GetAsync(string token, string cd_cidade, string ds_cidade, string uf)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.cd_cidade, a.ds_cidade, b.UF")
                    .AppendLine("from TB_FIN_Cidade a")
                    .AppendLine("inner join TB_FIN_UF b")
                    .AppendLine("on a.CD_UF = b.CD_UF")
                    .AppendLine("where ISNULL(a.ST_Registro, 'A') <> 'C'");
                if (!string.IsNullOrWhiteSpace(cd_cidade))
                    sql.AppendLine("and a.cd_cidade = '" + cd_cidade.Trim() + "'");
                if (!string.IsNullOrWhiteSpace(ds_cidade))
                    sql.AppendLine("and a.ds_cidade like '%" + ds_cidade.Trim() + "%'");
                if (!string.IsNullOrWhiteSpace(uf))
                    sql.AppendLine("and b.uf = '" + uf.Trim() + "'");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<Cidade>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
    }
}
