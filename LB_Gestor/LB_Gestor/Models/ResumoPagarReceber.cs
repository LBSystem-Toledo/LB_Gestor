using Prism.Mvvm;

namespace LB_Gestor.Models
{
    public class ResumoPagarReceber: BindableBase
    {
        decimal _vl_rec_mais30dias_venc = decimal.Zero;
        public decimal Vl_rec_mais30dias_venc { get { return _vl_rec_mais30dias_venc; } set { SetProperty(ref _vl_rec_mais30dias_venc, value); } }
        decimal _vl_rec_30dias_venc = decimal.Zero;
        public decimal Vl_rec_30dias_venc { get { return _vl_rec_30dias_venc; } set { SetProperty(ref _vl_rec_30dias_venc, value); } }
        decimal _vl_rec_hoje = decimal.Zero;
        public decimal Vl_rec_hoje { get { return _vl_rec_hoje; } set { SetProperty(ref _vl_rec_hoje, value); } }
        decimal _vl_rec_30dias = decimal.Zero;
        public decimal Vl_rec_30dias { get { return _vl_rec_30dias; } set { SetProperty(ref _vl_rec_30dias, value); } }
        decimal _vl_rec_mais30dias = decimal.Zero;
        public decimal Vl_rec_mais30dias { get { return _vl_rec_mais30dias; } set { SetProperty(ref _vl_rec_mais30dias, value); } }
        public decimal Tot_receber => Vl_rec_mais30dias + Vl_rec_30dias_venc + Vl_rec_hoje + Vl_rec_mais30dias_venc + Vl_rec_30dias;
        decimal _vl_pag_mais30dias_venc = decimal.Zero;
        public decimal Vl_pag_mais30dias_venc { get { return _vl_pag_mais30dias_venc; } set { SetProperty(ref _vl_pag_mais30dias_venc, value); } }
        decimal _vl_pag_30dias_venc = decimal.Zero;
        public decimal Vl_pag_30dias_venc { get { return _vl_pag_30dias_venc; } set { SetProperty(ref _vl_pag_30dias_venc, value); } }
        decimal _vl_pag_hoje = decimal.Zero;
        public decimal Vl_pag_hoje { get { return _vl_pag_hoje; } set { SetProperty(ref _vl_pag_hoje, value); } }
        decimal _vl_pag_30dias = decimal.Zero;
        public decimal Vl_pag_30dias { get { return _vl_pag_30dias; } set { SetProperty(ref _vl_pag_30dias, value); } }
        decimal _vl_pag_mais30dias = decimal.Zero;
        public decimal Vl_pag_mais30dias { get { return _vl_pag_mais30dias; } set { SetProperty(ref _vl_pag_mais30dias, value); } }
        public decimal Tot_pagar => Vl_pag_mais30dias_venc + Vl_pag_30dias_venc + Vl_pag_hoje + Vl_pag_30dias + Vl_pag_mais30dias;
    }
}
