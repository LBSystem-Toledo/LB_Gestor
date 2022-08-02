using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class ProrrogarVenctoPageViewModel : ViewModelBase
    {
        Duplicata _duplicata;
        public Duplicata Duplicata { get { return _duplicata; } set { SetProperty(ref _duplicata, value); } }

        public DelegateCommand ConfirmarCommand { get; }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public ProrrogarVenctoPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;

            ConfirmarCommand = new DelegateCommand(async () =>
            {
                if (Duplicata.Dt_prorrogacao.Value.Date <= Duplicata.Dt_vencto.Date)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Data prorrogação deve ser maior que data vencimento atual.", "OK");
                    return;
                }
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                        try
                        {
                            await dataService.ProrrogarVenctoAsync(Duplicata);
                            await dialogService.DisplayAlertAsync("Mensagem", "Vencimento prorrogado com sucesso.", "OK");
                            await navigationService.GoBackAsync();
                        }
                        catch (Exception ex)
                        { await dialogService.DisplayAlertAsync("Erro", ex.Message.Trim(), "OK"); }
                else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
            });
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Duplicata = parameters["DUPLICATA"] as Duplicata;
            Duplicata.Dt_prorrogacao = Duplicata.Dt_vencto;
        }
    }
}
