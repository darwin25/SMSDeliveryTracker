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
using Android.Util;

namespace SMSDeliveryTracker
{
    [BroadcastReceiver]
    [IntentFilter(
        new[] { "android.provider.Telephony.SMS_RECEIVED" })]

    public class SmsReceiver : BroadcastReceiver
    {
        Database sqldb;
      
        private const string Tag = "SMSBroadcastReceiver";
        private const string IntentAction = "android.provider.Telephony.SMS_RECEIVED";
        string result1 = "";

        public override void OnReceive(
        Context context, Intent intent)
        {
            sqldb = new Database("deliverydb");
            
            Log.Info(Tag, "Intent received: " + intent.Action);

            if (intent.Action != IntentAction) return;

            SmsMessage[] messages = Android.Provider.Telephony.Sms.Intents.GetMessagesFromIntent(intent);
 
            var sb = new StringBuilder();

            for (var i = 0; i < messages.Length; i++)
            {                
                try
                {
                    if (messages[i].OriginatingAddress == AppConstant.SMSServer)
                        {
                        sqldb.AddRecordMessageIn2(messages[i].MessageBody, messages[i].OriginatingAddress);
                        sqldb.AddRecordDelivery2(messages[i].MessageBody);

                        Toast.MakeText(Application.Context, "SMS Received - " + messages[i].MessageBody, ToastLength.Long).Show();
                    }
                }
                catch (Exception ex)
                {                    
                    Toast.MakeText(Application.Context, ex.Message.ToString(), ToastLength.Long).Show();
                }
            }
        }
    }
}