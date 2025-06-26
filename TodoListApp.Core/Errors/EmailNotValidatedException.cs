#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1502 // Element should not be on a single line
using System.Runtime.Serialization;

namespace TodoListApp.Core.Errors;

[Serializable]
public class EmailNotValidatedException : BaseException
{
    public EmailNotValidatedException() { }

    public EmailNotValidatedException(string message)
        : base(message) { }

    public EmailNotValidatedException(string message, Exception inner)
        : base(message, inner) { }

    protected EmailNotValidatedException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
