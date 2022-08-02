using LB_Gestor.Interface;
using LB_Gestor.Models;
using LB_Gestor.Utils;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LB_Gestor.DataService
{
    public class DataService: IDataService
    {
        public async Task<Token> ValidarLoginAsync(Usuario user) 
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Usuario/ValidarLoginAsync", Method.Post);
                    request.AddHeader("Accept", "application/json")
                        .AddJsonBody<Usuario>(user);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<Token>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<ContaGer>> GetSaldoContaAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/ContaGerencial/GetAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<ContaGer>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<ResumoPagarReceber> GetResumoPagarReceberAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/ContaGerencial/GetResumoPagarReceberAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<ResumoPagarReceber>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<ReceitaDespesa>> GetReceitaDespesaAsync(int mes, int ano, string tp_mov)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/ContaGerencial/GetReceitasDespesasAsync?mes=" + mes + "&ano=" + ano + "&tp_mov=" + tp_mov, Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<ReceitaDespesa>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<ReceitaDespesa>> GetResultadoAnoAsync(int ano)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/ContaGerencial/GetResultadoAnoAsync?ano=" + ano, Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<ReceitaDespesa>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<FluxoCaixa>> GetFluxoCaixaAsync(DateTime dt_final)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/ContaGerencial/GetFluxoCaixaAsync?dt_final=" + dt_final.ToString("yyyy-MM-dd"), Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<FluxoCaixa>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<DRE>> GetDREAsync(int ano)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/ContaGerencial/GetDREAsync?ano=" + ano, Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<DRE>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<RecebimentoPortador>> GetRecebimentoPortadorAsync(int mes, int ano)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/ContaGerencial/GetRecebimentoPortadorAsync?mes=" + mes.ToString() + "&ano=" + ano.ToString(), Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<RecebimentoPortador>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<Duplicata>> GetDuplicatasAsync(string cd_clifor, string tp_mov, decimal valor)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Duplicata/GetAsync?cd_clifor=" + cd_clifor + "&tp_mov=" + tp_mov + "&valor=" + valor.ToString("N2", new System.Globalization.CultureInfo("en-US", true)), Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<Duplicata>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<bool> GravarDuplicataAsync(Duplicata duplicata)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Duplicata/GravarAsync", Method.Post);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddJsonBody<Duplicata>(duplicata);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<bool>(response.Content);
                    else throw new Exception(response.Content);
                }
            }
            catch { return false; }
        }
        public async Task<bool> ProrrogarVenctoAsync(Duplicata duplicata)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Duplicata/ProrrogarVenctoAsync", Method.Post);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddJsonBody<Duplicata>(duplicata);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<bool>(response.Content);
                    else throw new Exception(response.Content);
                }
            }
            catch { return false; }
        }
        public async Task<bool> QuitarDuplicataAsync(Duplicata duplicata)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Duplicata/QuitarDuplicataAsync", Method.Post);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login)
                        .AddJsonBody<Duplicata>(duplicata);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<bool>(response.Content);
                    else throw new Exception(response.Content);
                }
            }
            catch { return false; }
        }
        public async Task<bool> GravarCaixaAsync(Caixa caixa)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Caixa/GravarAsync", Method.Post);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddJsonBody<Caixa>(caixa);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<bool>(response.Content);
                    else throw new Exception(response.Content);
                }
            }
            catch { return false; }
        }
        public async Task<IEnumerable<Historico>> GetHistoricosAsync(string tp_mov)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Historico/GetAsync?tp_mov=" + tp_mov, Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<Historico>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<PlanoConta>> GetPlanoContaAsync(string tp_mov)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/PlanoConta/GetAsync?tp_mov=" + tp_mov, Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<PlanoConta>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<Portador>> GetPortadorAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Portador/GetAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<Portador>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<Cliente>> GetClientesAsync(string cd_clifor, string nm_clifor)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Cliente/GetAsync?Cd_cliente=" + cd_clifor + "&Nome=" + nm_clifor, Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<Cliente>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<Cliente> GetFornecedorCNPJAsync(string cnpj)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Cliente/GetFornecedorCNPJAsync?cnpj=" + cnpj, Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<Cliente>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<string> GravarClienteAsync(Cliente cliente)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Cliente/GravarAsync?", Method.Post);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddJsonBody<Cliente>(cliente);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<string>(response.Content);
                    else throw new Exception(response.Content);
                }
            }
            catch { return string.Empty; }
        }
        public async Task<TEndereco_CEPRest> GetCEPAsync(string cep)
        {
            try
            {
                using (var client = new RestClient("https://viacep.com.br"))
                {
                    var request = new RestRequest("/ws/" + cep.SoNumero() + "/json", Method.Get);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<TEndereco_CEPRest>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<ConsultaCNPJ> GetClienteCNPJAsync(string cnpj)
        {
            try
            {
                using (var client = new RestClient("https://www.receitaws.com.br"))
                {
                    var request = new RestRequest("/v1/cnpj/" + cnpj.SoNumero(), Method.Get);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<ConsultaCNPJ>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<Cidade>> GetCidadesAsync(string Cd_cidade, string Ds_cidade, string Uf)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Cidade/GetAsync?Cd_cidade=" + Cd_cidade + "&Ds_cidade=" + Ds_cidade + "&Uf=" + Uf, Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<Cidade>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<UF>> GetUFsAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/UF/GetAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<UF>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<CfgDespAuto> GetCfgDespAutoAsync(string tp_despesa)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/CfgDespAuto/GetAsync?tp_despesa=" + tp_despesa, Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<CfgDespAuto>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }

        public async Task<KPIFaturamento> GetKPIFaturamentoAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/KPIFaturamento/GetKPIAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<KPIFaturamento>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<Faturamento>> GetVendasPorAnoAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/KPIFaturamento/GetVendasPorAnoAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<Faturamento>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<Faturamento>> GetVendasPorMesAsync(int ano)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/KPIFaturamento/GetVendasPorMesAsync?ano=" + ano, Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<Faturamento>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<FaturamentoPortador>> GetVendasPortadorAsync(int mes, int ano)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/KPIFaturamento/GetVendasPortadorAsync?mes=" + mes.ToString() + "&ano=" + ano.ToString(), Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<FaturamentoPortador>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<Faturamento>> GetVendasCidadeAsync(int mes, int ano)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/KPIFaturamento/GetVendasCidadeAsync?mes=" + mes.ToString() + "&ano=" + ano.ToString(), Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<Faturamento>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<Faturamento>> GetVendasGrupoAsync(int mes, int ano)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/KPIFaturamento/GetVendasGrupoAsync?mes=" + mes.ToString() + "&ano=" + ano.ToString(), Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<Faturamento>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<Faturamento>> GetVendasVendedoresAsync(int mes, int ano)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/KPIFaturamento/GetVendasVendedoresAsync?mes=" + mes.ToString() + "&ano=" + ano.ToString(), Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<Faturamento>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<RecebimentoVendedor>> GetRecebimentoVendedorAsync(int mes, int ano)
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/RecebimentoVendedor/GetRecebimentoVendedorAsync?mes=" + mes.ToString() + "&ano=" + ano.ToString(), Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj)
                        .AddHeader("login", App.token.Login);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<RecebimentoVendedor>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }

        #region Restaurante
        public async Task<IEnumerable<PedidoRestaurante>> GetPedidosHojeAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Restaurante/GetPedidosHojeAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<PedidoRestaurante>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<VendaGrupo>> GetVendasGrupoHojeAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Restaurante/GetVendasGrupoHojeAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<VendaGrupo>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<PedidoRestaurante>> GetPedidosOntemAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Restaurante/GetPedidosOntemAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<PedidoRestaurante>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<VendaGrupo>> GetVendasGrupoOntemAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Restaurante/GetVendasGrupoOntemAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<VendaGrupo>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<PedidoRestaurante>> GetPedidosFimSemanaAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Restaurante/GetPedidosFimSemanaAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<PedidoRestaurante>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<PedidoRestaurante>> GetPedidosOutrosDiasAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Restaurante/GetPedidosOutrosDiasAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<PedidoRestaurante>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<PedidoRestaurante>> GetEvolucao12MesesAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Restaurante/GetEvolucao12MesesAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<PedidoRestaurante>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<PedidoRestaurante>> GetEvolucao3AnosAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Restaurante/GetEvolucao3AnosAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<PedidoRestaurante>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        #endregion

        #region Estoque
        public async Task<KPIEstoque> GetKPIEstoqueAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Estoque/GetKPIAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<KPIEstoque>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<ProdutoAbaixoMinimo>> GetProdutosAbaixoMinimoAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Estoque/GetProdutosAbaixoMinimoAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<ProdutoAbaixoMinimo>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        public async Task<IEnumerable<ProdutoAbaixoMargemLucro>> GetProdutosAbaixoMargemLucroAsync()
        {
            try
            {
                using (var client = new RestClient(App.url_api))
                {
                    var request = new RestRequest("/api/Estoque/GetProdutosAbaixoMargemLucroAsync", Method.Get);
                    request.AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Bearer " + App.token.access_token)
                        .AddHeader("cnpj", App.token.Cnpj);
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                        return JsonConvert.DeserializeObject<IEnumerable<ProdutoAbaixoMargemLucro>>(response.Content);
                    else return null;
                }
            }
            catch { return null; }
        }
        #endregion
    }
}
