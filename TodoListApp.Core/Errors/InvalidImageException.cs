#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1502 // Element should not be on a single line
using System.Runtime.Serialization;

namespace TodoListApp.Core.Errors;

[Serializable]
public class InvalidImageException : BaseException
{
    public InvalidImageException() { }

    public InvalidImageException(string message)
        : base(message) { }

    public InvalidImageException(string message, Exception ex)
        : base(message, ex) { }

    protected InvalidImageException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
