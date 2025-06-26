#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1502 // Element should not be on a single line
using System.Runtime.Serialization;

namespace TodoListApp.Core.Errors;

[Serializable]
public class MailSendingException : BaseException
{
    public MailSendingException() { }

    public MailSendingException(string message)
        : base(message) { }

    public MailSendingException(string message, Exception inner)
        : base(message, inner) { }

    protected MailSendingException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
