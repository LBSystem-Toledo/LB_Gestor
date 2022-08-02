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
    public class PortadorDAO : IPortador
    {
        readonly IConfiguration _config;

        public PortadorDAO(IConfiguration config) { _config = config; }

        public async Task<IEnumerable<Portador>> GetAsync(string token)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select CD_Portador, DS_Portador")
                    .AppendLine("from TB_FIN_Portador")
                    .AppendLine("where ISNULL(ST_Registro, 'A') <> 'C'")
                    .AppendLine("and ISNULL(ST_ControleTitulo, 'N') <> 'S'")
                    .AppendLine("and ST_CartaoCredito = 1");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<Portador>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
    }
}
