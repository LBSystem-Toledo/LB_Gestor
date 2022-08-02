namespace LB_Gestor_API.Models
{
    public class PedidoRestaurante
    {
        public int DiaSemana { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string Modalidade { get; set; } = string.Empty;
        public int Qt_pedidos { get; set; } = 0;
        public decimal Vl_pedidos { get; set; } = decimal.Zero;
    }

    public class VendaGrupo
    {
        public string Cd_grupo { get; set; } = string.Empty;
        public string Ds_grupo { get; set; } = string.Empty;
        public int Quantidade { get; set; } = 0;
        public decimal Valor { get; set; } = decimal.Zero;
    }
}
