using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;

namespace LB_Gestor.Droid
{
    [Activity(Theme = "@style/MainTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    [IntentFilter(new[] { Intent.ActionView }, 
        Categories = new[] { Intent.CategoryDefault }, 
        DataMimeType = @"application/pdf")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        App _mainForms;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Instance = this;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            UserDialogs.Init(this);

            string vpath = string.Empty;
            if (Intent.Action == Intent.ActionView)
            {
                var uriFromExtras = Intent.GetParcelableExtra(Intent.ExtraStream) as Android.Net.Uri;
                var subject = Intent.GetStringExtra(Intent.ExtraSubject);

                var pdfStream = ContentResolver.OpenInputStream(Intent.Data);

                var memOfPdf = new System.IO.MemoryStream();
                pdfStream.CopyTo(memOfPdf);

                var docsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                vpath = System.IO.Path.Combine(docsPath, "temp.pdf");

                System.IO.File.WriteAllBytes(vpath, memOfPdf.ToArray());
            }

            _mainForms = new App(new AndroidInitializer(), vpath);

            LoadApplication(_mainForms);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

