using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class PedidoFimSemanaRestaurantePageViewModel : ViewModelBase
    {
        ObservableCollection<PedidoRestaurante> _pedidostotal;
        public ObservableCollection<PedidoRestaurante> PedidosTotal { get { return _pedidostotal; } set { SetProperty(ref _pedidostotal, value); } }
        ObservableCollection<PedidoRestaurante> _pedidosDia;
        public ObservableCollection<PedidoRestaurante> PedidosDia { get { return _pedidosDia; } set { SetProperty(ref _pedidosDia, value); }}
        ObservableCollection<PedidoRestaurante> _pedidossexta;
        public ObservableCollection<PedidoRestaurante> PedidosSexta { get { return _pedidossexta; } set { SetProperty(ref _pedidossexta, value); } }
        ObservableCollection<PedidoRestaurante> _pedidossabado;
        public ObservableCollection<PedidoRestaurante> PedidosSabado { get { return _pedidossabado; } set { SetProperty(ref _pedidossabado, value); } }
        ObservableCollection<PedidoRestaurante> _pedidosdomingo;
        public ObservableCollection<PedidoRestaurante> PedidosDomingo { get { return _pedidosdomingo; } set { SetProperty(ref _pedidosdomingo, value); } }

        public DelegateCommand AtualizarCommand { get; }

        readonly IDataService dataService;
        public PedidoFimSemanaRestaurantePageViewModel(INavigationService navigationService, IDataService _dataservice)
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
                    IEnumerable<PedidoRestaurante> pedidos = await dataService.GetPedidosFimSemanaAsync();
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
                        PedidosSexta = new ObservableCollection<PedidoRestaurante>(pedidos.ToList().FindAll(p => p.DiaSemana.Equals(6)));
                        PedidosSabado = new ObservableCollection<PedidoRestaurante>(pedidos.ToList().FindAll(p => p.DiaSemana.Equals(7)));
                        PedidosDomingo = new ObservableCollection<PedidoRestaurante>(pedidos.ToList().FindAll(p => p.DiaSemana.Equals(1)));
                    }
                }
        }
    }
}
