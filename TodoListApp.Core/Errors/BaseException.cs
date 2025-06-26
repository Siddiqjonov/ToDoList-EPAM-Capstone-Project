#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1502 // Element should not be on a single line
using System.Runtime.Serialization;
#pragma warning disable SYSLIB0051 // Type or member is obsolete

namespace TodoListApp.Core.Errors;

[Serializable]
public class BaseException : Exception
{
    public BaseException() { }

    public BaseException(string message)
        : base(message) { }

    public BaseException(string message, Exception inner)
        : base(message, inner) { }

    protected BaseException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
