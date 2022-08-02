using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using LB_Gestor.Utils;
using LBComandaPrism.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LB_Gestor.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private string Path { get; set; }

        private Usuario _user;
        public Usuario User { get { return _user; } set { SetProperty(ref _user, value); } }

        public DelegateCommand LoginCommand { get; }
        public DelegateCommand SairCommand { get; }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public LoginPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;
            User = new Usuario();

            LoginCommand = new DelegateCommand(async() =>
            {
                if (string.IsNullOrWhiteSpace(User.Login))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar LOGIN.", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(User.Senha))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar SENHA.", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(User.Cnpj))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar CNPJ.", "OK");
                    return;
                }
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                    {
                        Token tk = await dataService.ValidarLoginAsync(User);
                        if (tk == null ? true : string.IsNullOrWhiteSpace(tk.access_token))
                        {
                            await dialogService.DisplayAlertAsync("Mensagem", "Não foi possivel validar USUARIO.", "OK");
                            return;
                        }
                        tk.Login = User.Login;
                        tk.Cnpj = User.Cnpj.SoNumero();
                        App.token = tk;
                        await PCLHelper.WriteTextAllAsync("HistoricoLogin", User.Login + ":" + User.Senha + ":" + User.Cnpj);
                        if (string.IsNullOrWhiteSpace(Path))
                            await navigationService.NavigateAsync(new Uri("/MenuPage/NavigationPage/ResumoFinPage", System.UriKind.Relative));
                        else
                        {
                            NavigationParameters p = new NavigationParameters();
                            p.Add("PATH", Path);
                            await NavigationService.NavigateAsync("SelecionarOperacaoPage", p);
                        }
                    }
                else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
            });
            SairCommand = new DelegateCommand(() => DependencyService.Get<ICloseApplication>()?.closeApplication());
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("PATH"))
                Path = parameters["PATH"].ToString();
            if(await PCLHelper.ArquivoExisteAsync("HistoricoLogin"))
            {
                string aux = await PCLHelper.ReadAllTextAsync("HistoricoLogin");
                string[] vetor = aux.Split(new char[] { ':' });
                if (vetor.Length > 0)
                {
                    User.Login = aux.Split(new char[] { ':' })[0];
                    if (vetor.Length > 1)
                        User.Senha = aux.Split(new char[] { ':' })[1];
                    if (vetor.Length > 2)
                        User.Cnpj = aux.Split(new char[] { ':' })[2];
                }
            }
            if(!string.IsNullOrWhiteSpace(Path) &&
                !string.IsNullOrWhiteSpace(User.Login) &&
                !string.IsNullOrWhiteSpace(User.Senha) &&
                !string.IsNullOrWhiteSpace(User.Cnpj))
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                    {
                        Token tk = await dataService.ValidarLoginAsync(User);
                        if (tk != null)
                        {
                            tk.Login = User.Login;
                            tk.Cnpj = User.Cnpj.SoNumero();
                            App.token = tk;
                            NavigationParameters p = new NavigationParameters();
                            p.Add("PATH", Path);
                            await NavigationService.NavigateAsync("SelecionarOperacaoPage", p);
                        }
                    }
                else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
            }
        }
    }
}
