using Prism.Mvvm;

namespace LB_Gestor.Models
{
    public class RecebimentoPortador: BindableBase
    {
        string _portador = string.Empty;
        public string Portador { get { return _portador; } set { SetProperty(ref _portador, value); } }
        decimal _valor = decimal.Zero;
        public decimal Valor { get { return _valor; } set { SetProperty(ref _valor, value); } }
    }
}
