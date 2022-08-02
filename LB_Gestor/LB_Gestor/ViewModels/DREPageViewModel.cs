using Acr.UserDialogs;
using LB_Gestor.Interface;
using LB_Gestor.Models;
using Microcharts;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LB_Gestor.ViewModels
{
    public class DREPageViewModel : ViewModelBase
    {
        Registro _anocorrente;
        public Registro AnoCorrente
        {
            get { return _anocorrente; }
            set
            {
                SetProperty(ref _anocorrente, value);
                BuscarDREAsync();
            }
        }
        ObservableCollection<Registro> _anos;
        public ObservableCollection<Registro> Anos { get { return _anos; } set { SetProperty(ref _anos, value); } }
        ObservableCollection<DRE> _dre;
        public ObservableCollection<DRE> DRE { get { return _dre; } set { SetProperty(ref _dre, value); } }
        Chart _chardre;
        public Chart ChartDRE { get { return _chardre; } set { SetProperty(ref _chardre, value); } }

        readonly IPageDialogService dialogService;
        readonly IDataService dataService;
        public DREPageViewModel(INavigationService navigationService, IPageDialogService _dialogService, IDataService _dataservice)
            :base(navigationService)
        {
            dialogService = _dialogService;
            dataService = _dataservice;

            Anos = new ObservableCollection<Registro>();
            for (int i = 0; i < 10; i++)
                Anos.Add(new Registro { Chave = DateTime.Now.AddYears(i * -1).Year.ToString(), Valor = DateTime.Now.AddYears(i * -1).Year.ToString() });
            AnoCorrente = Anos.FirstOrDefault();
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            await BuscarDREAsync();
        }
        private async Task BuscarDREAsync()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Clear))
                {
                    IEnumerable<DRE> lista = await dataService.GetDREAsync(int.Parse(AnoCorrente.Chave));
                    if (lista == null ? false : lista.Count() > 0)
                    {
                        decimal r_janeiro = decimal.Zero;
                        decimal r_fevereiro = decimal.Zero;
                        decimal r_marco = decimal.Zero;
                        decimal r_abril = decimal.Zero;
                        decimal r_maio = decimal.Zero;
                        decimal r_junho = decimal.Zero;
                        decimal r_julho = decimal.Zero;
                        decimal r_agosto = decimal.Zero;
                        decimal r_setembro = decimal.Zero;
                        decimal r_outubro = decimal.Zero;
                        decimal r_novembro = decimal.Zero;
                        decimal r_dezembro = decimal.Zero;
                        lista
                            .ToList()
                            .Where(p => !p.Tp_conta.Trim().ToUpper().Equals("A"))
                            .ToList()
                            .ForEach(p =>
                            {
                                if (p.Nivel.Equals(1) && p.Tp_conta.Trim().ToUpper().Equals("S"))
                                {
                                    r_janeiro += p.Vl_janeiro * (p.Operador.Trim().ToUpper().Equals("D") ? -1 : 1);
                                    r_fevereiro += p.Vl_fevereiro * (p.Operador.Trim().ToUpper().Equals("D") ? -1 : 1);
                                    r_marco += p.Vl_marco * (p.Operador.Trim().ToUpper().Equals("D") ? -1 : 1);
                                    r_abril += p.Vl_abril * (p.Operador.Trim().ToUpper().Equals("D") ? -1 : 1);
                                    r_maio += p.Vl_maio * (p.Operador.Trim().ToUpper().Equals("D") ? -1 : 1);
                                    r_junho += p.Vl_junho * (p.Operador.Trim().ToUpper().Equals("D") ? -1 : 1);
                                    r_julho += p.Vl_julho * (p.Operador.Trim().ToUpper().Equals("D") ? -1 : 1);
                                    r_agosto += p.Vl_agosto * (p.Operador.Trim().ToUpper().Equals("D") ? -1 : 1);
                                    r_setembro += p.Vl_setembro * (p.Operador.Trim().ToUpper().Equals("D") ? -1 : 1);
                                    r_outubro += p.Vl_outubro * (p.Operador.Trim().ToUpper().Equals("D") ? -1 : 1);
                                    r_novembro += p.Vl_novembro * (p.Operador.Trim().ToUpper().Equals("D") ? -1 : 1);
                                    r_dezembro += p.Vl_dezembro * (p.Operador.Trim().ToUpper().Equals("D") ? -1 : 1);
                                }
                                else if (p.Nivel.Equals(1) && p.Tp_conta.Trim().ToUpper().Equals("R"))
                                {
                                    p.Vl_janeiro = r_janeiro;
                                    p.Vl_fevereiro = r_fevereiro;
                                    p.Vl_marco = r_marco;
                                    p.Vl_abril = r_abril;
                                    p.Vl_maio = r_maio;
                                    p.Vl_junho = r_junho;
                                    p.Vl_julho = r_julho;
                                    p.Vl_agosto = r_agosto;
                                    p.Vl_setembro = r_setembro;
                                    p.Vl_outubro = r_outubro;
                                    p.Vl_novembro = r_novembro;
                                    p.Vl_dezembro = r_dezembro;
                                }
                            });
                        DRE = new ObservableCollection<DRE>(lista);
                        //Montar grafico 
                        ChartEntry[] entriP = new ChartEntry[12];
                        entriP[0] = new ChartEntry((float)r_janeiro)
                        {
                            Label = "Janeiro",
                            ValueLabel = r_janeiro.ToString("C")
                        };
                        entriP[1] = new ChartEntry((float)r_fevereiro)
                        {
                            Label = "Fevereiro",
                            ValueLabel = r_fevereiro.ToString("C")
                        };
                        entriP[2] = new ChartEntry((float)r_marco)
                        {
                            Label = "Março",
                            ValueLabel = r_marco.ToString("C")
                        };
                        entriP[3] = new ChartEntry((float)r_abril)
                        {
                            Label = "Abril",
                            ValueLabel = r_abril.ToString("C")
                        };
                        entriP[4] = new ChartEntry((float)r_maio)
                        {
                            Label = "Maio",
                            ValueLabel = r_maio.ToString("C")
                        };
                        entriP[5] = new ChartEntry((float)r_junho)
                        {
                            Label = "Junho",
                            ValueLabel = r_junho.ToString("C")
                        };
                        entriP[6] = new ChartEntry((float)r_julho)
                        {
                            Label = "Julho",
                            ValueLabel = r_julho.ToString("C")
                        };
                        entriP[7] = new ChartEntry((float)r_agosto)
                        {
                            Label = "Agosto",
                            ValueLabel = r_agosto.ToString("C")
                        };
                        entriP[8] = new ChartEntry((float)r_setembro)
                        {
                            Label = "Setembro",
                            ValueLabel = r_setembro.ToString("C")
                        };
                        entriP[9] = new ChartEntry((float)r_outubro)
                        {
                            Label = "Outubro",
                            ValueLabel = r_outubro.ToString("C")
                        };
                        entriP[10] = new ChartEntry((float)r_novembro)
                        {
                            Label = "Novembro",
                            ValueLabel = r_novembro.ToString("C")
                        };
                        entriP[11] = new ChartEntry((float)r_dezembro)
                        {
                            Label = "Dezembro",
                            ValueLabel = r_dezembro.ToString("C")
                        };
                        ChartDRE = new PieChart
                        {
                            Entries = entriP
                        };
                    }
                    else
                    {
                        DRE = new ObservableCollection<DRE>();
                        ChartDRE = null;
                    }
                }
            else await dialogService.DisplayAlertAsync("Mensagem", "Dispositivo sem conexão com internet.", "OK");
        }
    }
}
