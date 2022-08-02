using System;
using Xamarin.Forms;

namespace LB_Gestor.Views
{
    public partial class VendasGrupoPage : ContentPage
    {
        public VendasGrupoPage()
        {
            InitializeComponent();
            myFiltro.MaxDate = DateTime.Now;
        }
    }
}
