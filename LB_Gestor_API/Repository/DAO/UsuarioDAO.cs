using Dapper;
using LB_Gestor_API.DataBase;
using LB_Gestor_API.Models;
using LB_Gestor_API.Repository.Interface;
using LB_Gestor_API.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace LB_Gestor_API.Repository.DAO
{
    public class UsuarioDAO : IUsuario
    {
        readonly IConfiguration _config;
        public UsuarioDAO(IConfiguration config) { _config = config; }

        public async Task<Token> ValidarUsuario(Usuario user)
        {
            try
            {
                //Verificar se cnpj esta habilitado para utilizar mobile
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select 1")
                    .AppendLine("from TB_CRM_CLIENTE")
                    .AppendLine("where ISNULL(Mobile, 0) = 1")
                    .AppendLine("and ISNULL(ST_REGISTRO, 'A') <> 'I'")
                    .AppendLine("and dbo.FVALIDA_NUMEROS(CNPJ) = '" + user.Cnpj.SoNumero() + "'");
                bool cnpj_habilitado = false;
                using (TConexao conexao = new TConexao(_config.GetConnectionString("conexaoHelp")))
                {
                    if (await conexao.OpenConnectionAsync())
                        cnpj_habilitado = await conexao._conexao.ExecuteScalarAsync<bool>(sql.ToString());
                }
                if (cnpj_habilitado)
                {
                    sql = new StringBuilder();
                    sql.AppendLine("select 1 ")
                        .AppendLine("from TB_DIV_Usuario a")
                        .AppendLine("where ISNULL(a.ST_Registro, 'A') <> 'C'")
                        .AppendLine("and a.Tp_Registro = 'U'")
                        .AppendLine("and a.Login = '" + user.Login.Trim() + "'")
                        .AppendLine("and a.Senha = '" + user.Senha.Trim() + "'");
                    using (TConexao conexao = new TConexao(_config.GetConnectionString(user.Cnpj.SoNumero())))
                    {
                        if (await conexao.OpenConnectionAsync())
                            if (await conexao._conexao.ExecuteScalarAsync<bool>(sql.ToString()))
                            {
                                var issuer = _config["Jwt:Issuer"];
                                var audience = _config["Jwt:Audience"];
                                var expiry = DateTime.Now.AddHours(24);
                                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                                var token = new JwtSecurityToken(issuer: issuer,
                                                                 audience: audience,
                                                                 expires: DateTime.Now.AddHours(24),
                                                                 signingCredentials: credentials);
                                var tokenHandler = new JwtSecurityTokenHandler();
                                string tk = tokenHandler.WriteToken(token);
                                //Buscar perfil App
                                //sql = new StringBuilder();
                                //sql.AppendLine("select PerfilAppGestor")
                                //    .AppendLine("from TB_DIV_Usuario a")
                                //    .AppendLine("where ISNULL(a.ST_Registro, 'A') <> 'C'")
                                //    .AppendLine("and a.Tp_Registro = 'U'")
                                //    .AppendLine("and a.Login = '" + user.Login.Trim() + "'")
                                //    .AppendLine("and a.Senha = '" + user.Senha.Trim() + "'");
                                //string perfil = await conexao._conexao.ExecuteScalarAsync<string>(sql.ToString());
                                return new Token
                                {
                                    //PerfilAppGestor = perfil,
                                    access_token = tk,
                                    expires_in = 24
                                };
                            }
                            else return new Token { };
                        else return new Token { };
                    }
                }
                else return new Token { };
            }
            catch { return new Token { }; }
        }
    }
}
