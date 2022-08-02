using LB_Gestor.Interface;
using LB_Gestor.Models;
using LB_Gestor.ViewModels;
using LB_Gestor.Views;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace LB_Gestor
{
    public partial class App
    {
        #if (DEBUG)
            public const string url_api = "http://192.168.1.11:45455";
            //public const string url_api = "http://cloud.lbsystemsoftware.com.br:33209/lbgestor";
        #else
            public const string url_api = "http://cloud.lbsystemsoftware.com.br:33209/lbgestor";
        #endif

        public static Token token { get; set; }
        public string Path_pdf { get; set; }

        public App(IPlatformInitializer initializer, string vPath_pdf)
            : base(initializer)
        {
            Path_pdf = vPath_pdf;
            if (!string.IsNullOrWhiteSpace(Path_pdf))
            {
                NavigationParameters p = new NavigationParameters();
                p.Add("PATH", Path_pdf);
                NavigationService.NavigateAsync("LoginPage", p);
            }
            else NavigationService.NavigateAsync("LoginPage");
        }

        protected override void OnInitialized()
        {
            InitializeComponent();
            DevExpress.XamarinForms.Charts.Initializer.Init();
            DevExpress.XamarinForms.DataGrid.Initializer.Init();
            DevExpress.XamarinForms.Editors.Initializer.Init();
            
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterSingleton<IDataService, DataService.DataService>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<MenuPage, MenuPageViewModel>();
            containerRegistry.RegisterForNavigation<ResumoFinPage, ResumoFinPageViewModel>();
            containerRegistry.RegisterForNavigation<ResHistoricoPage, ResHistoricoPageViewModel>();
            containerRegistry.RegisterForNavigation<FluxoCaixaPage, FluxoCaixaPageViewModel>();
            containerRegistry.RegisterForNavigation<ConsultaClientePage, ConsultaClientePageViewModel>();
            containerRegistry.RegisterForNavigation<NovoClientePage, NovoClientePageViewModel>();
            containerRegistry.RegisterForNavigation<DuplicatasPage, DuplicatasPageViewModel>();
            containerRegistry.RegisterForNavigation<ConsultaCidadePage, ConsultaCidadePageViewModel>();
            containerRegistry.RegisterForNavigation<NovaDupPage, NovaDupPageViewModel>();
            containerRegistry.RegisterForNavigation<ProrrogarVenctoPage, ProrrogarVenctoPageViewModel>();
            containerRegistry.RegisterForNavigation<QuitarPage, QuitarPageViewModel>();
            containerRegistry.RegisterForNavigation<SelecionarOperacaoPage, SelecionarOperacaoPageViewModel>();
            containerRegistry.RegisterForNavigation<CaixaPage, CaixaPageViewModel>();
            containerRegistry.RegisterForNavigation<QuitarDuplicataPage, QuitarDuplicataPageViewModel>();
            containerRegistry.RegisterForNavigation<KPIFaturamentoPage, KPIFaturamentoPageViewModel>();
            containerRegistry.RegisterForNavigation<VendasPortadorPage, VendasPortadorPageViewModel>();
            containerRegistry.RegisterForNavigation<VendasMesPage, VendasMesPageViewModel>();
            containerRegistry.RegisterForNavigation<VendasAnoPage, VendasAnoPageViewModel>();
            containerRegistry.RegisterForNavigation<VendasCidadePage, VendasCidadePageViewModel>();
            containerRegistry.RegisterForNavigation<VendasGrupoPage, VendasGrupoPageViewModel>();
            containerRegistry.RegisterForNavigation<VendasVendedoresPage, VendasVendedoresPageViewModel>();
            containerRegistry.RegisterForNavigation<RecebimentoPortadorPage, RecebimentoPortadorPageViewModel>();
            containerRegistry.RegisterForNavigation<DREPage, DREPageViewModel>();
            containerRegistry.RegisterForNavigation<PedidosHojeRestaurantePage, PedidosHojeRestaurantePageViewModel>();
            containerRegistry.RegisterForNavigation<DashBoardRestaurantePage, DashBoardRestaurantePageViewModel>();
            containerRegistry.RegisterForNavigation<PedidosOntemRestaurantePage, PedidosOntemRestaurantePageViewModel>();
            containerRegistry.RegisterForNavigation<PedidoFimSemanaRestaurantePage, PedidoFimSemanaRestaurantePageViewModel>();
            containerRegistry.RegisterForNavigation<PedidoOutrosDiasRestaurantePage, PedidoOutrosDiasRestaurantePageViewModel>();
            containerRegistry.RegisterForNavigation<DashBoardFinanceiroPage, DashBoardFinanceiroPageViewModel>();
            containerRegistry.RegisterForNavigation<DashBoardComercialPage, DashBoardComercialPageViewModel>();
            containerRegistry.RegisterForNavigation<Evolucao12MesesPage, Evolucao12MesesPageViewModel>();
            containerRegistry.RegisterForNavigation<Evolucao3AnosPage, Evolucao3AnosPageViewModel>();
            containerRegistry.RegisterForNavigation<RecebimentoVendedorPage, RecebimentoVendedorPageViewModel>();
            containerRegistry.RegisterForNavigation<DashBoardEstoquePage, DashBoardEstoquePageViewModel>();
        }
    }
}
