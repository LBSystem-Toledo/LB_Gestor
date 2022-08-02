namespace LB_Gestor_API.Models
{
    public class ResumoPagarReceber
    {
        public decimal Vl_rec_mais30dias_venc { get; set; } = decimal.Zero;
        public decimal Vl_rec_30dias_venc { get; set; } = decimal.Zero;
        public decimal Vl_rec_hoje { get; set; } = decimal.Zero;
        public decimal Vl_rec_30dias { get; set; } = decimal.Zero;
        public decimal Vl_rec_mais30dias { get; set; } = decimal.Zero;
        public decimal Vl_pag_mais30dias_venc { get; set; } = decimal.Zero;
        public decimal Vl_pag_30dias_venc { get; set; } = decimal.Zero;
        public decimal Vl_pag_hoje { get; set; } = decimal.Zero;
        public decimal Vl_pag_30dias { get; set; } = decimal.Zero;
        public decimal Vl_pag_mais30dias { get; set; } = decimal.Zero;
    }
}
