using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class RecebimentoPortadorPageViewModel : ViewModelBase
    {
        DateTime _dtfiltro = DateTime.Now;
        public DateTime DtFiltro
        {
            get { return _dtfiltro; }
            set
            {
                SetProperty(ref _dtfiltro, value);
                BuscarRecebimentosPortador();
            }
        }
        ObservableCollection<RecebimentoPortador> _portadores;
        public ObservableCollection<RecebimentoPortador> Portadores { get { return _portadores; } set { SetProperty(ref _portadores, value); } }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public RecebimentoPortadorPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            await BuscarRecebimentosPortador();
        }
        private async Task BuscarRecebimentosPortador()
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                    {
                        //Faturamento por portador
                        Portadores = new ObservableCollection<RecebimentoPortador>(await dataService.GetRecebimentoPortadorAsync(DtFiltro.Month, DtFiltro.Year));
                    }
                else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
            }
            catch { }
        }
    }
}
