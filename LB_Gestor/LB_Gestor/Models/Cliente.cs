using LB_Gestor.Utils;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace LB_Gestor.Models
{
    public class Cliente: BindableBase
    {
        string _cd_clifor = string.Empty;
        public string Cd_clifor { get { return _cd_clifor; } set { SetProperty(ref _cd_clifor, value); } }
        string _cd_cidade = string.Empty;
        public string Cd_cidade { get { return _cd_cidade; } set { SetProperty(ref _cd_cidade, value); } }
        string _ds_cidade = string.Empty;
        public string Ds_cidade { get { return _ds_cidade; } set { SetProperty(ref _ds_cidade, value); } }
        string _uf = string.Empty;
        public string UF { get { return _uf; } set { SetProperty(ref _uf, value); } }
        string _nm_clifor = string.Empty;
        public string Nm_clifor { get { return _nm_clifor; } set { SetProperty(ref _nm_clifor, value); } }
        string _nm_fantasia = string.Empty;
        public string Nm_fantasia { get { return _nm_fantasia; } set { SetProperty(ref _nm_fantasia, value); } }
        string _nr_cgc = string.Empty;
        public string Nr_cgc { get { return _nr_cgc; } set { SetProperty(ref _nr_cgc, value); } }
        string _nr_cpf = string.Empty;
        public string Nr_cpf { get { return _nr_cpf; } set { SetProperty(ref _nr_cpf, value); } }
        string _cd_endereco = string.Empty;
        public string Cd_endereco { get { return _cd_endereco; } set { SetProperty(ref _cd_endereco, value); } }
        string _cep = string.Empty;
        public string Cep { get { return _cep; } set { SetProperty(ref _cep, value); } }
        string _ds_endereco = string.Empty;
        public string Ds_endereco { get { return _ds_endereco; } set { SetProperty(ref _ds_endereco, value); } }
        string _numero = string.Empty;
        public string Numero { get { return _numero; } set { SetProperty(ref _numero, value); } }
        string _bairro = string.Empty;
        public string Bairro { get { return _bairro; } set { SetProperty(ref _bairro, value); } }
        public int? Id_categoriaclifor { get; set; } = null;
        public string CnpjCpf { get { return Nr_cpf.SoNumero().Length.Equals(14) ? Nr_cgc.FormatarCNPJ() : Nr_cpf.FormatarCPF(); } }
    }
}
