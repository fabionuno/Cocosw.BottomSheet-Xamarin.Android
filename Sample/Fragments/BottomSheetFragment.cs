
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Cocosw.BottomSheetActions;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Support.V4.Graphics.Drawable;



namespace BottomSheets.Sample
{
    public class BottomSheetFragment : Android.Support.V4.App.DialogFragment
    {
        private string name;
        private int action;

        public static BottomSheetFragment NewInstance(int action, string title) 
        {
            BottomSheetFragment frag = new BottomSheetFragment();
            Bundle args = new Bundle();
            args.PutString("title", title);
            args.PutInt("action", action);
            frag.Arguments = args;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            this.name = this.Arguments.GetString("title");
            this.action = this.Arguments.GetInt("action");

            switch (this.action)
            {
                default:
                case 0:
                    return SheetFromXml();
                case 1:
                    return SheetWithoutIcon();
                case 2:
                    return SheetDarkTheme();
                case 3:
                    return SheetGrid();
                case 4:
                    return SheetStyle();
                case 5:
                    return SheetStyleFromTheme();
                case 6:
                    return SheetShareItem();
                case 7:
                    return SheetShareItemShowAll();
                case 8:
                    return SheetMenuManipulate();
                case 9:
                    return SheetHeaderLayout();
            }

        }
       

        private BottomSheet SheetFromXml()
        {
            BottomSheet sheet = new BottomSheet.Builder(this.Activity)
                .Icon(this.GetRoundedBitmap(Resource.Drawable.icon))
                .Title(String.Format("To {0}", this.name))
                .Listener((IDialogInterfaceOnClickListener)this.Activity)
                .Sheet(Resource.Menu.list)
                .Build();

            return sheet;
        }

        private BottomSheet SheetWithoutIcon()
        {
            BottomSheet sheet = new BottomSheet.Builder(this.Activity)
                .Sheet(Resource.Menu.noicon)
                .Listener((IDialogInterfaceOnClickListener)this.Activity)
                .Build();

            return sheet;
        }

        private BottomSheet SheetDarkTheme()
        {
            BottomSheet sheet = new BottomSheet.Builder(this.Activity)
                .DarkTheme()
                .Title(String.Format("To {0}", this.name))
                .Sheet(Resource.Menu.list)
                .Listener((IDialogInterfaceOnClickListener)this.Activity)
                .Build();

            return sheet;
        }

        private BottomSheet SheetGrid()
        {
            BottomSheet sheet = new BottomSheet.Builder(this.Activity)
                .Sheet(Resource.Menu.list)
                .Listener((IDialogInterfaceOnClickListener)this.Activity)
                .Grid()
                .Build();

            return sheet;
        }

        private BottomSheet SheetStyle()
        {
            BottomSheet sheet = new BottomSheet.Builder(this.Activity, Resource.Style.BottomSheet_StyleDialog)
                .Title(String.Format("To {0}", this.name))
                .Sheet(Resource.Menu.list)
                .Listener((IDialogInterfaceOnClickListener)this.Activity)
                .Build();

            return sheet;
        }

        private BottomSheet SheetStyleFromTheme()
        {
            BottomSheet sheet = new BottomSheet.Builder(this.Activity)
                .Title(String.Format("To {0}", this.name))
                .Sheet(Resource.Menu.longlist)
                .Listener((IDialogInterfaceOnClickListener)this.Activity)
                .Limit(Resource.Integer.bs_initial_list_row)
                .Build();

            return sheet;
        }

        private BottomSheet SheetShareItem()
        {
            BottomSheet sheet = GetShareActions("Hello " + this.name)
                .Title("Share To " + this.name)
                .Build();

            return sheet;
        }

        private BottomSheet SheetShareItemShowAll()
        {
            BottomSheet sheet = GetShareActions("Hello " + this.name)
                .Title("Share To " + this.name)
                //Set initial number of actions which will be shown in current sheet.
                //* If more actions need to be shown, a "more" action will be displayed in the last position.
                .Limit(Resource.Integer.no_limit)
                .Build();

            return sheet;
        }

        private BottomSheet SheetMenuManipulate()
        {
            BottomSheet sheet = new BottomSheet.Builder(this.Activity)
                .Icon(this.GetRoundedBitmap(Resource.Drawable.icon))
                .Title(String.Format("To {0}", this.name))
                .Sheet(Resource.Menu.list)
                .Listener((IDialogInterfaceOnClickListener)this.Activity)
                .Build();


            IMenu menu = sheet.Menu;
            menu.GetItem(0).SetTitle("MenuClickListener");
            menu.GetItem(0).SetOnMenuItemClickListener(new MenuItemListener());

            menu.GetItem(1).SetVisible(false);
            menu.GetItem(2).SetEnabled(false);
            menu.Add(Menu.None, 23, Menu.None, "Fresh meal!");

            menu.FindItem(23).SetIcon(Resource.Drawable.perm_group_user_dictionary);
            menu.FindItem(23).SetOnMenuItemClickListener(new MenuItemListener());
            menu.SetGroupVisible(Android.Resource.Id.Empty, false);

            return sheet;
        }

        private BottomSheet SheetHeaderLayout()
        {
            BottomSheet sheet = new BottomSheet.Builder(this.Activity, Resource.Style.BottomSheet_CustomDialog)
                .Title(String.Format("To {0}", this.name))
                .Sheet(Resource.Menu.list)
                .Listener((IDialogInterfaceOnClickListener)this.Activity)
                .Build();

            sheet.ShowEvent += (object sender, EventArgs e) => {
                Toast.MakeText(this.Activity, "I'm showing", ToastLength.Short).Show();
            };

            sheet.DismissEvent += (object sender, EventArgs e) => {
                Toast.MakeText(this.Activity, "I'm dismissing", ToastLength.Short).Show();
            };

            return sheet;
        }

        private Drawable GetRoundedBitmap(int imageId) 
        {
            Bitmap src = BitmapFactory.DecodeResource(this.Resources, imageId);
            Bitmap dst;
            if (src.Width >= src.Height) {
                dst = Bitmap.CreateBitmap(src, src.Width / 2 - src.Height / 2, 0, src.Height, src.Height);
            } else {
                dst = Bitmap.CreateBitmap(src, 0, src.Height / 2 - src.Width / 2, src.Width, src.Width);
            }
            RoundedBitmapDrawable roundedBitmapDrawable = RoundedBitmapDrawableFactory.Create(this.Resources, dst);
            roundedBitmapDrawable.CornerRadius = (dst.Width / 2);
            roundedBitmapDrawable.SetAntiAlias(true);
            return roundedBitmapDrawable;
        }

        private BottomSheet.Builder GetShareActions(String text) 
        {
            Intent shareIntent = new Intent(Intent.ActionSend);
            shareIntent.SetType("text/plain");
            shareIntent.PutExtra(Intent.ExtraText, text);

            return BottomSheetHelper.ShareAction(this.Activity, shareIntent);
        }

        private class MenuItemListener : Java.Lang.Object, IMenuItemOnMenuItemClickListener
        {
            public bool OnMenuItemClick(IMenuItem item)
            {
                string msg = string.Empty;
                if (item.ItemId == 23)
                    msg = "Hello";
                else
                    msg = "You can set OnMenuItemClickListener for each item";

                Toast.MakeText(Application.Context, msg, ToastLength.Short).Show();
                return true;
            }
        }
    }
}