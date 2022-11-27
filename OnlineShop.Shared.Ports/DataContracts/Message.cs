using System.Collections.Generic;

namespace OnlineShop.Shared.Ports.DataContracts
{
    public class Message
    {
        public MessageType MessageType { get; set; }
        public string ResourceKey { get; set; }
        public List<string> MessageParams { get; set; }

        public override string ToString()
        {
            var msgParams = MessageParams ?? new List<string>();
            return $"[{MessageType}] - [{ResourceKey}] - [{string.Join(",", msgParams)}]";
        }
    }
}