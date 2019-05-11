using System.Collections.Generic;
using System.Linq;

namespace ProjectDiploma.ViewModel
{
    public enum MessageType
    {
        INFO,
        WARNING,
        ERROR
    }

    public class Response<T> : Response
    {
        public T ItemResult { get; set; }

        public Response(T item)
        {
            ItemResult = item;
        }
    }

    public struct Message
    {
        public MessageType MessageType;
        public string Text;

        public Message(MessageType messageType, string text)
        {
            MessageType = messageType;
            Text = text;
        }
    }

    public class Response
    {
        public bool HasErrors { get => Messages.Any(message => message.MessageType == MessageType.ERROR); }
        public List<Message> Messages { get; } = new List<Message>();

        public Response AddMessage(MessageType messageType, string text)
        {
            Messages.Add(new Message(messageType, text));
            return this;
        }
    }
}
