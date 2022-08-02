using LB_Gestor.Utils;
using Prism.Mvvm;

namespace LB_Gestor.Models
{
    public class PedidoRestaurante : BindableBase
    {
        int _mes;
        public int Mes { get { return _mes; } set { SetProperty(ref _mes, value); } }
        int _ano;
        public int Ano { get { return _ano; } set { SetProperty(ref _ano, value); } }
        public string MesAno => Mes.MesStr() + "/" + Ano.ToString();
        int _diasemana;
        public int DiaSemana { get { return _diasemana; } set { SetProperty(ref _diasemana, value); } }
        public string DiaStr
        {
            get
            {
                switch(DiaSemana)
                {
                    case 1: return "Domingo";
                    case 2: return "Segunda";
                    case 3: return "Terça";
                    case 4: return "Quarta";
                    case 5: return "Quinta";
                    case 6: return "Sexta";
                    case 7: return "Sabado";
                    default: return string.Empty;
                }
            }
        }
        string _modalidade = string.Empty;
        public string Modalidade { get { return _modalidade; } set { SetProperty(ref _modalidade, value); } }
        int _qt_pedidos = 0;
        public int Qt_pedidos { get { return _qt_pedidos; } set { SetProperty(ref _qt_pedidos, value); } }
        decimal _vl_pedidos = decimal.Zero;
        public decimal Vl_pedidos { get { return _vl_pedidos; } set { SetProperty(ref _vl_pedidos, value); } }
    }

    public class VendaGrupo: BindableBase
    {
        string _cd_grupo = string.Empty;
        public string Cd_grupo { get { return _cd_grupo; } set { SetProperty(ref _cd_grupo, value); } }
        string _ds_grupo = string.Empty;
        public string Ds_grupo { get { return _ds_grupo; } set { SetProperty(ref _ds_grupo, value); } }
        int _quantidade = 0;
        public int Quantidade { get { return _quantidade; } set { SetProperty(ref _quantidade, value); } }
        decimal _valor = decimal.Zero;
        public decimal Valor { get { return _valor; } set { SetProperty(ref _valor, value); } }
    }
}
