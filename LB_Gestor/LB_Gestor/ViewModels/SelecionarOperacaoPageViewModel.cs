using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using LB_Gestor.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.IO;
using System.Linq;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class SelecionarOperacaoPageViewModel : ViewModelBase
    {
        private string Path { get; set; }

        private RegPDF _reg;
        public RegPDF Reg { get { return _reg; } set { SetProperty(ref _reg, value); } }

        public DelegateCommand ProvisionarDupCommand { get; }
        public DelegateCommand QuitarDuplicataCommand { get; }
        public DelegateCommand GravarCaixaCommand { get; }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public SelecionarOperacaoPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            : base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;

            ProvisionarDupCommand = new DelegateCommand(async () =>
            {
                if (File.Exists(Path))
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                        {
                            Reg = await new LerPDF(Path).LerArquivoPDF();
                            if (Reg != null)
                            {
                                Duplicata dup = new Duplicata();
                                dup.Dt_emissao = DateTime.Parse(Reg.Dt_emissao);
                                dup.Dt_vencto = DateTime.Parse(Reg.Dt_vencimento);
                                dup.LoginDup = App.token.Login;
                                dup.Nr_docto = Reg.Nr_docto;
                                dup.Tp_mov = "P";
                                dup.Vl_parcela = Reg.Vl_documento;
                                NavigationParameters param = new NavigationParameters();
                                //Buscar configuração auto
                                if (Reg.TipoDespesa != TipoDespesa.Boleto)
                                {
                                    CfgDespAuto cfg = await dataService.GetCfgDespAutoAsync(Reg.TipoDespesa == TipoDespesa.DAS ? "0" :
                                                                                            Reg.TipoDespesa == TipoDespesa.FGTS ? "1" :
                                                                                            Reg.TipoDespesa == TipoDespesa.GPS ? "2" :
                                                                                            Reg.TipoDespesa == TipoDespesa.IRPJ ? "3" :
                                                                                            Reg.TipoDespesa == TipoDespesa.IRRF ? "4" :
                                                                                            Reg.TipoDespesa == TipoDespesa.FOLHA ? "5" :
                                                                                            Reg.TipoDespesa == TipoDespesa.NFSe ? "6" : string.Empty);

                                    if (cfg != null)
                                    {
                                        if (!string.IsNullOrWhiteSpace(cfg.Cd_clifor))
                                        {
                                            var lClifor = await dataService.GetClientesAsync(cfg.Cd_clifor, string.Empty);
                                            if (lClifor.Count() > 0)
                                                param.Add("CLIENTE", lClifor.First());
                                        }
                                        if (!string.IsNullOrWhiteSpace(cfg.Cd_historico))
                                            param.Add("CD_HISTORICO", cfg.Cd_historico);
                                        if (cfg.Id_plano > 0)
                                            param.Add("ID_PLANO", cfg.Id_plano);
                                    }
                                }
                                else
                                {
                                    Cliente c = await dataService.GetFornecedorCNPJAsync(Reg.Cnpjs);
                                    if (c != null)
                                        param.Add("CLIENTE", c);
                                }
                                param.Add("DUPLICATA", dup);
                                await navigationService.NavigateAsync("NovaDupPage", param);
                            }
                            else await dialogService.DisplayAlertAsync("Mensagem", "Modelo documento não suportado.", "OK");
                        }
                    else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
                }
                else await dialogService.DisplayAlertAsync("Mensagem", "Arquivo PDF não encontrado.", "OK");
            });
            QuitarDuplicataCommand = new DelegateCommand(async () =>
            {
                if (File.Exists(Path))
                {
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                    {
                        Reg = await new LerPDF(Path).LerPDFPagamento();
                        if (Reg != null)
                        {
                            NavigationParameters param = new NavigationParameters();
                            param.Add("DATA", Reg.Dt_emissao);
                            param.Add("VALOR", Reg.Vl_documento);
                            await navigationService.NavigateAsync("QuitarDuplicataPage", param);
                        }
                        else await dialogService.DisplayAlertAsync("Mensagem", "Modelo documento não suportado.", "OK");
                    }
                }
                else await dialogService.DisplayAlertAsync("Mensagem", "Arquivo PDF não encontrado.", "OK");
            });
            GravarCaixaCommand = new DelegateCommand(async () =>
            {
                if (File.Exists(Path))
                {
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                    {
                        Reg = await new LerPDF(Path).LerPDFPagamento();
                        if (Reg != null)
                        {
                            NavigationParameters param = new NavigationParameters();
                            param.Add("DATA", Reg.Dt_emissao);
                            param.Add("VALOR", Reg.Vl_documento);
                            await navigationService.NavigateAsync("CaixaPage", param);
                        }
                        else await dialogService.DisplayAlertAsync("Mensagem", "Modelo documento não suportado.", "OK");
                    }
                }
                else await dialogService.DisplayAlertAsync("Mensagem", "Arquivo PDF não encontrado.", "OK");
            });
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Path = parameters["PATH"].ToString();
        }
    }
}
