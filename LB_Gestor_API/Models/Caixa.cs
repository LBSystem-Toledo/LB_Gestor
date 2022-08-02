using System;

namespace LB_Gestor_API.Models
{
    public class Caixa
    {
        public string Cd_contager { get; set; } = string.Empty;
        public string Cd_historico { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string CompHistorico { get; set; } = string.Empty;
        public DateTime Dt_lancto { get; set; }
        public decimal Valor { get; set; } = decimal.Zero;
        public int Id_plano { get; set; }
    }
}
