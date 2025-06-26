#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1502 // Element should not be on a single line
using System.Runtime.Serialization;

namespace TodoListApp.Core.Errors;

[Serializable]
public class RemoteServerProcessingException : BaseException
{
    public RemoteServerProcessingException() { }

    public RemoteServerProcessingException(string message)
        : base(message) { }

    public RemoteServerProcessingException(string message, Exception inner)
        : base(message, inner) { }

    protected RemoteServerProcessingException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
