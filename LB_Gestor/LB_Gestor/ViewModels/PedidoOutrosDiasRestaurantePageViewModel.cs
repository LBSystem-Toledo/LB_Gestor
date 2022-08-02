using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class PedidoOutrosDiasRestaurantePageViewModel : ViewModelBase
    {
        ObservableCollection<PedidoRestaurante> _pedidostotal;
        public ObservableCollection<PedidoRestaurante> PedidosTotal { get { return _pedidostotal; } set { SetProperty(ref _pedidostotal, value); } }
        ObservableCollection<PedidoRestaurante> _pedidosDia;
        public ObservableCollection<PedidoRestaurante> PedidosDia { get { return _pedidosDia; } set { SetProperty(ref _pedidosDia, value); } }
        ObservableCollection<PedidoRestaurante> _pedidossegunda;
        public ObservableCollection<PedidoRestaurante> PedidosSegunda { get { return _pedidossegunda; } set { SetProperty(ref _pedidossegunda, value); } }
        ObservableCollection<PedidoRestaurante> _pedidosterca;
        public ObservableCollection<PedidoRestaurante> PedidosTerca { get { return _pedidosterca; } set { SetProperty(ref _pedidosterca, value); } }
        ObservableCollection<PedidoRestaurante> _pedidosquarta;
        public ObservableCollection<PedidoRestaurante> PedidosQuarta { get { return _pedidosquarta; } set { SetProperty(ref _pedidosquarta, value); } }
        ObservableCollection<PedidoRestaurante> _pedidosquinta;
        public ObservableCollection<PedidoRestaurante> PedidosQuinta { get { return _pedidosquinta; } set { SetProperty(ref _pedidosquinta, value); } }

        public DelegateCommand AtualizarCommand { get; }

        readonly IDataService dataService;
        public PedidoOutrosDiasRestaurantePageViewModel(INavigationService navigationService, IDataService _dataservice)
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
                    IEnumerable<PedidoRestaurante> pedidos = await dataService.GetPedidosOutrosDiasAsync();
                    if (pedidos?.Count() > 0)
                    {
                        PedidosTotal = new ObservableCollection<PedidoRestaurante>();
                        pedidos.GroupBy(p => p.Modalidade,
                        (aux, venda) =>
                        new
                        {
                            modalidade = aux,
                            qtd = venda.Sum(x => x.Qt_pedidos),
                            vl = venda.Sum(x => x.Vl_pedidos)
                        }).ToList()
                        .ForEach(p => PedidosTotal.Add(new PedidoRestaurante { Modalidade = p.modalidade, Qt_pedidos = p.qtd, Vl_pedidos = p.vl }));
                        PedidosDia = new ObservableCollection<PedidoRestaurante>();
                        pedidos.GroupBy(p => p.DiaSemana,
                        (aux, venda) =>
                        new
                        {
                            dia = aux,
                            qtd = venda.Sum(x => x.Qt_pedidos),
                            vl = venda.Sum(x => x.Vl_pedidos)
                        }).ToList()
                        .ForEach(p => PedidosDia.Add(new PedidoRestaurante { DiaSemana = p.dia, Qt_pedidos = p.qtd, Vl_pedidos = p.vl }));
                        PedidosSegunda = new ObservableCollection<PedidoRestaurante>(pedidos.ToList().FindAll(p => p.DiaSemana.Equals(2)));
                        PedidosTerca = new ObservableCollection<PedidoRestaurante>(pedidos.ToList().FindAll(p => p.DiaSemana.Equals(3)));
                        PedidosQuarta = new ObservableCollection<PedidoRestaurante>(pedidos.ToList().FindAll(p => p.DiaSemana.Equals(4)));
                        PedidosQuinta = new ObservableCollection<PedidoRestaurante>(pedidos.ToList().FindAll(p => p.DiaSemana.Equals(5)));
                    }
                }
        }
    }
}
