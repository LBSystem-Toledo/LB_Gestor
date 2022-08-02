using System;
using Xamarin.Forms;

namespace LB_Gestor.Views
{
    public partial class VendasPortadorPage : ContentPage
    {
        public VendasPortadorPage()
        {
            InitializeComponent();
            myFiltro.MaxDate = DateTime.Now;
        }
    }
}
