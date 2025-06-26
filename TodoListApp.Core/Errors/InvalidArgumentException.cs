#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1502 // Element should not be on a single line
using System.Runtime.Serialization;

namespace TodoListApp.Core.Errors;

[Serializable]
public class InvalidArgumentException : BaseException
{
    public InvalidArgumentException() { }

    public InvalidArgumentException(string message)
        : base(message) { }

    public InvalidArgumentException(string message, Exception ex)
        : base(message, ex) { }

    protected InvalidArgumentException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
