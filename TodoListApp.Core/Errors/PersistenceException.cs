#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1502 // Element should not be on a single line
using System.Runtime.Serialization;

namespace TodoListApp.Core.Errors;

[Serializable]
public class PersistenceException : BaseException
{
    public PersistenceException() { }

    public PersistenceException(string message)
        : base(message) { }

    public PersistenceException(string message, Exception inner)
        : base(message, inner) { }

    protected PersistenceException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
