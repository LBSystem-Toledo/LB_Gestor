using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class QuitarDuplicataPageViewModel : ViewModelBase
    {
        decimal _vl_quitar;
        public decimal Vl_quitar { get { return _vl_quitar; } set { SetProperty(ref _vl_quitar, value); } }
        DateTime _dt_pagamento;
        public DateTime Dt_pagamento { get { return _dt_pagamento; } set { SetProperty(ref _dt_pagamento, value); } }
        Cliente _cliente;
        public Cliente Cliente { get { return _cliente; } set { SetProperty(ref _cliente, value); } }
        Duplicata _dupcorrente;
        public Duplicata DupCorrente { get { return _dupcorrente; } set { SetProperty(ref _dupcorrente, value); } }
        ObservableCollection<Duplicata> _duplicatas;
        public ObservableCollection<Duplicata> Duplicatas { get { return _duplicatas; } set { SetProperty(ref _duplicatas, value); } }
        private Portador _portador;
        public Portador Portador { get { return _portador; } set { SetProperty(ref _portador, value); } }
        private ObservableCollection<Portador> _portadores;
        public ObservableCollection<Portador> Portadores { get { return _portadores; } set { SetProperty(ref _portadores, value); } }
        private ContaGer _conta;
        public ContaGer Conta { get { return _conta; } set { SetProperty(ref _conta, value); } }
        private ObservableCollection<ContaGer> _contas;
        public ObservableCollection<ContaGer> Contas { get { return _contas; } set { SetProperty(ref _contas, value); } }

        public DelegateCommand BuscarClienteCommand { get; }
        public DelegateCommand ConfirmarCommand { get; }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public QuitarDuplicataPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;

            BuscarClienteCommand = new DelegateCommand(async () => await NavigationService.NavigateAsync("ConsultaClientePage"));
            ConfirmarCommand = new DelegateCommand(async () =>
            {
                if (Portador == null)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório selecionar portador.", "OK");
                    return;
                }
                if (Conta == null)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório selecionar conta.", "OK");
                    return;
                }
                if (DupCorrente == null)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório selecionar duplicata para quitar.", "OK");
                    return;
                }
                DupCorrente.Cd_portador = Portador.Cd_portador;
                DupCorrente.Cd_contager = Conta.Cd_contager;
                DupCorrente.Dt_quitar = Dt_pagamento;
                DupCorrente.Vl_quitar = Vl_quitar;
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                        try
                        {
                            await dataService.QuitarDuplicataAsync(DupCorrente);
                            await dialogService.DisplayAlertAsync("Mensagem", "Duplicata quitada com sucesso.", "OK");
                            await navigationService.GoBackAsync();
                        }
                        catch (Exception ex)
                        { await dialogService.DisplayAlertAsync("Erro", ex.Message.Trim(), "OK"); }
                else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
            });
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("CLIENTE"))
                Cliente = parameters["CLIENTE"] as Cliente;
            if (parameters.ContainsKey("DATA"))
                Dt_pagamento = DateTime.Parse(parameters["DATA"].ToString());
            if (parameters.ContainsKey("VALOR"))
                Vl_quitar = decimal.Parse(parameters["VALOR"].ToString());
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                {
                    IEnumerable<Portador> lport = await dataService.GetPortadorAsync();
                    Portadores = new ObservableCollection<Portador>(lport);
                    IEnumerable<ContaGer> lcont = await dataService.GetSaldoContaAsync();
                    Contas = new ObservableCollection<ContaGer>(lcont);
                    IEnumerable<Duplicata> lista = await dataService.GetDuplicatasAsync(Cliente == null ? string.Empty : Cliente.Cd_clifor, "P", Cliente == null ? Vl_quitar : decimal.Zero);
                    Duplicatas = new ObservableCollection<Duplicata>(lista);
                }
            else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
        }
    }
}
