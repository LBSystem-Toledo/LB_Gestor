using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Microcharts;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class PedidosHojeRestaurantePageViewModel : ViewModelBase
    {
        ObservableCollection<PedidoRestaurante> _pedidoshoje;
        public ObservableCollection<PedidoRestaurante> PedidosHoje { get { return _pedidoshoje; } set { SetProperty(ref _pedidoshoje, value); } }
        Chart _chartgrupo;
        public Chart ChartGrupo { get { return _chartgrupo; } set { SetProperty(ref _chartgrupo, value); } }

        public DelegateCommand AtualizarCommand { get; }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public PedidosHojeRestaurantePageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;

            AtualizarCommand = new DelegateCommand(async () => await AtualizarAsync());
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
                    PedidosHoje = new ObservableCollection<PedidoRestaurante>(await dataService.GetPedidosHojeAsync());
                    IEnumerable<VendaGrupo> vendas = await dataService.GetVendasGrupoHojeAsync();
                    ChartEntry[] entri = new ChartEntry[vendas.Count()];
                    int index = 0;
                    vendas.OrderBy(p => p.Quantidade)
                        .ToList()
                        .ForEach(p =>
                        {
                            entri[index++] = new ChartEntry((float)p.Valor)
                            {
                                Label = p.Ds_grupo.Trim(),
                                ValueLabel = p.Quantidade.ToString("N0", new System.Globalization.CultureInfo("pt-BR", true)),
                                Color = SKColor.Parse("#f08b29")
                            };
                        });
                    ChartGrupo = new BarChart
                    {
                        Entries = entri
                    };
                }
        }
    }
}
