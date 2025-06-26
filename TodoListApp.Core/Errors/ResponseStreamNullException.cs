#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1502 // Element should not be on a single line
using System.Runtime.Serialization;

namespace TodoListApp.Core.Errors;

[Serializable]
public class ResponseStreamNullException : BaseException
{
    public ResponseStreamNullException() { }

    public ResponseStreamNullException(string message)
        : base(message) { }

    public ResponseStreamNullException(string message, Exception inner)
        : base(message, inner) { }

    protected ResponseStreamNullException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
