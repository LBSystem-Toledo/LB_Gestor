namespace LB_Gestor.Models
{
    public enum TipoDespesa { DAS, FGTS, GPS, IRPJ, IRRF, FOLHA, NFSe, Boleto };
    public class RegPDF
    {
        public string Cnpj { get; set; } = string.Empty;
        public string Cnpjs { get; set; } = string.Empty;
        public string Nr_docto { get; set; } = string.Empty;
        public decimal Vl_documento { get; set; } = decimal.Zero;
        public string Dt_emissao { get; set; } = string.Empty;
        public string Dt_vencimento { get; set; } = string.Empty;
        public string Cod_barra { get; set; } = string.Empty;
        public TipoDespesa TipoDespesa { get; set; }
    }
}
