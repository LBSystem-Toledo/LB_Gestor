using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class CaixaPageViewModel : ViewModelBase
    {
        private Caixa _caixa;
        public Caixa Caixa { get { return _caixa; } set { SetProperty(ref _caixa, value); } }
        private Historico _histcorrente;
        public Historico HistCorrente { get { return _histcorrente; } set { SetProperty(ref _histcorrente, value); } }
        private ObservableCollection<Historico> _historicos;
        public ObservableCollection<Historico> Historicos { get { return _historicos; } set { SetProperty(ref _historicos, value); } }
        private PlanoConta _contacorrente;
        public PlanoConta ContaCorrente { get { return _contacorrente; } set { SetProperty(ref _contacorrente, value); } }
        private ObservableCollection<PlanoConta> _planoContas;
        public ObservableCollection<PlanoConta> PlanoContas { get { return _planoContas; } set { SetProperty(ref _planoContas, value); } }
        private ContaGer _contagercorrente;
        public ContaGer ContaGerCorrente { get { return _contagercorrente; } set { SetProperty(ref _contagercorrente, value); } }
        private ObservableCollection<ContaGer> _Contasger;
        public ObservableCollection<ContaGer> ContasGer { get { return _Contasger; } set { SetProperty(ref _Contasger, value); } }

        public DelegateCommand ConfirmarCommand { get; }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public CaixaPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;

            Caixa = new Caixa();

            ConfirmarCommand = new DelegateCommand(async () =>
            {
                if(ContaGerCorrente == null)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório selecionar conta gerencial.", "OK");
                    return;
                }
                if(HistCorrente == null)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório selecionar histórico.", "OK");
                    return;
                }
                Caixa.Cd_contager = ContaGerCorrente.Cd_contager;
                Caixa.Cd_historico = HistCorrente.Cd_historico;
                if (ContaCorrente != null)
                    Caixa.Id_plano = ContaCorrente.Id_plano;
                Caixa.Login = App.token.Login;
                try
                {
                    if(Connectivity.NetworkAccess == NetworkAccess.Internet)
                        using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                        {
                            await dataService.GravarCaixaAsync(Caixa);
                            await dialogService.DisplayAlertAsync("Mensagem", "Caixa gravado com sucesso.", "OK");
                            await NavigationService.GoBackAsync();
                        }
                    else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
                }
                catch(Exception ex) {await dialogService.DisplayAlertAsync("Erro", ex.Message.Trim(), "OK"); }
            });
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("DATA"))
                Caixa.Dt_lancto = DateTime.Parse(parameters["DATA"].ToString());
            if (parameters.ContainsKey("VALOR"))
                Caixa.Valor = decimal.Parse(parameters["VALOR"].ToString());
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                {
                    var contas = await dataService.GetSaldoContaAsync();
                    if(contas != null)
                        ContasGer = new ObservableCollection<ContaGer>(contas);
                    var historicos = await dataService.GetHistoricosAsync("P");
                    if(historicos != null)
                        Historicos = new ObservableCollection<Historico>(historicos);
                    var planos = await dataService.GetPlanoContaAsync("D");
                    if(planos != null)
                        PlanoContas = new ObservableCollection<PlanoConta>(planos);
                }
            else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
        }
    }
}
