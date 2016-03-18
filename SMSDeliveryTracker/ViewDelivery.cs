using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DemoApp
{
    [Activity(Label = "ViewDelivery")]
    public class ViewDelivery : Activity
    {
        Database sqldb;

        EditText editDeliveryID, editCustomerName, editAddress, editMobileNo, editCommitedDeliveryTime, editOrderAmount, editDeliveryStatus;
        TextView txtDeliveryID, txtCustomerName, txtAddress, txtMobileNo, txtCommitedDeliveryTime, txtOrderAmount, txtDeliveryStatus;
        Button btnAccept, btnUpdatedCompleted;

        protected override void OnCreate(Bundle savedInstanceState)
        {            
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Delivery_Details);

            sqldb = new Database("deliverydb");

            InitializeControls();
           
            editDeliveryID.Text = Intent.GetStringExtra("deliveryId") ?? "Data not available";

            DisplayData(editDeliveryID.Text);

            btnAccept.Click += (object sender, EventArgs e) =>
            {
                string datetimeaccepted = DateTime.Now.ToString();
                string message = editDeliveryID.Text + "|" + datetimeaccepted + "|" + "ACCEPTED";

                SmsSender s = new SmsSender();

                s.SendSMS("+639288706627", message);

                sqldb.UpdateRecord(editDeliveryID.Text, "ACCEPTED", datetimeaccepted.ToString());
                btnAccept.Enabled = false;
                btnUpdatedCompleted.Enabled = true;
            };

            btnUpdatedCompleted.Click += (object sender, EventArgs e) =>
            {
                string datetimeaccepted = DateTime.Now.ToString();
                string message = editDeliveryID.Text + "|" + datetimeaccepted + "|" + "DELIVERED";

                SmsSender s = new SmsSender();
                
                s.SendSMS("+639288706627", message);

                sqldb.UpdateRecord(editDeliveryID.Text, "DELIVERED", datetimeaccepted.ToString(), datetimeaccepted.ToString());

                btnUpdatedCompleted.Enabled = false;               
            };

        }

        private void InitializeControls()
        {
            editDeliveryID = FindViewById<EditText>(Resource.Id.editDeliveryID);
            editCustomerName = FindViewById<EditText>(Resource.Id.editCustomerName);
            editAddress = FindViewById<EditText>(Resource.Id.editAddress);
            editMobileNo = FindViewById<EditText>(Resource.Id.editMobileNo);
            editCommitedDeliveryTime = FindViewById<EditText>(Resource.Id.editCommitedDeliveryTime);
            editOrderAmount = FindViewById<EditText>(Resource.Id.editOrderAmount);
            editDeliveryStatus = FindViewById<EditText>(Resource.Id.editDeliveryStatus);

            txtDeliveryID = FindViewById<TextView>(Resource.Id.txtDeliveryID);
            txtCustomerName = FindViewById<TextView>(Resource.Id.txtCustomerName);
            txtAddress = FindViewById<TextView>(Resource.Id.txtAddress);
            txtMobileNo = FindViewById<TextView>(Resource.Id.txtMobileNo);
            txtCommitedDeliveryTime = FindViewById<TextView>(Resource.Id.txtCommitedDeliveryTime);
            txtOrderAmount = FindViewById<TextView>(Resource.Id.txtOrderAmount);
            txtDeliveryStatus = FindViewById<TextView>(Resource.Id.txtDeliveryStatus);

            btnAccept = FindViewById<Button>(Resource.Id.btnAccept);
            btnUpdatedCompleted = FindViewById<Button>(Resource.Id.btnUpdateCompleted);
        }
        
        void DisplayData(string sValue)
        {
            DataTable dt = new DataTable();

            dt = sqldb.GetDeliveryDetails(sValue);

            editCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
            editAddress.Text = dt.Rows[0]["Address"].ToString();
            editMobileNo.Text = dt.Rows[0]["MobileNumber"].ToString();
            editCommitedDeliveryTime.Text  = this.GetTime(dt.Rows[0]["CommitedDeliveryTime"].ToString());
            editOrderAmount.Text = dt.Rows[0]["OrderAmount"].ToString();
            editDeliveryStatus.Text = dt.Rows[0]["DeliveryStatus"].ToString();


            if (editDeliveryStatus.Text == "PENDING / UNASSIGNED")
            {
                btnAccept.Enabled = true;
                btnUpdatedCompleted.Enabled = false;
            }
            else if (editDeliveryStatus.Text == "ACCEPTED")
            {
                btnAccept.Enabled = false;
                btnUpdatedCompleted.Enabled = true;
            }
            else if (editDeliveryStatus.Text == "DELIVERED")
            {
                btnAccept.Enabled = false;
                btnUpdatedCompleted.Enabled = false;
            }
            else
            {
                btnAccept.Enabled = false;
                btnUpdatedCompleted.Enabled = false;
            }
        }

        private string GetTime(string dtValue)
        {
            DateTime datetimevalue = DateTime.Parse(dtValue);

            return datetimevalue.ToShortTimeString();            
        }

    }
}