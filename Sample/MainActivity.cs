using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Cocosw.BottomSheetActions;
using Android.Support.V7.App;

namespace BottomSheets.Sample
{
    [Activity(Label = "BottomSheet Sample", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ListView listView;
        private ArrayAdapter<String> adapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            String[] items = new String[]{"From Xml", "Without Icon", "Dark Theme", "Grid", "Style", "Style from Theme", "ShareAction", "ShareAction Show All", "Menu Manipulate", "Header Layout"};
            this.adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, Android.Resource.Id.Text1, items);

            this.listView = FindViewById<ListView>(Resource.Id.listView);
            this.listView.Adapter = this.adapter;
        }

        protected override void OnResume()
        {
            base.OnResume();
            this.listView.ItemClick += ListView_ItemClick;
        }

        protected override void OnPause()
        {
            this.listView.ItemClick -= ListView_ItemClick;
            base.OnPause();
        }

        private void ListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(BottomSheetListActivity));
            intent.PutExtra("action", e.Position);
            intent.PutExtra("title", this.adapter.GetItem(e.Position));
            intent.PutExtra("style", (e.Position == 5));
            StartActivity(intent);
        }
    }
}