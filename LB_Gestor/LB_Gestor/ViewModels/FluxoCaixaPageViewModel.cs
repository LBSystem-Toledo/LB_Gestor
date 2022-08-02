using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using LB_Gestor.Utils;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class FluxoCaixaPageViewModel : ViewModelBase
    {
        CountryGdp _gdppagar;
        public CountryGdp GdpValuePagar { get { return _gdppagar; } set { SetProperty(ref _gdppagar, value); } }
        CountryGdp _gdpreceber;
        public CountryGdp GdpValueReceber { get { return _gdpreceber; } set { SetProperty(ref _gdpreceber, value); } }
        CountryGdp _gdpacumulado;
        public CountryGdp GdpValueAcumulado { get { return _gdpacumulado; } set { SetProperty(ref _gdpacumulado, value); } }
        ObservableCollection<FluxoCaixa> _fluxocaixa;
        public ObservableCollection<FluxoCaixa> FluxoCaixa { get { return _fluxocaixa; } set { SetProperty(ref _fluxocaixa, value); } }

        DateTime _dtfluxo = DateTime.Now.AddDays(7);
        public DateTime DtFluxo
        {
            get { return _dtfluxo; }
            set
            {
                SetProperty(ref _dtfluxo, value);
                BuscarFluxoCaixaAsycn();
            }
        }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public FluxoCaixaPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;
        }
        
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            await BuscarFluxoCaixaAsycn();
        }
        
        private async Task BuscarFluxoCaixaAsycn()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                {
                    IEnumerable<FluxoCaixa> lFluxo = await dataService.GetFluxoCaixaAsync(DtFluxo);
                    if (lFluxo == null ? false : lFluxo.Count() > 0)
                    {
                        List<FluxoCaixa> lista = lFluxo.ToList();
                        lista.Insert(0, new Models.FluxoCaixa { Dt_vencto = lFluxo.First().Dt_vencto.AddDays(-1), SaldoAntCC = lFluxo.First().SaldoAntCC });
                        FluxoCaixa = new ObservableCollection<FluxoCaixa>(lista);
                        GdpValue[] pagar = new GdpValue[0];
                        GdpValue[] receber = new GdpValue[0];
                        GdpValue[] resultado = new GdpValue[0];
                        lista.ToList().ForEach(p =>
                        {
                            Array.Resize(ref pagar, pagar.Length + 1);
                            pagar[pagar.Length - 1] = new GdpValue(p.Dt_vencto.Date, string.Empty, p.Vl_pagar * (-1));
                            Array.Resize(ref receber, receber.Length + 1);
                            receber[receber.Length - 1] = new GdpValue(p.Dt_vencto.Date, string.Empty, p.Vl_receber);
                            Array.Resize(ref resultado, resultado.Length + 1);
                            resultado[resultado.Length - 1] = new GdpValue(p.Dt_vencto.Date, string.Empty, p.SaldoProjetado);
                        });
                        GdpValuePagar = new CountryGdp("PAGAR", pagar);
                        GdpValueReceber = new CountryGdp("RECEBER", receber);
                        GdpValueAcumulado = new CountryGdp("ACUMULADO", resultado);
                    }
                    else
                    {
                        FluxoCaixa = new ObservableCollection<FluxoCaixa>();
                        GdpValuePagar = null;
                        GdpValueReceber = null;
                        GdpValueAcumulado = null;
                    }
                }
            else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
        }
    }
}
