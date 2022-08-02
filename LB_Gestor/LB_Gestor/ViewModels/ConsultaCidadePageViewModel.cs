using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class ConsultaCidadePageViewModel : ViewModelBase
    {
        private string _filtro = string.Empty;
        public string Filtro { get { return _filtro; } set { SetProperty(ref _filtro, value); } }
        private UF _ufcorrente;
        public UF Ufcorrente { get { return _ufcorrente; } set { SetProperty(ref _ufcorrente, value); } }
        private ObservableCollection<UF> _ufs;
        public ObservableCollection<UF> Ufs { get { return _ufs; } set { SetProperty(ref _ufs, value); } }
        private ObservableCollection<Cidade> _cidades;
        public ObservableCollection<Cidade> Cidades { get { return _cidades; } set { SetProperty(ref _cidades, value); } }
        public Cidade CidadeCorrente { get; set; }

        public DelegateCommand BuscarCommand { get; }
        public DelegateCommand SelecionarCommand { get; }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public ConsultaCidadePageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
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
                            IEnumerable<Cidade> lista = await dataService.GetCidadesAsync(string.Empty, Filtro, Ufcorrente?.Uf ?? string.Empty);
                            Cidades = new ObservableCollection<Cidade>(lista.ToList().OrderBy(p => p.UF));
                        }
                    else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
                }
                catch { }
            });

            SelecionarCommand = new DelegateCommand(async () =>
            {
                NavigationParameters param = new NavigationParameters();
                param.Add("CIDADE", CidadeCorrente);
                await NavigationService.GoBackAsync(param);
            });
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                    {
                        IEnumerable<UF> lista = await dataService.GetUFsAsync();
                        Ufs = new ObservableCollection<UF>(lista);
                        Ufs.Insert(0, new UF { Uf = "<TODOS>" });
                    }
                else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
            }
            catch { }
        }
    }
}
