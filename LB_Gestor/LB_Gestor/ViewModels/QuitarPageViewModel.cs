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
    public class QuitarPageViewModel : ViewModelBase
    {
        private Duplicata _duplicata;
        public Duplicata Duplicata { get { return _duplicata; } set { SetProperty(ref _duplicata, value); } }
        private Portador _portador;
        public Portador Portador { get { return _portador; } set { SetProperty(ref _portador, value); } }
        private ObservableCollection<Portador> _portadores;
        public ObservableCollection<Portador> Portadores { get { return _portadores; } set { SetProperty(ref _portadores, value); } }
        private ContaGer _conta;
        public ContaGer Conta { get { return _conta; } set { SetProperty(ref _conta, value); } }
        private ObservableCollection<ContaGer> _contas;
        public ObservableCollection<ContaGer> Contas { get { return _contas; } set { SetProperty(ref _contas, value); } }

        public DelegateCommand ConfirmarCommand { get; }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public QuitarPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;

            ConfirmarCommand = new DelegateCommand(async () =>
            {
                if(Portador == null)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório selecionar portador.", "OK");
                    return;
                }
                if(Conta == null)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório selecionar conta.", "OK");
                    return;
                }
                if(Duplicata.Vl_quitar <= decimal.Zero)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar valor QUITAR.", "OK");
                    return;
                }
                if(Duplicata.Vl_desconto > Duplicata.Vl_quitar)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Valor DESCONTO não pode ser maior que valor QUITAR.", "OK");
                    return;
                }
                Duplicata.Cd_portador = Portador.Cd_portador;
                Duplicata.Cd_contager = Conta.Cd_contager;
                Duplicata.Dt_quitar = DateTime.Now.Date;
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                        try
                        {
                            await dataService.QuitarDuplicataAsync(Duplicata);
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
            Duplicata = parameters["DUPLICATA"] as Duplicata;
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                {
                    IEnumerable<Portador> lport = await dataService.GetPortadorAsync();
                    Portadores = new ObservableCollection<Portador>(lport);
                    IEnumerable<ContaGer> lcont = await dataService.GetSaldoContaAsync();
                    Contas = new ObservableCollection<ContaGer>(lcont);
                }
            else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
        }
    }
}
