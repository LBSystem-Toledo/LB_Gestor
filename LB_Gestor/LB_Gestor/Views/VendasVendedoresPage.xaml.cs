using System;
using Xamarin.Forms;

namespace LB_Gestor.Views
{
    public partial class VendasVendedoresPage : ContentPage
    {
        public VendasVendedoresPage()
        {
            InitializeComponent();
            myFiltro.MaxDate = DateTime.Now;
        }
    }
}
