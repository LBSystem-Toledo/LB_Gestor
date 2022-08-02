using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Microcharts;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class VendasMesPageViewModel : ViewModelBase
    {
        Registro _anocorrente;
        public Registro AnoCorrente
        {
            get { return _anocorrente; }
            set
            {
                SetProperty(ref _anocorrente, value);
                VendasMesAsync();
            }
        }
        ObservableCollection<Registro> _anos;
        public ObservableCollection<Registro> Anos { get { return _anos; } set { SetProperty(ref _anos, value); } }
        Chart _charvendas;
        public Chart ChartVendas { get { return _charvendas; } set { SetProperty(ref _charvendas, value); } }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public VendasMesPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;

            Anos = new ObservableCollection<Registro>();
            for (int i = 0; i < 10; i++)
                Anos.Add(new Registro { Chave = DateTime.Now.AddYears(i * -1).Year.ToString(), Valor = DateTime.Now.AddYears(i * -1).Year.ToString() });
            AnoCorrente = Anos.FirstOrDefault();
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
                        var lista = await dataService.GetVendasPorMesAsync(int.Parse(AnoCorrente.Chave));
                        ChartEntry[] entriP = new ChartEntry[lista.Count()];
                        int index = 0;
                        lista.ToList()
                            .OrderBy(p => p.Mes)
                            .ToList()
                            .ForEach(p =>
                            {
                                entriP[index++] = new ChartEntry((float)p.Vl_venda)
                                {
                                    Label = p.MesStr,
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
