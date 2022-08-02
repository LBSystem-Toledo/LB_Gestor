using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using LB_Gestor.Utils;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class Evolucao3AnosPageViewModel : ViewModelBase
    {
        CountryGdp _gdpanoatual;
        public CountryGdp GdpValueAnoAtual { get { return _gdpanoatual; } set { SetProperty(ref _gdpanoatual, value); } }
        CountryGdp _gdpanoant;
        public CountryGdp GdpValueAnoAnt { get { return _gdpanoant; } set { SetProperty(ref _gdpanoant, value); } }
        CountryGdp _gdpanoant1;
        public CountryGdp GdpValueAnoAnt1 { get { return _gdpanoant1; } set { SetProperty(ref _gdpanoant1, value); } }

        readonly IDataService dataService;
        public Evolucao3AnosPageViewModel(INavigationService navigationService, IDataService _dataservice)
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
                    IEnumerable<PedidoRestaurante> Pedidos = new ObservableCollection<PedidoRestaurante>(await dataService.GetEvolucao3AnosAsync());
                    if (Pedidos?.Count() > 0)
                    {
                        GdpValue[] anoatual = new GdpValue[0];
                        GdpValue[] anoant = new GdpValue[0];
                        GdpValue[] anoant1 = new GdpValue[0];
                        Pedidos.ToList()
                            .FindAll(p => p.Ano == DateTime.Now.Year)
                            .OrderBy(p=> p.Mes)
                            .ToList()
                            .ForEach(p =>
                            {
                                Array.Resize(ref anoatual, anoatual.Length + 1);
                                anoatual[anoatual.Length - 1] = new GdpValue(null, p.Mes.MesStr(), p.Vl_pedidos);
                            });
                        Pedidos.ToList()
                            .FindAll(p => p.Ano == DateTime.Now.Year - 1)
                            .OrderBy(p => p.Mes)
                            .ToList()
                            .ForEach(p =>
                            {
                                Array.Resize(ref anoant, anoant.Length + 1);
                                anoant[anoant.Length - 1] = new GdpValue(null, p.Mes.MesStr(), p.Vl_pedidos);
                            });
                        Pedidos.ToList()
                            .FindAll(p => p.Ano == DateTime.Now.Year - 2)
                            .OrderBy(p => p.Mes)
                            .ToList()
                            .ForEach(p =>
                            {
                                Array.Resize(ref anoant1, anoant1.Length + 1);
                                anoant1[anoant1.Length - 1] = new GdpValue(null, p.Mes.MesStr(), p.Vl_pedidos);
                            });
                        GdpValueAnoAtual = new CountryGdp(DateTime.Now.Year.ToString(), anoatual);
                        GdpValueAnoAnt = new CountryGdp((DateTime.Now.Year - 1).ToString(), anoant);
                        GdpValueAnoAnt1 = new CountryGdp((DateTime.Now.Year - 2).ToString(), anoant1);
                    }
                    else
                    {
                        GdpValueAnoAtual = null;
                        GdpValueAnoAnt = null;
                        GdpValueAnoAnt1 = null;
                    }
                }
        }
    }
}
