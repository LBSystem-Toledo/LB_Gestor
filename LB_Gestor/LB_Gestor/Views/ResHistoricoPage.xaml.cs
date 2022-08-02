using System;
using Xamarin.Forms;

namespace LB_Gestor.Views
{
    public partial class ResHistoricoPage : CarouselPage
    {
        public ResHistoricoPage()
        {
            InitializeComponent();
            myDespesa.MaxDate = DateTime.Now;
            myReceita.MaxDate = DateTime.Now;
        }
    }
}
