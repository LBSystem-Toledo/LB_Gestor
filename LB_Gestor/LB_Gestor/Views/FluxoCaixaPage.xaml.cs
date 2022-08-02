using System;
using Xamarin.Forms;

namespace LB_Gestor.Views
{
    public partial class FluxoCaixaPage : ContentPage
    {
        public FluxoCaixaPage()
        {
            InitializeComponent();
            dtFinal.MinimumDate = DateTime.Now;
        }
    }
}
