using LB_Gestor.Interface;
using LB_Gestor.Models;
using Microcharts;
using Prism.Navigation;
using Prism.Services;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;

namespace LB_Gestor.ViewModels
{
    public class ResumoHistoricoPageViewModel : ViewModelBase
    {
        Chart _chartreceita;
        public Chart ChartReceita { get { return _chartreceita; } set { SetProperty(ref _chartreceita, value); } }
        Chart _chartdespesa;
        public Chart ChartDespesa { get { return _chartdespesa; } set { SetProperty(ref _chartdespesa, value); } }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public ResumoHistoricoPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            IEnumerable<ReceitaDespesa> lista = await dataService.GetReceitaDespesaAsync(4, 2022, string.Empty);
            #region RECEBER
            IEnumerable<ReceitaDespesa> lRec = lista.ToList().Where(p => p.Tp_mov.Trim().ToUpper().Equals("R"));
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
            #endregion
            #region PAGAR
            IEnumerable<ReceitaDespesa> lPag = lista.ToList().Where(p => p.Tp_mov.Trim().ToUpper().Equals("P"));
            ChartEntry[] entriP = new ChartEntry[lPag.Count()];
            index = 0;
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
    }
}
