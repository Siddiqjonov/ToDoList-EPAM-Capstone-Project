#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1502 // Element should not be on a single line
using System.Runtime.Serialization;

namespace TodoListApp.Core.Errors;

[Serializable]
public class EntityNotFoundException : BaseException
{
    public EntityNotFoundException() { }

    public EntityNotFoundException(string message)
        : base(message) { }

    public EntityNotFoundException(string message, Exception inner)
        : base(message, inner) { }

    protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
