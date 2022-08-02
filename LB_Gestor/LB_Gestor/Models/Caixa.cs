using Prism.Mvvm;
using System;

namespace LB_Gestor.Models
{
    public class Caixa: BindableBase
    {
        public string Cd_contager { get; set; } = string.Empty;
        public string Cd_historico { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        private string _comphistorico = string.Empty;
        public string CompHistorico { get { return _comphistorico; } set { SetProperty(ref _comphistorico, value); } }
        private DateTime _dt_lancto = DateTime.Now;
        public DateTime Dt_lancto { get { return _dt_lancto; } set { SetProperty(ref _dt_lancto, value); } }
        private decimal _valor = decimal.Zero;
        public decimal Valor { get { return _valor; } set { SetProperty(ref _valor, value); } }
        public int Id_plano { get; set; }
    }
}
