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

    public class Response
    {
        public bool HasErrors { get => Messages.Any(message => message.MessageType == MessageType.ERROR); }
        public List<(MessageType MessageType, string Text)> Messages { get; } = new List<(MessageType, string)>();

        public Response AddMessage(MessageType messageType, string text)
        {
            Messages.Add((messageType, text));
            return this;
        }
    }
}
