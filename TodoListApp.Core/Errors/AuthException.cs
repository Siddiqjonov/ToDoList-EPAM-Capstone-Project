#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1502 // Element should not be on a single line
using System.Runtime.Serialization;

namespace TodoListApp.Core.Errors;

[Serializable]
public class AuthException : BaseException
{
    public AuthException() { }

    public AuthException(string message)
        : base(message) { }

    public AuthException(string message, Exception inner)
        : base(message, inner) { }

    protected AuthException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
