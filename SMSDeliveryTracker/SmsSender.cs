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
using Android.Telephony;

namespace DemoApp
{
    public class SmsSender
    {
        public SmsSender()
        {
        }

        public string SendSMS(string mobilenumber, string message)
        {
            //var number = "0123456789";
            //var message = "SMS Message Text...";
            var manager = SmsManager.Default;
            try
            {
                manager.SendTextMessage(mobilenumber, null, message, null, null);
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.Message.ToString(), ToastLength.Long).Show();
            }

            return "0";
        }

        
    }
}