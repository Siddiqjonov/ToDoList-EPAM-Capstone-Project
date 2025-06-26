#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1502 // Element should not be on a single line
using System.Runtime.Serialization;

namespace TodoListApp.Core.Errors;

[Serializable]
public class UnauthorizedException : BaseException
{
    public UnauthorizedException() { }

    public UnauthorizedException(string message)
        : base(message) { }

    public UnauthorizedException(string message, Exception inner)
        : base(message, inner) { }

    protected UnauthorizedException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
