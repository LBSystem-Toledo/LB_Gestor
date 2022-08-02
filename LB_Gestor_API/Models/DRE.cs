using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LB_Gestor_API.Models
{
    public class DRE
    {
        public int Id_contaDRE { get; set; }
        public string Classificacao { get; set; } = string.Empty;
        public string Ds_contaDRE { get; set; } = string.Empty;
        public string Tp_conta { get; set; } = string.Empty;
        public string Operador { get; set; } = string.Empty;
        public int Nivel { get; set; }
        public decimal Vl_janeiro { get; set; } = decimal.Zero;
        public decimal Vl_fevereiro { get; set; } = decimal.Zero;
        public decimal Vl_marco { get; set; } = decimal.Zero;
        public decimal Vl_abril { get; set; } = decimal.Zero;
        public decimal Vl_maio { get; set; } = decimal.Zero;
        public decimal Vl_junho { get; set; } = decimal.Zero;
        public decimal Vl_julho { get; set; } = decimal.Zero;
        public decimal Vl_agosto { get; set; } = decimal.Zero;
        public decimal Vl_setembro { get; set; } = decimal.Zero;
        public decimal Vl_outubro { get; set; } = decimal.Zero;
        public decimal Vl_novembro { get; set; } = decimal.Zero;
        public decimal Vl_dezembro { get; set; } = decimal.Zero;
    }
}
