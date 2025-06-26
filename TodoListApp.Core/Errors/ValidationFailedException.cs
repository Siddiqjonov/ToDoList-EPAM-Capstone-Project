#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1502 // Element should not be on a single line
using System.Runtime.Serialization;

namespace TodoListApp.Core.Errors;

[Serializable]
public class ValidationFailedException : BaseException
{
    public ValidationFailedException() { }

    public ValidationFailedException(string message)
        : base(message) { }

    public ValidationFailedException(string message, Exception inner)
        : base(message, inner) { }

    protected ValidationFailedException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
