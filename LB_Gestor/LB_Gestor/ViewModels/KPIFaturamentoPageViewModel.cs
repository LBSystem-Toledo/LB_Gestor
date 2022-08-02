using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class KPIFaturamentoPageViewModel : ViewModelBase
    {
        KPIFaturamento _kpi;
        public KPIFaturamento Kpi { get { return _kpi; } set { SetProperty(ref _kpi, value); } }
        

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public KPIFaturamentoPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                    {
                        Kpi = await dataService.GetKPIFaturamentoAsync();
                    }
                else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
            }
            catch { }
        }
    }
}
