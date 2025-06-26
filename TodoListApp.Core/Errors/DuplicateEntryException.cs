#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1502 // Element should not be on a single line
using System.Runtime.Serialization;

namespace TodoListApp.Core.Errors;

[Serializable]
public class DuplicateEntryException : BaseException // NotAllowedException
{
    public DuplicateEntryException() { }

    public DuplicateEntryException(string message)
        : base(message) { }

    public DuplicateEntryException(string message, Exception inner)
        : base(message, inner) { }

    protected DuplicateEntryException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
