using Prism.Mvvm;
using System;

namespace LB_Gestor.Models
{
    public class Duplicata: BindableBase
    {
        public string Cd_empresa { get; set; } = string.Empty;
        public decimal Nr_lancto { get; set; }
        public decimal Cd_parcela { get; set; }
        string _nr_docto = string.Empty;
        public string Nr_docto { get { return _nr_docto; } set { SetProperty(ref _nr_docto, value); } }
        public string Cd_clifor { get; set; } = string.Empty;
        public string Cd_endereco { get; set; } = string.Empty;
        string _nm_clifor = string.Empty;
        public string Nm_clifor { get { return _nm_clifor; } set { SetProperty(ref _nm_clifor, value); } }
        public string Cd_historico { get; set; } = string.Empty;
        string _ds_historico = string.Empty;
        public string Ds_historico { get { return _ds_historico; } set { SetProperty(ref _ds_historico, value); } }
        public string Tp_duplicata { get; set; } = string.Empty;
        string _tp_mov = string.Empty;
        public string Tp_mov { get { return _tp_mov; } set { SetProperty(ref _tp_mov, value); } }
        DateTime _dt_emissao = DateTime.Now;
        public DateTime Dt_emissao { get { return _dt_emissao; } set { SetProperty(ref _dt_emissao, value); } }
        DateTime _dt_vencto = DateTime.Now;
        public DateTime Dt_vencto { get { return _dt_vencto; } set { SetProperty(ref _dt_vencto, value); } }
        decimal _vl_parcela = decimal.Zero;
        public decimal Vl_parcela { get { return _vl_parcela; } set { SetProperty(ref _vl_parcela, value); } }
        decimal _vl_atual = decimal.Zero;
        public decimal Vl_atual { get { return _vl_atual; } set { SetProperty(ref _vl_atual, value); } }
        string _status_parcela = string.Empty;
        public string Status_parcela { get { return _status_parcela; } set { SetProperty(ref _status_parcela, value); } }
        public int Id_plano { get; set; }
        string _ds_conta = string.Empty;
        public string Ds_conta { get { return _ds_conta; } set { SetProperty(ref _ds_conta, value); } }
        public string Cd_portador { get; set; } = string.Empty;
        public string Cd_contager { get; set; } = string.Empty;
        private DateTime _dt_quitar;
        public DateTime Dt_quitar { get { return _dt_quitar; } set { SetProperty(ref _dt_quitar, value); } }
        private decimal _vl_quitar = decimal.Zero;
        public decimal Vl_quitar { get { return _vl_quitar; } set { SetProperty(ref _vl_quitar, value); } }
        private decimal _vl_desconto = decimal.Zero;
        public decimal Vl_desconto { get { return _vl_desconto; } set { SetProperty(ref _vl_desconto, value); } }
        string _comphistorico = string.Empty;
        public string CompHistorico { get { return _comphistorico; } set { SetProperty(ref _comphistorico, value); } }
        public string LoginDup { get; set; } = string.Empty;
        private DateTime? _dt_prorrogacao = null;
        public DateTime? Dt_prorrogacao { get { return _dt_prorrogacao; } set { SetProperty(ref _dt_prorrogacao, value); } }
    }
}
