#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1502 // Element should not be on a single line
using System.Runtime.Serialization;

namespace TodoListApp.Core.Errors;

[Serializable]
public class DatabaseException : BaseException
{
    public DatabaseException() { }

    public DatabaseException(string message)
        : base(message) { }

    public DatabaseException(string message, Exception inner)
        : base(message, inner) { }

    protected DatabaseException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
