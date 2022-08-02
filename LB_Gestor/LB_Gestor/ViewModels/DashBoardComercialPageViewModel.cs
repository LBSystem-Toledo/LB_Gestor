using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LB_Gestor.ViewModels
{
    public class DashBoardComercialPageViewModel : ViewModelBase
    {
        public DelegateCommand KpiVendasCommand { get; }
        public DelegateCommand VendasPortadorCommand { get; }
        public DelegateCommand VendasMesCommand { get; }
        public DelegateCommand VendasAnoCommand { get; }
        public DelegateCommand VendasCidadeCommand { get; }
        public DelegateCommand VendasGrupoCommand { get; }
        public DelegateCommand VendasVendedorCommand { get; }
        public DelegateCommand RecebimentoVendedorCommand { get; }

        public DashBoardComercialPageViewModel(INavigationService navigationService)
            :base(navigationService)
        {
            KpiVendasCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("KPIFaturamentoPage"));
            VendasPortadorCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("VendasPortadorPage"));
            VendasMesCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("VendasMesPage"));
            VendasAnoCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("VendasAnoPage"));
            VendasCidadeCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("VendasCidadePage"));
            VendasGrupoCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("VendasGrupoPage"));
            VendasVendedorCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("VendasVendedoresPage"));
            RecebimentoVendedorCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("RecebimentoVendedorPage"));
        }
    }
}
