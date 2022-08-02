using Prism.Mvvm;

namespace LB_Gestor.Models
{
    public class RecebimentoVendedor: BindableBase
    {
        int _mes;
        public int Mes { get { return _mes; } set { SetProperty(ref _mes, value); } }
        int _ano;
        public int Ano { get { return _ano; } set { SetProperty(ref _ano, value); } }
        string _vendedor = string.Empty;
        public string Vendedor { get { return _vendedor; } set { SetProperty(ref _vendedor, value); } }
        decimal _vl_recebido = decimal.Zero;
        public decimal Vl_recebido { get { return _vl_recebido; } set { SetProperty(ref _vl_recebido, value); } }
    }
}
