using Prism.Mvvm;
using System;

namespace LB_Gestor.Models
{
    public class KPIFaturamento: BindableBase
    {
        decimal _vendashoje = decimal.Zero;
        public decimal VendasHoje { get { return _vendashoje; } set { SetProperty(ref _vendashoje, value); } }
        decimal _vendasontem = decimal.Zero;
        public decimal VendasOntem { get { return _vendasontem; } set { SetProperty(ref _vendasontem, value); } }
        decimal _acumuladosemana = decimal.Zero;
        public decimal AcumuladoSemana { get { return _acumuladosemana; } set { SetProperty(ref _acumuladosemana, value); } }
        decimal _acumuladomes = decimal.Zero;
        public decimal AcumuladoMes { get { return _acumuladomes; } set { SetProperty(ref _acumuladomes, value); } }
        decimal _cmvmes = decimal.Zero;
        public decimal CmvMes { get { return _cmvmes; } set { SetProperty(ref _cmvmes, value); } }
        public decimal LucroBrutoMes => decimal.Subtract(AcumuladoMes, CmvMes);
        public decimal MargemBrutaMes => Math.Round(decimal.Multiply(decimal.Divide(LucroBrutoMes, AcumuladoMes), 100), 2, MidpointRounding.ToEven);
        public decimal VendaMesFiscal { get; set; } = decimal.Zero;
        public decimal VendaMesNaoFiscal => decimal.Subtract(AcumuladoMes, VendaMesFiscal);
        public decimal MargemFiscal => Math.Round(decimal.Multiply(decimal.Divide(VendaMesFiscal, AcumuladoMes), 100), 2, MidpointRounding.ToEven);
    }
}
