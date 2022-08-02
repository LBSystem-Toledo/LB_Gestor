using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using LB_Gestor.Models;
using LB_Gestor.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LB_Gestor.Services
{
    public class LerPDF
    {
        readonly string _path;
        public LerPDF(string path) { _path = path; }

        public async Task<RegPDF> LerArquivoPDF()
        {
            RegPDF ret = await Task.Run(() =>
                            {
                                RegPDF reg = null;
                                using (PdfReader reader = new PdfReader(_path))
                                {
                                    using (PdfDocument doc = new PdfDocument(reader))
                                    {
                                        string texto = PdfTextExtractor.GetTextFromPage(doc.GetPage(1), new SimpleTextExtractionStrategy());
                                        string[] linhas = texto.Split('\n');
                                        if (linhas[2].Trim().ToUpper().Equals("GRF - GUIA DE RECOLHIMENTO DO FGTS"))
                                        {
                                            reg = new RegPDF();
                                            reg.Cnpj = linhas[13];
                                            reg.Nr_docto = linhas[17].Split(' ')[0];
                                            reg.Vl_documento = decimal.Parse(linhas[25]);
                                            string aux = linhas[17].Split(' ')[0];
                                            reg.Dt_emissao = new DateTime(int.Parse(aux.Split('/')[1]), int.Parse(aux.Split('/')[0]), DateTime.DaysInMonth(int.Parse(aux.Split('/')[1]), int.Parse(aux.Split('/')[0]))).ToString("dd/MM/yyyy");
                                            reg.Dt_vencimento = linhas[17].Split(' ')[1];
                                            reg.Cod_barra = linhas[0].Trim().Substring(0, 11) +
                                                        linhas[0].Trim().Substring(12, 11) +
                                                        linhas[0].Trim().Substring(24, 11) +
                                                        linhas[0].Trim().Substring(36, 11);
                                            reg.TipoDespesa = TipoDespesa.FGTS;
                                        }
                                        else if (linhas[0].Trim().ToUpper().Equals("DOCUMENTO DE ARRECADAÇÃO") &&
                                            linhas[1].Trim().ToUpper().Equals("DE RECEITAS FEDERAIS"))
                                        {
                                            if (texto.Contains("IRRF"))
                                            {
                                                reg = new RegPDF();
                                                reg.Cnpj = linhas[2].Substring(0, 18);
                                                reg.Nr_docto = linhas[12].Split(' ')[0].Substring(3, 7);
                                                reg.Vl_documento = decimal.Parse(linhas[10]);
                                                reg.Dt_emissao = reg.Nr_docto;
                                                reg.Dt_vencimento = linhas[6];
                                                reg.Cod_barra = linhas[22].SoNumero();
                                                reg.TipoDespesa = TipoDespesa.IRRF;
                                            }
                                            else
                                            {
                                                reg = new RegPDF();
                                                reg.Cnpj = linhas[2].Substring(0, 18);
                                                reg.Nr_docto = linhas[12].Split(' ')[0];
                                                reg.Vl_documento = decimal.Parse(linhas[10]);
                                                reg.Dt_emissao = new DateTime(int.Parse(reg.Nr_docto.Split('/')[1]), reg.Nr_docto.Split('/')[0].MesInt(), DateTime.DaysInMonth(int.Parse(reg.Nr_docto.Split('/')[1]), reg.Nr_docto.Split('/')[0].MesInt())).ToString("dd/MM/yyyy");
                                                reg.Dt_vencimento = linhas[6];
                                                reg.Cod_barra = linhas[26].SoNumero();
                                                reg.TipoDespesa = TipoDespesa.GPS;
                                            }
                                        }
                                        else if (linhas[20].Trim().ToUpper().Equals("DOCUMENTO DE ARRECADAÇÃO DE RECEITAS FEDERAIS"))
                                        {
                                            reg = new RegPDF();
                                            reg.Cnpj = linhas[90];
                                            reg.Nr_docto = linhas[87].Trim().Substring(3, 7);
                                            reg.Vl_documento = decimal.Parse(linhas[105]);
                                            reg.Dt_emissao = linhas[87];
                                            reg.Dt_vencimento = linhas[102];
                                            reg.TipoDespesa = TipoDespesa.IRRF;
                                        }
                                        else if (linhas[0].Trim().ToUpper().Equals("DOCUMENTO DE ARRECADAÇÃO") &&
                                                linhas[1].Trim().ToUpper().Equals("DO SIMPLES NACIONAL"))
                                        {
                                            reg = new RegPDF();
                                            reg.Cnpj = linhas[2].Substring(0, linhas[2].IndexOf(' '));
                                            reg.Nr_docto = linhas[11].Split(' ')[0];
                                            reg.Vl_documento = decimal.Parse(linhas[9]);
                                            reg.Dt_emissao = new DateTime(int.Parse(reg.Nr_docto.Split('/')[1]), reg.Nr_docto.Split('/')[0].MesInt(), DateTime.DaysInMonth(int.Parse(reg.Nr_docto.Split('/')[1]), reg.Nr_docto.Split('/')[0].MesInt())).ToString("dd/MM/yyyy");
                                            reg.Dt_vencimento = linhas[6];
                                            reg.Cod_barra = linhas[35].SoNumero();
                                            reg.TipoDespesa = TipoDespesa.DAS;
                                        }
                                        else if (texto.Contains("IRPJ"))
                                        {
                                            //Lista Datas
                                            Regex regex = new Regex(@"\d{2}\/\d{2}\/\d{4}");
                                            MatchCollection matchs = regex.Matches(texto);
                                            List<DateTime> datas = new List<DateTime>();
                                            foreach (Match m in matchs)
                                            {
                                                if (m.Value.IsDateTime())
                                                    if (!datas.Contains(DateTime.Parse(m.Value)))
                                                        datas.Add(DateTime.Parse(m.Value));
                                            }
                                            //Lista Valor
                                            regex = new Regex(@"(\d{1,3}(\.\d{3})*|\d+)(\,\d{2})");
                                            matchs = regex.Matches(texto);
                                            List<decimal> valores = new List<decimal>();
                                            foreach (Match m in matchs)
                                            {
                                                if (!valores.Contains(decimal.Parse(m.Value)))
                                                    valores.Add(decimal.Parse(m.Value));
                                            }
                                            reg = new RegPDF();
                                            reg.Nr_docto = datas.Min().Month.ToString() + "/" + datas.Min().Year.ToString();
                                            reg.Vl_documento = valores.Max();
                                            reg.Dt_emissao = datas.Min().ToString("dd/MM/yyyy");
                                            reg.Dt_vencimento = datas.Max().ToString("dd/MM/yyyy");
                                            reg.TipoDespesa = TipoDespesa.IRPJ;
                                        }
                                        else if (Regex.IsMatch(texto, @"\d{5}\.\d{5} \d{5}\.\d{6} \d{5}\.\d{6} \d \d{14}") ||
                                            Regex.IsMatch(texto, @"\d{5}\.\d{5}  \d{5}\.\d{6}  \d{5}\.\d{6}  \d  \d{14}"))
                                        {
                                            //Verificar se é boleto
                                            Regex regex = new Regex(@"\d{5}\.\d{5}  \d{5}\.\d{6}  \d{5}\.\d{6}  \d  \d{14}");
                                            Match match = regex.Match(texto);
                                            string linha = string.Empty;
                                            if (match.Success)
                                                linha = match.Value;
                                            if (string.IsNullOrWhiteSpace(linha))
                                            {
                                                regex = new Regex(@"\d{5}\.\d{5} \d{5}\.\d{6} \d{5}\.\d{6} \d \d{14}");
                                                match = regex.Match(texto);
                                                if (match.Success)
                                                    linha = match.Value;
                                            }
                                            //Procurar CNPJ
                                            regex = new Regex(@"[0-9]{2}\.?[0-9]{3}\.?[0-9]{3}\/?[0-9]{4}\-?[0-9]{2}");
                                            MatchCollection matchs = regex.Matches(texto);
                                            List<string> cnpjs = new List<string>();
                                            foreach (Match m in matchs)
                                            {
                                                if (m.Value.ValidaCNPJ())
                                                    if (!cnpjs.Contains(m.Value))
                                                        cnpjs.Add(m.Value);
                                            }
                                            string filtro = string.Empty;
                                            string virg = string.Empty;
                                            cnpjs.ForEach(p =>
                                            {
                                                filtro += virg + "'" + p.SoNumero() + "'";
                                                virg = ",";
                                            });
                                            //Lista Datas
                                            regex = new Regex(@"\d{2}\/\d{2}\/\d{4}");
                                            matchs = regex.Matches(texto);
                                            List<DateTime> datas = new List<DateTime>();
                                            foreach (Match m in matchs)
                                            {
                                                if (m.Value.IsDateTime())
                                                    if (!datas.Contains(DateTime.Parse(m.Value)))
                                                        datas.Add(DateTime.Parse(m.Value));
                                            }
                                            //Vencimento e valor
                                            regex = new Regex(@"\d{2}\/\d{2}\/\d{4} \d{1,3}\,\d{2}");
                                            match = regex.Match(texto);
                                            string data_valor = string.Empty;
                                            string vencimento = string.Empty;
                                            string valor = string.Empty;
                                            if (match.Success)
                                                data_valor = match.Value;
                                            if (!string.IsNullOrWhiteSpace(data_valor))
                                            {
                                                vencimento = data_valor.Split(' ')[0];
                                                valor = data_valor.Split(' ')[1];
                                            }
                                            else
                                            {
                                                if (string.IsNullOrWhiteSpace(data_valor))
                                                {
                                                    vencimento = datas.Max().ToString("dd/MM/yyyy");
                                                    regex = new Regex(@"R\$\ (\d{1,3}(\.\d{3})*|\d+)(\,\d{2})?");
                                                    match = regex.Match(texto);
                                                    if (match.Success)
                                                        valor = match.Value.Replace("R$", string.Empty);
                                                }
                                            }
                                            reg = new RegPDF();
                                            reg.Cnpjs = filtro;
                                            reg.Nr_docto = datas.Min().Month.ToString() + "/" + datas.Min().Year.ToString();
                                            reg.Vl_documento = decimal.Parse(valor);
                                            reg.Dt_emissao = datas.Min().ToString("dd/MM/yyyy");
                                            reg.Dt_vencimento = vencimento;
                                            reg.TipoDespesa = TipoDespesa.Boleto;
                                        }
                                    }
                                }
                                return reg;
                            });
            return ret;
        }
        public async Task<RegPDF> LerPDFPagamento()
        {
            RegPDF ret = await Task.Run(() =>
            {
                RegPDF reg = null;
                using (PdfReader reader = new PdfReader(_path))
                {
                    using (PdfDocument doc = new PdfDocument(reader))
                    {
                        string texto = PdfTextExtractor.GetTextFromPage(doc.GetPage(1), new SimpleTextExtractionStrategy());
                        //Lista Datas
                        Regex regex = new Regex(@"\d{2}\/\d{2}\/\d{4}");
                        MatchCollection matchs = regex.Matches(texto);
                        List<DateTime> datas = new List<DateTime>();
                        foreach (Match m in matchs)
                        {
                            if (m.Value.IsDateTime())
                                if (!datas.Contains(DateTime.Parse(m.Value)))
                                    datas.Add(DateTime.Parse(m.Value));
                        }
                        //Lista Valor
                        regex = new Regex(@"(\d{1,3}(\.\d{3})*|\d+)(\,\d{2})");
                        matchs = regex.Matches(texto);
                        List<decimal> valores = new List<decimal>();
                        foreach (Match m in matchs)
                        {
                            if (!valores.Contains(decimal.Parse(m.Value)))
                                valores.Add(decimal.Parse(m.Value));
                        }
                        reg = new RegPDF();
                        reg.Vl_documento = valores.Max();
                        reg.Dt_emissao = datas.Max().ToString("dd/MM/yyyy");
                    }
                }
                return reg;
            });
            return ret;
        }
    }
}
