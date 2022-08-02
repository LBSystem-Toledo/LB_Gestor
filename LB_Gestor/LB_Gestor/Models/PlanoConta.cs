using Prism.Mvvm;

namespace LB_Gestor.Models
{
    public class PlanoConta: BindableBase
    {
        public int Id_plano { get; set; }
        string _ds_conta = string.Empty;
        public string Ds_conta { get { return _ds_conta; } set { SetProperty(ref _ds_conta, value); } }
        public string Tp_registro { get; set; } = string.Empty;
    }
}
