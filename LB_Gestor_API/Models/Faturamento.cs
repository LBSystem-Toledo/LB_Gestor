namespace LB_Gestor_API.Models
{
    public class Faturamento
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string Ds_cidade { get; set; } = string.Empty;
        public string Ds_grupo { get; set; } = string.Empty;
        public string Vendedor { get; set; } = string.Empty;
        public decimal Vl_venda { get; set; } = decimal.Zero;
    }
}
