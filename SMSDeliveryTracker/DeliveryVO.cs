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

namespace SMSDeliveryTracker
{
    public class DeliveryVO
    {
        public int Id { get; set; }

        public string DeliveryId { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string MobileNumber { get; set; }

        public DateTime CommitedDeliveryTime { get; set; }

        public DateTime ActualDelivryTime { get; set; }
    }
}