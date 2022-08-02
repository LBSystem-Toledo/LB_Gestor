using Acr.UserDialogs;
using LB_Gestor.Interface;
using Microcharts;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class RecebimentoVendedorPageViewModel : ViewModelBase
    {
        DateTime _dtfiltro = DateTime.Now;
        public DateTime DtFiltro
        {
            get { return _dtfiltro; }
            set
            {
                SetProperty(ref _dtfiltro, value);
                RecebimentoMesAsync();
            }
        }
        Chart _charrecebimento;
        public Chart ChartRecebimentos { get { return _charrecebimento; } set { SetProperty(ref _charrecebimento, value); } }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public RecebimentoVendedorPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            await RecebimentoMesAsync();
        }
        private async Task RecebimentoMesAsync()
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                    {
                        //Faturamento por Mês
                        var lista = await dataService.GetRecebimentoVendedorAsync(DtFiltro.Month, DtFiltro.Year);
                        ChartEntry[] entriP = new ChartEntry[lista.Count()];
                        int index = 0;
                        lista.ToList()
                            .OrderBy(p => p.Vl_recebido)
                            .ToList()
                            .ForEach(p =>
                            {
                                entriP[index++] = new ChartEntry((float)p.Vl_recebido)
                                {
                                    Label = p.Vendedor,
                                    ValueLabel = p.Vl_recebido.ToString("C"),
                                    Color = SkiaSharp.SKColor.Parse("#f08b29")
                                };
                            });
                        ChartRecebimentos = new BarChart
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
