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
    public class VendasGrupoPageViewModel : ViewModelBase
    {
        DateTime _dtfiltro = DateTime.Now;
        public DateTime DtFiltro
        {
            get { return _dtfiltro; }
            set
            {
                SetProperty(ref _dtfiltro, value);
                VendasMesAsync();
            }
        }
        Chart _charvendas;
        public Chart ChartVendas { get { return _charvendas; } set { SetProperty(ref _charvendas, value); } }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public VendasGrupoPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            await VendasMesAsync();
        }
        private async Task VendasMesAsync()
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                    {
                        //Faturamento por Mês
                        var lista = await dataService.GetVendasGrupoAsync(DtFiltro.Month, DtFiltro.Year);
                        ChartEntry[] entriP = new ChartEntry[lista.Count()];
                        int index = 0;
                        lista.ToList()
                            .OrderBy(p => p.Vl_venda)
                            .ToList()
                            .ForEach(p =>
                            {
                                entriP[index++] = new ChartEntry((float)p.Vl_venda)
                                {
                                    Label = p.Ds_grupo,
                                    ValueLabel = p.Vl_venda.ToString("C"),
                                    Color = SkiaSharp.SKColor.Parse("#f08b29")
                                };
                            });
                        ChartVendas = new BarChart
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
