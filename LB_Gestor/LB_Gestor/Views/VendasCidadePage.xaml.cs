using System;
using Xamarin.Forms;

namespace LB_Gestor.Views
{
    public partial class VendasCidadePage : ContentPage
    {
        public VendasCidadePage()
        {
            InitializeComponent();
            myFiltro.MaxDate = DateTime.Now;
        }
    }
}
