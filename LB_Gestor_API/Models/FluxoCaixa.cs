using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LB_Gestor_API.Models
{
    public class FluxoCaixa
    {
        public DateTime Dt_vencto { get; set; }
        public decimal SaldoAntCC { get; set; } = decimal.Zero;
        public decimal Vl_receber { get; set; } = decimal.Zero;
        public decimal Vl_pagar { get; set; } = decimal.Zero;
        public decimal Acumulado { get; set; } = decimal.Zero;
    }
}
