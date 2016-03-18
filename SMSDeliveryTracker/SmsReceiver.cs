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
            //if (intent.HasExtra("pdus"))
            //{
            //    try
            //    {
            //        var smsArray = (Java.Lang.Object[])intent.Extras.Get("pdus");
            //        string address = "";
            //        string message = "";

            //        foreach (var item in smsArray)
            //        {
            //            var sms = SmsMessage.CreateFromPdu((byte[])item);
            //            message += sms.MessageBody;
            //            address = sms.OriginatingAddress;

            //            this.SaveSMS(sms.MessageBody, sms.OriginatingAddress);
            //            Toast.MakeText(Application.Context, "A new message is received", ToastLength.Short).Show();
            //        }
            //    }
            //    catch(Exception ex)
            //    {
            //        Toast.MakeText(Application.Context, ex.Message.ToString(), ToastLength.Short).Show();

            //    }
            //}

            Log.Info(Tag, "Intent received: " + intent.Action);

            if (intent.Action != IntentAction) return;

            SmsMessage[] messages = Android.Provider.Telephony.Sms.Intents.GetMessagesFromIntent(intent);
 
            var sb = new StringBuilder();

            for (var i = 0; i < messages.Length; i++)
            {
                
                try
                {
                    if (messages[i].OriginatingAddress == "+639288706627")
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
        //MessageText, MessageFrom, SendFrom, ReceiveTime
        private void SaveSMS(string messagetext, string messagefrom)
        {
            
            MessageInVO vo = new MessageInVO();
            Database db = new Database();

            
           // string result2 = "";
            try
            {
                vo.MessageText = messagetext;
                vo.MessageFrom = messagefrom;

                //result1 = db.AddRecordMessageIn2(vo);
                //result1 = db.AddRecordDelivery2(vo.MessageText);

                Toast.MakeText(Application.Context, "SMS Received", ToastLength.Long).Show();
            }

            catch(Exception ex)
            {
                Toast.MakeText(Application.Context, result1, ToastLength.Long).Show();
            }

        }
    }
}