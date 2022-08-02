using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Microcharts;
using Prism.Navigation;
using Prism.Services;
using SkiaSharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class ResumoFinPageViewModel : ViewModelBase
    {
        ObservableCollection<ContaGer> _contas;
        public ObservableCollection<ContaGer> Contas { get { return _contas; } set { SetProperty(ref _contas, value); } }
        ResumoPagarReceber _resumo;
        public ResumoPagarReceber Resumo { get { return _resumo; } set { SetProperty(ref _resumo, value); } }
        Chart _chartreceita;
        public Chart ChartReceita { get { return _chartreceita; } set { SetProperty(ref _chartreceita, value); } }
        Chart _chartdespesa;
        public Chart ChartDespesa { get { return _chartdespesa; } set { SetProperty(ref _chartdespesa, value); } }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public ResumoFinPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                {
                    Contas = new ObservableCollection<ContaGer>(await dataService.GetSaldoContaAsync());
                    Resumo = await dataService.GetResumoPagarReceberAsync();
                }
            else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
        }
    }
}
