using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LB_Gestor.ViewModels
{
    public class DashBoardFinanceiroPageViewModel : ViewModelBase
    {
        public DelegateCommand ResumoCommand { get; }
        public DelegateCommand ResultHistoricoCommand { get; }
        public DelegateCommand RecPortadorCommand { get; }
        public DelegateCommand FluxoCaixaCommand { get; }
        public DelegateCommand DRECommand { get; }
        public DelegateCommand DuplicatasCommand { get; }

        public DashBoardFinanceiroPageViewModel(INavigationService navigationService)
            :base(navigationService)
        {
            ResumoCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("ResumoFinPage"));
            ResultHistoricoCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("ResHistoricoPage"));
            RecPortadorCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("RecebimentoPortadorPage"));
            FluxoCaixaCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("FluxoCaixaPage"));
            DRECommand = new DelegateCommand(async () => await navigationService.NavigateAsync("DREPage"));
            DuplicatasCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("DuplicatasPage"));
        }
    }
}
