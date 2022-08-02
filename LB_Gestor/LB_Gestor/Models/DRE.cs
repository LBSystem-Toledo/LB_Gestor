using Prism.Mvvm;

namespace LB_Gestor.Models
{
    public class DRE: BindableBase
    {
        string _classificacao = string.Empty;
        public string Classificacao { get { return _classificacao; } set { SetProperty(ref _classificacao, value); } }
        string _ds_contadre = string.Empty;
        public string Ds_contaDRE { get { return _ds_contadre; } set { SetProperty(ref _ds_contadre, value); } }
        string _tp_conta = string.Empty;
        public string Tp_conta { get { return _tp_conta; } set { SetProperty(ref _tp_conta, value); } }
        string _operador = string.Empty;
        public string Operador { get { return _operador; } set { SetProperty(ref _operador, value); } }
        int _nivel;
        public int Nivel { get { return _nivel; } set { SetProperty(ref _nivel, value); } }
        decimal _vl_janeiro = decimal.Zero;
        public decimal Vl_janeiro { get { return _vl_janeiro; } set { SetProperty(ref _vl_janeiro, value); } }
        decimal _vl_fevereiro = decimal.Zero;
        public decimal Vl_fevereiro { get { return _vl_fevereiro; } set { SetProperty(ref _vl_fevereiro, value); } }
        decimal _vl_marco = decimal.Zero;
        public decimal Vl_marco { get { return _vl_marco; } set { SetProperty(ref _vl_marco, value); } }
        decimal _vl_abril = decimal.Zero;
        public decimal Vl_abril { get { return _vl_abril; } set { SetProperty(ref _vl_abril, value); } }
        decimal _vl_maio = decimal.Zero;
        public decimal Vl_maio { get { return _vl_maio; } set { SetProperty(ref _vl_maio, value); } }
        decimal _vl_junho = decimal.Zero;
        public decimal Vl_junho { get { return _vl_junho; } set { SetProperty(ref _vl_junho, value); } }
        decimal _vl_julho = decimal.Zero;
        public decimal Vl_julho { get { return _vl_julho; } set { SetProperty(ref _vl_julho, value); } }
        decimal _vl_agosto = decimal.Zero;
        public decimal Vl_agosto { get { return _vl_agosto; } set { SetProperty(ref _vl_agosto, value); } }
        decimal _vl_setembro = decimal.Zero;
        public decimal Vl_setembro { get { return _vl_setembro; } set { SetProperty(ref _vl_setembro, value); } }
        decimal _vl_outubro = decimal.Zero;
        public decimal Vl_outubro { get { return _vl_outubro; } set { SetProperty(ref _vl_outubro, value); } }
        decimal _vl_novembro = decimal.Zero;
        public decimal Vl_novembro { get { return _vl_novembro; } set { SetProperty(ref _vl_novembro, value); } }
        decimal _vl_dezembro = decimal.Zero;
        public decimal Vl_dezembro { get { return _vl_dezembro; } set { SetProperty(ref _vl_dezembro, value); } }
        public decimal Total_exercicio =>
            Vl_janeiro + Vl_fevereiro + Vl_marco + Vl_abril +
            Vl_maio + Vl_junho + Vl_julho + Vl_agosto +
            Vl_setembro + Vl_outubro + Vl_novembro + Vl_dezembro;
    }
}
