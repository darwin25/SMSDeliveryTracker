using System;

namespace DemoApp
{
    [Serializable()]
    public class MessageInVO
    {
        
        public int Id { get; set; }

        public DateTime SendTime { get; set; }

        public DateTime ReceiveTime { get; set; }

        public string MessageFrom { get; set; }

        public string MessageTo { get; set; }

        public string SMSC { get; set; }

        public string MessageText { get; set; }

        public string MessageType { get; set; }

        public int MessageParts { get; set; }

        public string MessagePDU { get; set; }

        public string Gateway { get; set; }

        public string UserId { get; set; }

    }
}