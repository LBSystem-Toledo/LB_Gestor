using Prism.Mvvm;
using System.Drawing;

namespace LB_Gestor.Models
{
    public class ContaGer: BindableBase
    {
        public string Cd_contager { get; set; } = string.Empty;
        private string _ds_contager = string.Empty;
        public string Ds_contager { get { return _ds_contager; } set { SetProperty(ref _ds_contager, value); } }
        private decimal _vl_saldo = decimal.Zero;
        public decimal Vl_Saldo { get { return _vl_saldo; } set { SetProperty(ref _vl_saldo, value); } }
        public Color CorSaldo => Vl_Saldo <= decimal.Zero ? Color.Red : Color.Green;
    }
}
