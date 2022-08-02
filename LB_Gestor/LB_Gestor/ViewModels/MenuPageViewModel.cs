using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;

namespace LB_Gestor.ViewModels
{
    public class MenuPageViewModel : ViewModelBase
    {
        string _login = string.Empty;
        public string Login { get { return _login.ToUpper(); } set { SetProperty(ref _login, value); } }
        
        public DelegateCommand ComercialCommand { get; }
        public DelegateCommand FinanceiroCommand { get; }
        public DelegateCommand RestauranteCommand { get; }
        public DelegateCommand EstoqueCommand { get; }

        readonly IPageDialogService dialogService;
        public MenuPageViewModel(INavigationService navigationService, IPageDialogService _dialogService)
            :base(navigationService)
        {
            dialogService = _dialogService;
            Login = App.token.Login;
                        
            ComercialCommand = new DelegateCommand(async () => await navigationService.NavigateAsync(new Uri("/MenuPage/NavigationPage/DashBoardComercialPage", System.UriKind.Relative)));
            FinanceiroCommand = new DelegateCommand(async () => await navigationService.NavigateAsync(new Uri("/MenuPage/NavigationPage/DashBoardFinanceiroPage", System.UriKind.Relative)));
            RestauranteCommand = new DelegateCommand(async () => await navigationService.NavigateAsync(new Uri("/MenuPage/NavigationPage/DashBoardRestaurantePage", System.UriKind.Relative)));
            EstoqueCommand = new DelegateCommand(async () => await navigationService.NavigateAsync(new Uri("/MenuPage/NavigationPage/DashBoardEstoquePage", System.UriKind.Relative)));
        }
    }
}
