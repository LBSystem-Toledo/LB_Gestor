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
    public class DuplicatasPageViewModel : ViewModelBase
    {
        Cliente _cliente;
        public Cliente Cliente { get { return _cliente; } set { SetProperty(ref _cliente, value); } }
        Registro _movcorrente;
        public Registro Movcorrente { get { return _movcorrente; } set { SetProperty(ref _movcorrente, value); } }
        ObservableCollection<Registro> _mov;
        public ObservableCollection<Registro> Mov { get { return _mov; } set { SetProperty(ref _mov, value); } }
        ObservableCollection<Duplicata> _duplicatas;
        public ObservableCollection<Duplicata> Duplicatas { get { return _duplicatas; } set { SetProperty(ref _duplicatas, value); } }

        public DelegateCommand LimparCliente { get; }
        public DelegateCommand BuscarClienteCommand { get; }
        public DelegateCommand BuscarDuplicataCommand { get; }
        public DelegateCommand NovaDuplicataCommand { get; }
        public DelegateCommand<Duplicata> ProrrogarCommand { get; }
        public DelegateCommand<Duplicata> ReceberCommand { get; }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public DuplicatasPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            : base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;
            Mov = new ObservableCollection<Registro>();
            Mov.Add(new Registro { Chave = "P", Valor = "PAGAR" });
            Mov.Add(new Registro { Chave = "R", Valor = "RECEBER" });
            Movcorrente = Mov.First();

            LimparCliente = new DelegateCommand(() => Cliente = null);
            BuscarClienteCommand = new DelegateCommand(async () => await NavigationService.NavigateAsync("ConsultaClientePage"));
            BuscarDuplicataCommand = new DelegateCommand(async () =>
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                    {
                        IEnumerable<Duplicata> lista = await dataService.GetDuplicatasAsync(Cliente?.Cd_clifor, Movcorrente.Chave, decimal.Zero);
                        Duplicatas = new ObservableCollection<Duplicata>(lista);
                    }
                else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
            });
            NovaDuplicataCommand = new DelegateCommand(async () => await NavigationService.NavigateAsync("NovaDupPage"));
            ProrrogarCommand = new DelegateCommand<Duplicata>(async (Duplicata d) =>
            {
                NavigationParameters p = new NavigationParameters();
                p.Add("DUPLICATA", d);
                await navigationService.NavigateAsync("ProrrogarVenctoPage", p);
            });
            ReceberCommand = new DelegateCommand<Duplicata>(async (Duplicata d) =>
            {
                NavigationParameters p = new NavigationParameters();
                p.Add("DUPLICATA", d);
                await navigationService.NavigateAsync("QuitarPage", p);
            });
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("CLIENTE"))
                Cliente = parameters["CLIENTE"] as Cliente;
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                {
                    IEnumerable<Duplicata> lista = await dataService.GetDuplicatasAsync(Cliente?.Cd_clifor, Movcorrente.Chave, decimal.Zero);
                    Duplicatas = new ObservableCollection<Duplicata>(lista);
                }
            else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
        }
    }
}
