using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Microcharts;
using Prism.Navigation;
using Prism.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class ResHistoricoPageViewModel : ViewModelBase
    {
        DateTime _dtreceita = DateTime.Now;
        public DateTime DtReceita 
        { 
            get { return _dtreceita; } 
            set 
            {
                SetProperty(ref _dtreceita, value);
                BuscarReceitaAsync();
            } 
        }
        decimal _receita;
        public decimal Receita { get { return _receita; } set { SetProperty(ref _receita, value); } }
        DateTime _dtdespesa = DateTime.Now;
        public DateTime DtDespesa 
        { 
            get { return _dtdespesa; } 
            set 
            {
                SetProperty(ref _dtdespesa, value);
                BuscarDespesaAsync();
            } 
        }
        decimal _despesa;
        public decimal Despesa { get { return _despesa; } set { SetProperty(ref _despesa, value); } }
        Chart _chartreceita;
        public Chart ChartReceita { get { return _chartreceita; } set { SetProperty(ref _chartreceita, value); } }
        Chart _chartdespesa;
        public Chart ChartDespesa { get { return _chartdespesa; } set { SetProperty(ref _chartdespesa, value); } }
        decimal _resultado = decimal.Zero;
        public decimal Resultado { get { return _resultado; } set { SetProperty(ref _resultado, value); } }
        Chart _charresultado;
        public Chart ChartResultado { get { return _charresultado; } set { SetProperty(ref _charresultado, value); } }
        ObservableCollection<Registro> _anos;
        public ObservableCollection<Registro> Anos { get { return _anos; } set { SetProperty(ref _anos, value); } }
        Registro _anocorrente;
        public Registro AnoCorrente 
        { 
            get { return _anocorrente; } 
            set 
            { 
                SetProperty(ref _anocorrente, value);
                ResultadoAsync();
            } 
        }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public ResHistoricoPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;
            
            Anos = new ObservableCollection<Registro>();
            for (int i = 0; i < 10; i++)
                Anos.Add(new Registro { Chave = DateTime.Now.AddYears(i * -1).Year.ToString(), Valor = DateTime.Now.AddYears(i * -1).Year.ToString() });
            AnoCorrente = Anos.FirstOrDefault();
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            await ResultadoAsync();
        }

        private async Task BuscarReceitaAsync()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                {
                    IEnumerable<ReceitaDespesa> lRec = await dataService.GetReceitaDespesaAsync(DtReceita.Month, DtReceita.Year, "R");
                    Receita = lRec.Sum(p => p.Valor);
                    ChartEntry[] entri = new ChartEntry[lRec.Count()];
                    int index = 0;
                    lRec.OrderBy(p => p.Valor)
                        .ToList()
                        .ForEach(p =>
                        {
                            entri[index++] = new ChartEntry((float)p.Valor)
                            {
                                Label = p.Ds_historico,
                                ValueLabel = p.Valor.ToString("C"),
                                Color = SKColor.Parse("#00FF00")
                            };
                        });
                    ChartReceita = new BarChart
                    {
                        Entries = entri
                    };
                }
            else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
        }
        private async Task BuscarDespesaAsync()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                {
                    IEnumerable<ReceitaDespesa> lPag = await dataService.GetReceitaDespesaAsync(DtDespesa.Month, DtDespesa.Year, "P");
                    #region PAGAR
                    Despesa = lPag.Sum(p => p.Valor);
                    ChartEntry[] entriP = new ChartEntry[lPag.Count()];
                    int index = 0;
                    lPag.OrderBy(p => p.Valor)
                        .ToList()
                        .ForEach(p =>
                        {
                            entriP[index++] = new ChartEntry((float)p.Valor)
                            {
                                Label = p.Ds_historico,
                                ValueLabel = p.Valor.ToString("C"),
                                Color = SKColor.Parse("#FF0000")
                            };
                        });
                    ChartDespesa = new BarChart
                    {
                        Entries = entriP
                    };
                    #endregion
                }
            else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
        }
        private async Task ResultadoAsync()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                {
                    IEnumerable<ReceitaDespesa> lRes = await dataService.GetResultadoAnoAsync(int.Parse(AnoCorrente.Chave));
                    Resultado = lRes.Sum(p => p.Valor);
                    ChartEntry[] entriP = new ChartEntry[lRes.Count()];
                    int index = 0;
                    lRes.ToList()
                        .ForEach(p =>
                        {
                            entriP[index++] = new ChartEntry((float)p.Valor)
                            {
                                Label = p.MesAno,
                                ValueLabel = p.Valor.ToString("C"),
                                Color = p.Valor > decimal.Zero ? SKColor.Parse("#00FF00") : SKColor.Parse("#FF0000")
                            };
                        });
                    ChartResultado = new BarChart
                    {
                        Entries = entriP
                    };
                }
            else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
        }
    }
}
