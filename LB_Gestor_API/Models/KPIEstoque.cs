namespace LB_Gestor_API.Models
{
    public class KPIEstoque
    {
        public decimal ValorEstoque { get; set; } = decimal.Zero;
        public int ProdutosAbaixoMinimo { get; set; }
        public int ProdutosAbaixoMargemLucro { get; set; }
    }

    public class ProdutoAbaixoMinimo
    {
        public string Ds_produto { get; set; } = string.Empty;
        public decimal Tot_saldo { get; set; } = decimal.Zero;
        public decimal Qt_min_estoque { get; set; } = decimal.Zero;
    }

    public class ProdutoAbaixoMargemLucro
    {
        public string Ds_tabelapreco { get; set; } = string.Empty;
        public string Ds_produto { get; set; } = string.Empty;
        public decimal Vl_precovenda { get; set; } = decimal.Zero;
        public decimal UltimaCompra { get; set; } = decimal.Zero;
    }
}
