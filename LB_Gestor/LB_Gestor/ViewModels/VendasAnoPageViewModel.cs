using Acr.UserDialogs;
using LB_Gestor.Interface;
using Microcharts;
using Prism.Navigation;
using Prism.Services;
using System.Linq;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class VendasAnoPageViewModel : ViewModelBase
    {
        Chart _charvendas;
        public Chart ChartVendas { get { return _charvendas; } set { SetProperty(ref _charvendas, value); } }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public VendasAnoPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
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
                        //Faturamento por portador
                        var lista = await dataService.GetVendasPorAnoAsync();
                        ChartEntry[] entriP = new ChartEntry[lista.Count()];
                        int index = 0;
                        lista.ToList()
                            .OrderBy(p => p.Ano)
                            .ToList()
                            .ForEach(p =>
                            {
                                entriP[index++] = new ChartEntry((float)p.Vl_venda)
                                {
                                    Label = p.Ano.ToString(),
                                    ValueLabel = p.Vl_venda.ToString("C"),
                                    Color = SkiaSharp.SKColor.Parse("#f08b29")
                                };
                            });
                        ChartVendas = new LineChart
                        {
                            Entries = entriP
                        };
                    }
                else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
            }
            catch { }
        }
    }
}
