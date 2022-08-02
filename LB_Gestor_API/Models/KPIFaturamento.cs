namespace LB_Gestor_API.Models
{
    public class KPIFaturamento
    {
        public decimal VendasHoje { get; set; } = decimal.Zero;
        public decimal VendasOntem { get; set; } = decimal.Zero;
        public decimal AcumuladoSemana { get; set; } = decimal.Zero;
        public decimal AcumuladoMes { get; set; } = decimal.Zero;
        public decimal CmvMes { get; set; } = decimal.Zero;
        public decimal VendaMesFiscal { get; set; } = decimal.Zero;
    }
}
