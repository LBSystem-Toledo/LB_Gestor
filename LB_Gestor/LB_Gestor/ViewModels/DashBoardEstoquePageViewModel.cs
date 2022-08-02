using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Prism.Navigation;
using Prism.Services;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class DashBoardEstoquePageViewModel : ViewModelBase
    {
        KPIEstoque _kpi;
        public KPIEstoque Kpi { get { return _kpi; } set { SetProperty(ref _kpi, value); } }
        ObservableCollection<ProdutoAbaixoMinimo> _produtosabaixominimo;
        public ObservableCollection<ProdutoAbaixoMinimo> ProdutosAbaixoMinimo { get { return _produtosabaixominimo; } set { SetProperty(ref _produtosabaixominimo, value); } }
        ObservableCollection<ProdutoAbaixoMargemLucro> _produtosabaixomargemlucro;
        public ObservableCollection<ProdutoAbaixoMargemLucro> ProdutosAbaixoMargemLucro { get { return _produtosabaixomargemlucro; } set { SetProperty(ref _produtosabaixomargemlucro, value); } }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public DashBoardEstoquePageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                    {
                        Kpi = await dataService.GetKPIEstoqueAsync();
                        ProdutosAbaixoMinimo = new ObservableCollection<ProdutoAbaixoMinimo>(await dataService.GetProdutosAbaixoMinimoAsync());
                        ProdutosAbaixoMargemLucro = new ObservableCollection<ProdutoAbaixoMargemLucro>(await dataService.GetProdutosAbaixoMargemLucroAsync());
                    }
                else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
            }
            catch { }
        }
    }
}
