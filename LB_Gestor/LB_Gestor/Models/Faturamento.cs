using LB_Gestor.Utils;
using Prism.Mvvm;

namespace LB_Gestor.Models
{
    public class Faturamento: BindableBase
    {
        int _mes;
        public int Mes { get { return _mes; } set { SetProperty(ref _mes, value); } }
        public string MesStr => Mes.MesStr();
        int _ano;
        public int Ano { get { return _ano; } set { SetProperty(ref _ano, value); } }
        public string MesAno => MesStr + "/" + Ano.ToString();
        string _ds_cidade = string.Empty;
        public string Ds_cidade { get { return _ds_cidade; } set { SetProperty(ref _ds_cidade, value); } }
        string _ds_grupo = string.Empty;
        public string Ds_grupo { get { return _ds_grupo; } set { SetProperty(ref _ds_grupo, value); } }
        string _vendedor = string.Empty;
        public string Vendedor { get { return _vendedor; } set { SetProperty(ref _vendedor, value); } }
        decimal _vl_venda = decimal.Zero;
        public decimal Vl_venda { get { return _vl_venda; } set { SetProperty(ref _vl_venda, value); } }
    }
}
