using Prism.Mvvm;
using System;

namespace LB_Gestor.Models
{
    public class FluxoCaixa: BindableBase
    {
        DateTime _dt_vencto;
        public DateTime Dt_vencto { get { return _dt_vencto; } set { SetProperty(ref _dt_vencto, value); } }
        decimal _saldoantcc = decimal.Zero;
        public decimal SaldoAntCC { get { return _saldoantcc; } set { SetProperty(ref _saldoantcc, value); } }
        decimal _vl_receber = decimal.Zero;
        public decimal Vl_receber { get { return _vl_receber; } set { SetProperty(ref _vl_receber, value); } }
        decimal _vl_pagar = decimal.Zero;
        public decimal Vl_pagar { get { return _vl_pagar; } set { SetProperty(ref _vl_pagar, value); } }
        decimal _acumulado = decimal.Zero;
        public decimal Acumulado { get { return _acumulado; } set { SetProperty(ref _acumulado, value); } }
        public decimal SaldoProjetado => SaldoAntCC + Acumulado;
    }
}
