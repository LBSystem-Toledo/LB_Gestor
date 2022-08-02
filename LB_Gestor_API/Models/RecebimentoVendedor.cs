namespace LB_Gestor_API.Models
{
    public class RecebimentoVendedor
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string Vendedor { get; set; } = string.Empty;
        public decimal Vl_recebido { get; set; } = decimal.Zero;
    }
}
