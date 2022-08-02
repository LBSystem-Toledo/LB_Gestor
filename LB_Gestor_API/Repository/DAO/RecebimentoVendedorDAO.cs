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
    public class RecebimentoVendedorDAO: IRecebimentoVendedor
    {
        readonly IConfiguration _config;
        public RecebimentoVendedorDAO(IConfiguration config) { _config = config; }

        public async Task<IEnumerable<RecebimentoVendedor>> GetRecebimentoVendedorAsync(string token, string login, int mes, int ano)
        {
            try
            {
                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        StringBuilder sql = new StringBuilder();
                        //Detalhes Mes
                        sql.AppendLine("select MONTH(a.DT_Liquidacao) as Mes,")
                            .AppendLine("YEAR(a.DT_Liquidacao) as Ano,")
                            .AppendLine("e.NM_Clifor as Vendedor,")
                            .AppendLine("ISNULL(SUM(ISNULL(a.Vl_Liquidacao, 0) + ISNULL(a.Vl_JuroAcrescimo, 0) - ISNULL(a.Vl_DescontoBonus, 0)), 0) as Vl_recebido")
                            .AppendLine("from TB_FIN_Liquidacao a")
                            .AppendLine("inner join TB_FIN_Duplicata b")
                            .AppendLine("on a.CD_Empresa = b.CD_Empresa")
                            .AppendLine("and a.Nr_Lancto = b.Nr_Lancto")
                            .AppendLine("and ISNULL(a.ST_Registro, 'A') <> 'C'")
                            .AppendLine("and ISNULL(b.ST_Registro, 'A') <> 'C'")
                            .AppendLine("inner join TB_FAT_Pedido_X_Duplicata c")
                            .AppendLine("on b.CD_Empresa = c.CD_Empresa")
                            .AppendLine("and b.Nr_Lancto = c.Nr_Lancto")
                            .AppendLine("inner join VTB_FAT_PEDIDO d")
                            .AppendLine("on c.Nr_Pedido = d.Nr_Pedido")
                            .AppendLine("inner join TB_FIN_Clifor e")
                            .AppendLine("on d.CD_Vendedor = e.CD_Clifor")
                            .AppendLine("inner join TB_DIV_Usuario_X_Empresa f")
                            .AppendLine("on a.CD_Empresa = f.CD_Empresa")
                            .AppendLine("where MONTH(a.DT_Liquidacao) = MONTH(GETDATE())")
                            .AppendLine("and YEAR(a.DT_Liquidacao) = YEAR(GETDATE())")
                            .AppendLine("and d.tp_movimento = 'S'")
                            .AppendLine("and f.Login = '" + login.Trim() + "'")
                            .AppendLine("group by MONTH(a.DT_Liquidacao), YEAR(a.DT_Liquidacao), e.NM_Clifor");
                        return await conexao._conexao.QueryAsync<RecebimentoVendedor>(sql.ToString());
                    }
                    else return null;
                }
            }
            catch { return null; }
        }
    }
}
