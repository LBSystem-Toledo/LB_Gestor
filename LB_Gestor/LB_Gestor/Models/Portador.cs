using Prism.Mvvm;

namespace LB_Gestor.Models
{
    public class Portador: BindableBase
    {
        public string Cd_portador { get; set; } = string.Empty;
        string _ds_portador = string.Empty;
        public string Ds_portador { get { return _ds_portador; } set { SetProperty(ref _ds_portador, value); } }
    }
}
