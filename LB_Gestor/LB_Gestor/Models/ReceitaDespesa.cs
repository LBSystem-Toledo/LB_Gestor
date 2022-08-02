using LB_Gestor.Utils;
using Prism.Mvvm;

namespace LB_Gestor.Models
{
    public class ReceitaDespesa: BindableBase
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string MesAno
        {
            get
            {
                if (Mes > 0 && Ano > 0)
                    return Mes.MesStr() + "/" + Ano.ToString();
                else return string.Empty;
            }
        }
        public string Tp_mov { get; set; } = string.Empty;
        string _ds_historico = string.Empty;
        public string Ds_historico { get { return _ds_historico; } set { SetProperty(ref _ds_historico, value); } }
        decimal _valor = decimal.Zero;
        public decimal Valor { get { return _valor; } set { SetProperty(ref _valor, value); } }
    }
}
