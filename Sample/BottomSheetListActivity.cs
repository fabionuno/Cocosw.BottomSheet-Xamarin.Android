
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Cocosw.BottomSheetActions;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Support.V4.Graphics.Drawable;


namespace BottomSheets.Sample
{
    [Activity]			
    public class BottomSheetListActivity : AppCompatActivity, AdapterView.IOnItemClickListener, IDialogInterfaceOnClickListener
    {
        private int action;
        private ArrayAdapter<String> adapter;
        private int selectedPosition;

        protected override void OnCreate(Bundle bundle)
        {
            if (Intent.GetBooleanExtra("style", false))
            {
                this.SetTheme(Resource.Style.StyleTheme);
            }

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            this.Title = Intent.GetStringExtra("title");
            this.action = Intent.GetIntExtra("action", 0);

            String[] items = new String[]{"Miguel de Icaza", "Nat Friedman", "James Montemagno", "Joseph Hill", "Stephanie Schatz\n"};

            this.adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, Android.Resource.Id.Text1, items);
            var listView = FindViewById<ListView>(Resource.Id.listView);
            listView.Adapter = this.adapter;
            listView.OnItemClickListener = this;

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                this.Finish();

            return base.OnOptionsItemSelected(item);
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            this.selectedPosition = position;
            this.ShowBottomSheet(this.selectedPosition);
        }

        private void ShowBottomSheet(int position)
        {            
            BottomSheetFragment fragment = 
                BottomSheetFragment.NewInstance(this.action, this.adapter.GetItem(position));

            fragment.Show(this.SupportFragmentManager, "dialog");

        }
    
        public void OnClick(IDialogInterface dialog, int which)
        {
            string name = this.adapter.GetItem(this.selectedPosition);
            switch (which) 
            {
                case Resource.Id.share:
                    Toast.MakeText(this, "Share to " + name, ToastLength.Short).Show();
                    break;
                case Resource.Id.upload:
                    Toast.MakeText(this, "Upload for " + name, ToastLength.Short).Show();
                    break;
                case Resource.Id.call:
                    Toast.MakeText(this, "Call to " + name, ToastLength.Short).Show();
                    break;
                case Resource.Id.help:
                    Toast.MakeText(this, "Help me!", ToastLength.Short).Show();
                    break;
            }
        }
    }
}