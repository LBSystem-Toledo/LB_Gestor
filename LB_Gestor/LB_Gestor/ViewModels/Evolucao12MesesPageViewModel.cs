using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Microcharts;
using Prism.Navigation;
using SkiaSharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class Evolucao12MesesPageViewModel : ViewModelBase
    {
        Chart _chartevolucao;
        public Chart ChartEvolucao { get { return _chartevolucao; } set { SetProperty(ref _chartevolucao, value); } }

        readonly IDataService dataService;
        public Evolucao12MesesPageViewModel(INavigationService navigationService, IDataService _dataservice)
            :base(navigationService)
        {
            dataService = _dataservice;
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            await AtualizarAsync();
        }

        private async Task AtualizarAsync()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                {
                    IEnumerable<PedidoRestaurante> pedidos = await dataService.GetEvolucao12MesesAsync();
                    ChartEntry[] entri = new ChartEntry[pedidos.Count()];
                    int index = 0;
                    pedidos
                        .ToList()
                        .ForEach(p =>
                        {
                            entri[index++] = new ChartEntry((float)p.Vl_pedidos)
                            {
                                Label = p.MesAno.Trim(),
                                ValueLabel = p.Vl_pedidos.ToString("N2", new System.Globalization.CultureInfo("pt-BR", true)),
                                Color = SKColor.Parse("#f08b29")
                            };
                        });
                    ChartEvolucao = new LineChart
                    {
                        Entries = entri
                    };
                }
        }
    }
}
