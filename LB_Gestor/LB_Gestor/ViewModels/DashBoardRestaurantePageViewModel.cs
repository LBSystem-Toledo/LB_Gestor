using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LB_Gestor.ViewModels
{
    public class DashBoardRestaurantePageViewModel : ViewModelBase
    {
        public DelegateCommand PedidoHojeCommand { get; }
        public DelegateCommand PedidoOntemCommand { get; }
        public DelegateCommand PedidoFimSemanaCommand { get; }
        public DelegateCommand PedidoOutrosDiasCommand { get; }
        public DelegateCommand EvolucaoMesCommand { get; }
        public DelegateCommand EvolucaoAnoCommand { get; }

        public DashBoardRestaurantePageViewModel(INavigationService navigationService)
            :base(navigationService)
        {
            PedidoHojeCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("PedidosHojeRestaurantePage"));
            PedidoOntemCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("PedidosOntemRestaurantePage"));
            PedidoFimSemanaCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("PedidoFimSemanaRestaurantePage"));
            PedidoOutrosDiasCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("PedidoOutrosDiasRestaurantePage"));
            EvolucaoMesCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("Evolucao12MesesPage"));
            EvolucaoAnoCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("Evolucao3AnosPage"));
        }
    }
}
