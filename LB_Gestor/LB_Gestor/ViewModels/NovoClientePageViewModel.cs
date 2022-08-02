using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using LB_Gestor.Utils;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class NovoClientePageViewModel : ViewModelBase
    {
        private bool _isFisica = false;
        public bool isFisica { get { return _isFisica; } set { SetProperty(ref _isFisica, value); } }
        private bool _isJuridica = false;
        public bool isJuridica { get { return _isJuridica; } set { SetProperty(ref _isJuridica, value); } }
        private Cliente _cliente;
        public Cliente Cliente { get { return _cliente; } set { SetProperty(ref _cliente, value); } }
        private Registro _tipopessoacorrente;
        public Registro TipoPessoaCorrente
        {
            get { return _tipopessoacorrente; }
            set
            {
                SetProperty(ref _tipopessoacorrente, value);
                isFisica = value.Chave.Equals("F");
                isJuridica = value.Chave.Equals("J");
            }
        }
        private ObservableCollection<Registro> _tipospessoas;
        public ObservableCollection<Registro> TiposPessoas { get { return _tipospessoas; } set { SetProperty(ref _tipospessoas, value); } }

        public DelegateCommand SalvarCommand { get; }
        public DelegateCommand BuscarCnpjCommand { get; }
        public DelegateCommand BuscarCepCommand { get; }
        public DelegateCommand BuscarCidadeCommand { get; }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public NovoClientePageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;

            TiposPessoas = new ObservableCollection<Registro>();
            TiposPessoas.Add(new Registro { Chave = "J", Valor = "JURIDICA" });
            TiposPessoas.Add(new Registro { Chave = "F", Valor = "FISICA" });

            Cliente = new Cliente();

            BuscarCepCommand = new DelegateCommand(async () =>
            {
                if (Cliente.Cep.SoNumero().Length != 8)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "CEP Invalido.", "OK");
                    return;
                }
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                    {
                        TEndereco_CEPRest ret = await dataService.GetCEPAsync(Cliente.Cep);
                        if (ret != null)
                        {
                            if (!string.IsNullOrEmpty(ret.logradouro.Trim()))
                                Cliente.Ds_endereco = ret.logradouro;
                            if (!string.IsNullOrEmpty(ret.ibge.Trim()))
                            {
                                IEnumerable<Cidade> cid = await dataService.GetCidadesAsync(ret.ibge, string.Empty, string.Empty);
                                if (cid.Count() > 0)
                                {
                                    Cliente.Cd_cidade = cid.First().Cd_cidade;
                                    Cliente.Ds_cidade = cid.First().Ds_cidade;
                                    Cliente.UF = cid.First().UF;
                                }
                            }
                            if (!string.IsNullOrEmpty(ret.bairro.Trim()))
                                Cliente.Bairro = ret.bairro;
                        }
                    }
                else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
            });
            BuscarCnpjCommand = new DelegateCommand(async () =>
            {
                if (string.IsNullOrWhiteSpace(Cliente.Nr_cgc.SoNumero()))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "CNPJ Invalido.", "OK");
                    return;
                }
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                    {
                        var ret = await dataService.GetClienteCNPJAsync(Cliente.Nr_cgc);
                        if (ret != null)
                        {
                            Cliente.Nm_clifor = ret.Nome;
                            Cliente.Nm_fantasia = ret.Fantasia;
                            Cliente.Cep = ret.Cep;
                            Cliente.Numero = ret.Numero;
                            var ret_cep = await dataService.GetCEPAsync(Cliente.Cep);
                            if (ret_cep != null)
                            {
                                if (!string.IsNullOrEmpty(ret_cep.logradouro.Trim()))
                                    Cliente.Ds_endereco = ret_cep.logradouro;
                                if (!string.IsNullOrEmpty(ret_cep.ibge.Trim()))
                                {
                                    IEnumerable<Cidade> cid = await dataService.GetCidadesAsync(ret_cep.ibge, string.Empty, string.Empty);
                                    if (cid.Count() > 0)
                                    {
                                        Cliente.Cd_cidade = cid.First().Cd_cidade;
                                        Cliente.Ds_cidade = cid.First().Ds_cidade;
                                        Cliente.UF = cid.First().UF;
                                    }
                                }
                                if (!string.IsNullOrEmpty(ret_cep.bairro.Trim()))
                                    Cliente.Bairro = ret_cep.bairro;
                            }
                        }
                    }
                else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
            });
            SalvarCommand = new DelegateCommand(async () =>
            {
                if (string.IsNullOrWhiteSpace(Cliente.Nm_clifor))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar Razão Social/Nome cliente.", "OK");
                    return;
                }
                if (!Cliente.Nr_cgc.ValidaCNPJ() && !Cliente.Nr_cpf.ValidaCPF())
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar CNPJ ou CPF valido.", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Cliente.Ds_endereco))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar Rua.", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Cliente.Numero))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar Numero.", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Cliente.Bairro))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar Bairro.", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Cliente.Cd_cidade))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar Cidade.", "OK");
                    return;
                }
                try
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                        {

                            if (TipoPessoaCorrente.Chave.Trim().ToUpper().Equals("F"))
                            {
                                Cliente.Nr_cgc = string.Empty;
                                Cliente.Nm_fantasia = string.Empty;
                            }
                            else
                                Cliente.Nr_cpf = string.Empty;
                            if (!string.IsNullOrWhiteSpace(await dataService.GravarClienteAsync(Cliente)))
                            {
                                await dialogService.DisplayAlertAsync("Mensagem", "Cliente gravado com sucesso.", "OK");
                                await NavigationService.GoBackAsync();
                            }
                            else await dialogService.DisplayAlertAsync("Mensagem", "Erro ao gravar cliente.", "OK");
                        }
                    else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
                }
                catch (Exception ex)
                { await dialogService.DisplayAlertAsync("Erro", ex.Message.Trim(), "OK"); }
            });
            BuscarCidadeCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("ConsultaCidadePage", null);
            });
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.ContainsKey("CIDADE"))
                {
                    Cidade c = parameters["CIDADE"] as Cidade;
                    Cliente.Cd_cidade = c.Cd_cidade;
                    Cliente.Ds_cidade = c.Ds_cidade;
                    Cliente.UF = c.UF;
                }
            }
        }
    }
}
