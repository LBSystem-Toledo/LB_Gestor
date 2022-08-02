using System;

namespace LB_Gestor_API.Models
{
    public class Duplicata
    {
        public string Cd_empresa { get; set; } = string.Empty;
        public decimal Nr_lancto { get; set; }
        public decimal Cd_parcela { get; set; }
        public string Nr_docto { get; set; } = string.Empty;
        public string Cd_clifor { get; set; } = string.Empty;
        public string Cd_endereco { get; set; } = string.Empty;
        public string Nm_clifor { get; set; } = string.Empty;
        public string Cd_historico { get; set; } = string.Empty;
        public string Ds_historico { get; set; } = string.Empty;
        public string Tp_duplicata { get; set; } = string.Empty;
        public string Tp_mov { get; set; } = string.Empty;
        public DateTime Dt_emissao { get; set; }
        public DateTime Dt_vencto { get; set; }
        public decimal Vl_parcela { get; set; } = decimal.Zero;
        public decimal Vl_atual { get; set; } = decimal.Zero;
        public string Status_parcela { get; set; } = string.Empty;
        public int Id_plano { get; set; }
        public string Ds_conta { get; set; } = string.Empty;
        public string Cd_portador { get; set; } = string.Empty;
        public string Cd_contager { get; set; } = string.Empty;
        public DateTime Dt_quitar { get; set; }
        public decimal Vl_quitar { get; set; } = decimal.Zero;
        public decimal Vl_desconto { get; set; } = decimal.Zero;
        public string CompHistorico { get; set; } = string.Empty;
        public string LoginDup { get; set; } = string.Empty;
        public DateTime? Dt_prorrogacao { get; set; } = null;
    }
}
