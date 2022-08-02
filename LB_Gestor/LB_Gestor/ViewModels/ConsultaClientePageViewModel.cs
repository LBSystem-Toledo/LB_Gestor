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
    public class ConsultaClientePageViewModel : ViewModelBase
    {
        private string _filtrocliente = string.Empty;
        public string Filtrocliente { get { return _filtrocliente; } set { SetProperty(ref _filtrocliente, value); } }
        private ObservableCollection<Cliente> _clientes;
        public ObservableCollection<Cliente> Clientes { get { return _clientes; } set { SetProperty(ref _clientes, value); } }

        public DelegateCommand BuscarCommand { get; }
        public DelegateCommand NovoCommand { get; }
        public DelegateCommand<Cliente> SelecionarCommand { get; }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public ConsultaClientePageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;

            BuscarCommand = new DelegateCommand(async () =>
            {
                try
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                        {
                            IEnumerable<Cliente> lista = await dataService.GetClientesAsync(string.Empty, Filtrocliente);
                            Clientes = new ObservableCollection<Cliente>(lista.OrderBy(p => p.Nm_clifor));
                        }
                    else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
                }
                catch { }
            });
            NovoCommand = new DelegateCommand(async () => await NavigationService.NavigateAsync("NovoClientePage"));
            SelecionarCommand = new DelegateCommand<Cliente>(async (Cliente c) =>
            {
                if (c != null)
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("CLIENTE", c);
                    await NavigationService.GoBackAsync(param);
                }
                else await NavigationService.GoBackAsync();
            });
        }
    }
}
