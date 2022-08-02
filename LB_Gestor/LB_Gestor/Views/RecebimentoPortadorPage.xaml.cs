using System;
using Xamarin.Forms;

namespace LB_Gestor.Views
{
    public partial class RecebimentoPortadorPage : ContentPage
    {
        public RecebimentoPortadorPage()
        {
            InitializeComponent();
            myFiltro.MaxDate = DateTime.Now;
        }
    }
}
