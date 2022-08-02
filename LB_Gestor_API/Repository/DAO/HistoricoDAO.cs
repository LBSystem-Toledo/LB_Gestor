using Dapper;
using LB_Gestor_API.DataBase;
using LB_Gestor_API.Models;
using LB_Gestor_API.Repository.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.DAO
{
    public class HistoricoDAO: IHistorico
    {
        readonly IConfiguration _config;

        public HistoricoDAO(IConfiguration config) { _config = config; }

        public async Task<IEnumerable<Historico>> GetAsync(string token, string tp_mov)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.cd_historico, a.ds_historico, a.tp_mov")
                    .AppendLine("from TB_FIN_Historico a")
                    .AppendLine("where isnull(a.st_registro, 'A') <> 'C'");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<Historico>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
    }
}
