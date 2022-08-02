using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace LB_Gestor.Models
{
    public class KPIEstoque: BindableBase
    {
        decimal _valorestoque = decimal.Zero;
        public decimal ValorEstoque { get { return _valorestoque; } set { SetProperty(ref _valorestoque, value); } }
        int _produtosabaixominimo = 0;
        public int ProdutosAbaixoMinimo { get { return _produtosabaixominimo; } set { SetProperty(ref _produtosabaixominimo, value); } }
        int _produtosabaixomargemlucro = 0;
        public int ProdutosAbaixoMargemLucro { get { return _produtosabaixomargemlucro; } set { SetProperty(ref _produtosabaixomargemlucro, value); } }
    }

    public class ProdutoAbaixoMinimo: BindableBase
    {
        string _ds_produto = string.Empty;
        public string Ds_produto { get { return _ds_produto; } set { SetProperty(ref _ds_produto, value); } }
        decimal _tot_saldo = decimal.Zero;
        public decimal Tot_saldo { get { return _tot_saldo; } set { SetProperty(ref _tot_saldo, value); } }
        decimal _qt_min_estoque = decimal.Zero;
        public decimal Qt_min_estoque { get { return _qt_min_estoque; } set { SetProperty(ref _qt_min_estoque, value); } }
    }

    public class ProdutoAbaixoMargemLucro: BindableBase
    {
        string _ds_tabelapreco = string.Empty;
        public string Ds_tabelapreco { get { return _ds_tabelapreco; } set { SetProperty(ref _ds_tabelapreco, value); } }
        string _ds_produto = string.Empty;
        public string Ds_produto { get { return _ds_produto; } set { SetProperty(ref _ds_produto, value); } }
        decimal _vl_precovenda = decimal.Zero;
        public decimal Vl_precovenda { get { return _vl_precovenda; } set { SetProperty(ref _vl_precovenda, value); } }
        decimal _ultimacompra = decimal.Zero;
        public decimal UltimaCompra { get { return _ultimacompra; } set { SetProperty(ref _ultimacompra, value); } }
        public decimal Margem => Math.Round(decimal.Subtract(100, decimal.Multiply(decimal.Divide(UltimaCompra, Vl_precovenda), 100)), 2, MidpointRounding.ToEven);
    }
}
