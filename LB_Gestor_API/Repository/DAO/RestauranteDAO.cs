using Dapper;
using LB_Gestor_API.DataBase;
using LB_Gestor_API.Models;
using LB_Gestor_API.Repository.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.DAO
{
    public class RestauranteDAO: IRestaurante
    {
        readonly IConfiguration _config;

        public RestauranteDAO(IConfiguration config) { _config = config; }

        public async Task<IEnumerable<PedidoRestaurante>> GetPedidosHojeAsync(string token)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.modalidade,")
                    .AppendLine("COUNT(distinct ID_PreVenda) as Qt_pedidos,")
                    .AppendLine("SUM(isnull(a.Valor, 0)) as Vl_pedidos")
                    .AppendLine("from VTB_RES_VENDA a")
                    .AppendLine("where a.DT_Venda >= '" + DateTime.Now.ToString("yyyyMMdd") + " 05:00:00" + "'")
                    .AppendLine("group by a.modalidade");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<PedidoRestaurante>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<VendaGrupo>> GetVendasGrupoHojeAsync(string token)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.CD_Grupo, a.DS_Grupo,")
                    .AppendLine("sum(a.Quantidade) as Quantidade,")
                    .AppendLine("SUM(isnull(a.Valor, 0)) as Valor")
                    .AppendLine("from VTB_RES_VENDA a")
                    .AppendLine("where a.DT_Venda >= '" + DateTime.Now.ToString("yyyyMMdd") + " 05:00:00" + "'")
                    .AppendLine("group by a.CD_Grupo, a.DS_Grupo")
                    .AppendLine("order by Quantidade desc");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<VendaGrupo>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<PedidoRestaurante>> GetPedidosOntemAsync(string token)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.modalidade,")
                    .AppendLine("COUNT(distinct ID_PreVenda) as Qt_pedidos,")
                    .AppendLine("SUM(isnull(a.Valor, 0)) as Vl_pedidos")
                    .AppendLine("from VTB_RES_VENDA a")
                    .AppendLine("where a.DT_Venda <= '" + DateTime.Now.ToString("yyyyMMdd") + " 05:00:00" + "'")
                    .AppendLine("and a.DT_Venda >= '" + DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + " 04:59:59" + "'")
                    .AppendLine("group by a.modalidade");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<PedidoRestaurante>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<VendaGrupo>> GetVendasGrupoOntemAsync(string token)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.CD_Grupo, a.DS_Grupo,")
                    .AppendLine("sum(a.Quantidade) as Quantidade,")
                    .AppendLine("SUM(isnull(a.Valor, 0)) as Valor")
                    .AppendLine("from VTB_RES_VENDA a")
                    .AppendLine("where a.DT_Venda <= '" + DateTime.Now.ToString("yyyyMMdd") + " 05:00:00" + "'")
                    .AppendLine("and a.DT_Venda >= '" + DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + " 04:59:59" + "'")
                    .AppendLine("group by a.CD_Grupo, a.DS_Grupo")
                    .AppendLine("order by Quantidade desc");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<VendaGrupo>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<PedidoRestaurante>> GetPedidosFimSemanaAsync(string token)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select DATEPART(DW, a.DT_Venda) as DiaSemana, a.modalidade,")
                    .AppendLine("COUNT(distinct ID_PreVenda) as Qt_pedidos,")
                    .AppendLine("SUM(isnull(a.Valor, 0)) as Vl_pedidos")
                    .AppendLine("from VTB_RES_VENDA a")
                    .AppendLine("where CONVERT(datetime, floor(convert(decimal(30,10), a.DT_Venda))) >= convert(datetime, floor(convert(decimal(30,10), DATEADD(day, -28, GETDATE()))))")
                    .AppendLine("and DATEPART(DW, a.DT_Venda) in(6, 7, 1)")
                    .AppendLine("group by DATEPART(DW, a.DT_Venda), a.modalidade")
                    .AppendLine("order by DATEPART(DW, a.DT_Venda), a.modalidade");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<PedidoRestaurante>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<PedidoRestaurante>> GetPedidosOutrosDiasAsync(string token)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select DATEPART(DW, a.DT_Venda) as DiaSemana, a.modalidade,")
                    .AppendLine("COUNT(distinct ID_PreVenda) as Qt_pedidos,")
                    .AppendLine("SUM(isnull(a.Valor, 0)) as Vl_pedidos")
                    .AppendLine("from VTB_RES_VENDA a")
                    .AppendLine("where CONVERT(datetime, floor(convert(decimal(30,10), a.DT_Venda))) >= convert(datetime, floor(convert(decimal(30,10), DATEADD(day, -28, GETDATE()))))")
                    .AppendLine("and DATEPART(DW, a.DT_Venda) not in(6, 7, 1)")
                    .AppendLine("group by DATEPART(DW, a.DT_Venda), a.modalidade")
                    .AppendLine("order by DATEPART(DW, a.DT_Venda), a.modalidade");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<PedidoRestaurante>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<PedidoRestaurante>> GetEvolucao12MesesAsync(string token)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select MONTH(a.DT_Venda) as Mes,")
                    .AppendLine("YEAR(a.DT_Venda) as Ano,")
                    .AppendLine("SUM(isnull(a.Valor, 0)) as Vl_pedidos")
                    .AppendLine("from VTB_RES_VENDA a")
                    .AppendLine("where a.DT_Venda BETWEEN convert(date, dateadd(MONTH, -12, current_timestamp), 103) AND convert(date, current_timestamp, 103)")
                    .AppendLine("group by MONTH(a.DT_Venda), YEAR(a.DT_Venda)")
                    .AppendLine("order by YEAR(a.DT_Venda), MONTH(a.DT_Venda)");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<PedidoRestaurante>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<PedidoRestaurante>> GetEvolucao3AnosAsync(string token)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select MONTH(a.DT_Venda) as Mes,")
                    .AppendLine("YEAR(a.DT_Venda) as Ano,")
                    .AppendLine("SUM(isnull(a.Valor, 0)) as Vl_pedidos")
                    .AppendLine("from VTB_RES_VENDA a")
                    .AppendLine("where year(a.DT_Venda) >= YEAR(DATEADD(year, -3, getdate()))")
                    .AppendLine("group by MONTH(a.DT_Venda), YEAR(a.DT_Venda)")
                    .AppendLine("order by MONTH(a.DT_Venda), YEAR(a.DT_Venda)");

                using (TConexao conexao = new TConexao(_config.GetConnectionString(token)))
                {
                    if (await conexao.OpenConnectionAsync())
                        return await conexao._conexao.QueryAsync<PedidoRestaurante>(sql.ToString());
                    else return null;
                }
            }
            catch { return null; }
        }
    }
}
