using Android.App;
using LB_Gestor.Interface;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(LB_Gestor.Droid.CloseApplication))]
namespace LB_Gestor.Droid
{
    public class CloseApplication : ICloseApplication
    {
        [Obsolete]
        public void closeApplication()
        {
            Activity activity = (Activity)Forms.Context;
            activity.FinishAffinity();
        }
    }
}