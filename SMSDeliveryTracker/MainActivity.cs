using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DemoApp
{
    [Activity(Label = "DemoApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        //Database class new object
        Database sqldb;
        
        TextView txtID, txtDeliveryId, txtCustomerName, txtCommitedDeliveryTime;
        TextView shMsg;

        ListView listItems;
        //Launches the Create event for app
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            //Set our Main layout as default view
            SetContentView (Resource.Layout.Main);
            //Initializes new Database class object

            //Application.Context.DeleteDatabase("deliverydb");            
            sqldb = new Database("deliverydb");
            //Gets ImageButton object instances

            txtID = FindViewById<TextView>(Resource.Id.ID);
            txtDeliveryId = FindViewById<TextView> (Resource.Id.DeliveryId);
            txtCustomerName = FindViewById<TextView> (Resource.Id.CustomerName);
            txtCommitedDeliveryTime = FindViewById<TextView> (Resource.Id.CommitedDeliveryTime);
            //Gets TextView object instances
            shMsg = FindViewById<TextView> (Resource.Id.shMsg);
            //Gets ListView object instance
            listItems = FindViewById<ListView> (Resource.Id.listItems);
            //Sets Database class message property to shMsg TextView instance
            shMsg.Text = DateTime.Today.Date.ToShortDateString();

            GetCursorView();
         
            //Add ItemClick event handler to ListView instance
            listItems.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs> (item_Clicked);           

        }
        //Launched when a ListView item is clicked
        
        void item_Clicked (object sender, AdapterView.ItemClickEventArgs e)
        {
            //Gets TextView object instance from record_view layout

            TextView shId = e.View.FindViewById<TextView>(Resource.Id.DeliveryId_row);
            TextView shDeliveryId = e.View.FindViewById<TextView> (Resource.Id.DeliveryId_row);
            TextView shCustomerName = e.View.FindViewById<TextView> (Resource.Id.CustomerName_row);
            TextView shCommitedDeliveryTime = e.View.FindViewById<TextView> (Resource.Id.CommitedDeliveryTime_row);           

            var activity2 = new Intent(this, typeof(ViewDelivery));
            activity2.PutExtra("deliveryId", shDeliveryId.Text);
            StartActivity(activity2);
        }
        //Gets the cursor view to show all records
        void GetCursorView()
        {
            Android.Database.ICursor sqldb_cursor = sqldb.GetRecordCursor();
            if (sqldb_cursor != null) 
            {
                sqldb_cursor.MoveToFirst ();
                string[] from = new string[] {"_id", "DeliveryId", "CustomerName", "CommitedDeliveryTime" };
                int[] to = new int[] {

                    Resource.Id.Id_row,
                    Resource.Id.DeliveryId_row, 
                    Resource.Id.CustomerName_row,
                    Resource.Id.CommitedDeliveryTime_row 
                };
                //Creates a SimplecursorAdapter for ListView object
                SimpleCursorAdapter sqldb_adapter = new SimpleCursorAdapter (this, Resource.Layout.record_view, sqldb_cursor, from, to);
                listItems.Adapter = sqldb_adapter;
            } 
            else 
            {
                Toast.MakeText(Application.Context, sqldb.Message, ToastLength.Long).Show();
                
            }
        }
        //Gets the cursor view to show records according to search criteria
        void GetCursorView (string sqldb_column, string sqldb_value)
        {
            Android.Database.ICursor sqldb_cursor = sqldb.GetRecordCursor (sqldb_value);

            if (sqldb_cursor != null) 
            {
                sqldb_cursor.MoveToFirst ();
                string[] from = new string[] { "_id", "DeliveryId","CustomerName","CommitedDeliveryTime" };
                int[] to = new int[] 
                {
                    Resource.Id.Id_row,
                    Resource.Id.DeliveryId_row, 
                    Resource.Id.CustomerName_row,
                    Resource.Id.CommitedDeliveryTime_row
                 
                };
                SimpleCursorAdapter sqldb_adapter = new SimpleCursorAdapter (this, Resource.Layout.record_view, sqldb_cursor, from, to);
                listItems.Adapter = sqldb_adapter;
            } 
            else 
            {
                Toast.MakeText(Application.Context, sqldb.Message, ToastLength.Long).Show();
            }
        }
    }
}


