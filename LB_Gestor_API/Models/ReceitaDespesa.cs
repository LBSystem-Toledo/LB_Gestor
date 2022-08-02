namespace LB_Gestor_API.Models
{
    public class ReceitaDespesa
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string Tp_mov { get; set; } = string.Empty;
        public string Ds_historico { get; set; } = string.Empty;
        public decimal Valor { get; set; } = decimal.Zero;
    }
}
