using Prism.Mvvm;

namespace LB_Gestor.Models
{
    public class Historico: BindableBase
    {
        public string Cd_historico { get; set; } = string.Empty;
        string _ds_historico = string.Empty;
        public string Ds_historico { get { return _ds_historico; } set { SetProperty(ref _ds_historico, value); } }
        public string Tp_mov { get; set; } = string.Empty;
    }
}
