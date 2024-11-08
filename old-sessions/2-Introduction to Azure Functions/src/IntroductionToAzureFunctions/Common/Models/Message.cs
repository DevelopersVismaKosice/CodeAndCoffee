using System;

namespace Common.Models
{
    public class Message
    {
        public string RequestId { get; set; }
        public Request Request { get; set; }

        public Message()
        { }

        public Message(Request request)
        {
            RequestId = Guid.NewGuid().ToString();
            Request = request;
        }
    }
}
