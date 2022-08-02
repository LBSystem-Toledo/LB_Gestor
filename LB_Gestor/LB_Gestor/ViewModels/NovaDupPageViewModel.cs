using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class NovaDupPageViewModel : ViewModelBase
    {
        private Duplicata _duplicata;
        public Duplicata Duplicata { get { return _duplicata; } set { SetProperty(ref _duplicata, value); } }
        private Registro _movcorrente;
        public Registro MovCorrente 
        { 
            get { return _movcorrente; } 
            set 
            {
                SetProperty(ref _movcorrente, value);
                BuscarHistorico();
            } 
        }
        private ObservableCollection<Registro> _movs;
        public ObservableCollection<Registro> Movs { get { return _movs; } set { SetProperty(ref _movs, value); } }
        private Historico _histcorrente;
        public Historico HistCorrente { get { return _histcorrente; } set { SetProperty(ref _histcorrente, value); } }
        private ObservableCollection<Historico> _historicos;
        public ObservableCollection<Historico> Historicos { get { return _historicos; } set { SetProperty(ref _historicos, value); } }
        private PlanoConta _contacorrente;
        public PlanoConta ContaCorrente { get { return _contacorrente; } set { SetProperty(ref _contacorrente, value); } }
        private ObservableCollection<PlanoConta> _planoContas;
        public ObservableCollection<PlanoConta> PlanoContas { get { return _planoContas; } set { SetProperty(ref _planoContas, value); } }
        private Cliente _cliente;
        public Cliente Cliente { get { return _cliente; } set { SetProperty(ref _cliente, value); } }

        public DelegateCommand LimparCliente { get; }
        public DelegateCommand BuscarClienteCommand { get; }
        public DelegateCommand SalvarCommand { get; }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public NovaDupPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;

            Duplicata = new Duplicata();

            Movs = new ObservableCollection<Registro>();
            Movs.Add(new Registro { Chave = "P", Valor = "PAGAR" });
            Movs.Add(new Registro { Chave = "R", Valor = "RECEBER" });

            LimparCliente = new DelegateCommand(() => Cliente = null);
            BuscarClienteCommand = new DelegateCommand(async () => await NavigationService.NavigateAsync("ConsultaClientePage"));

            SalvarCommand = new DelegateCommand(async () =>
            {
                if(Cliente == null)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar cliente/fornecedor.", "OK");
                    return;
                }
                if(HistCorrente == null)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório selecionar histórico.", "OK");
                    return;
                }
                if(Duplicata.Vl_parcela <= decimal.Zero)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar valor duplicata.", "OK");
                    return;
                }
                if(Duplicata.Dt_vencto.Date < Duplicata.Dt_emissao.Date)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Data vencimento não pode ser menor que data emissão.", "OK");
                    return;
                }
                Duplicata.Cd_clifor = Cliente.Cd_clifor;
                Duplicata.Cd_endereco = Cliente.Cd_endereco;
                Duplicata.Cd_historico = HistCorrente.Cd_historico;
                if (ContaCorrente != null)
                    Duplicata.Id_plano = ContaCorrente.Id_plano;
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                    {
                        try
                        {
                            await dataService.GravarDuplicataAsync(Duplicata);
                            await dialogService.DisplayAlertAsync("Mensagem", "Duplicata gravada com sucesso.", "OK");
                            await navigationService.GoBackAsync();
                        }
                        catch (Exception ex)
                        { await dialogService.DisplayAlertAsync("Erro", ex.Message.Trim(), "OK"); }
                    }
                else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
            });
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {

            if (parameters.ContainsKey("CLIENTE"))
                Cliente = parameters["CLIENTE"] as Cliente;
            if (parameters.ContainsKey("DUPLICATA"))
            {
                Duplicata = parameters["DUPLICATA"] as Duplicata;
                if (parameters.ContainsKey("CD_HISTORICO"))
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        var lista = await dataService.GetHistoricosAsync("P");
                        Historicos = new ObservableCollection<Historico>(lista);
                        HistCorrente = lista.ToList().FirstOrDefault(p => p.Cd_historico == parameters["CD_HISTORICO"].ToString());
                    }
                    else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");

                }
                if (parameters.ContainsKey("ID_PLANO"))
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        var planos = await dataService.GetPlanoContaAsync("D");
                        PlanoContas = new ObservableCollection<PlanoConta>(planos);
                        ContaCorrente = planos.ToList().FirstOrDefault(p => p.Id_plano == int.Parse(parameters["ID_PLANO"].ToString()));
                    }
                    else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
                }
                MovCorrente = Movs.First();
            }
        }
        private async Task BuscarHistorico()
        {
            if (MovCorrente != null)
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                    {
                        if (HistCorrente == null)
                        {
                            var lista = await dataService.GetHistoricosAsync(MovCorrente.Chave);
                            Historicos = new ObservableCollection<Historico>(lista);
                        }
                        if (ContaCorrente == null)
                        {
                            var planos = await dataService.GetPlanoContaAsync(MovCorrente.Chave.Equals("P") ? "D" : "R");
                            PlanoContas = new ObservableCollection<PlanoConta>(planos);
                        }
                    }
                else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
        }
    }
}
