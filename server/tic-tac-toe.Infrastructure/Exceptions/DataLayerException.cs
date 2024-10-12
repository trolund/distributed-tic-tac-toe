using System.Runtime.Serialization;

namespace Manufacturing.Infrastructure.Exceptions;

[Serializable]
public class DataLayerException : Exception
{
    public DataLayerException()
    {
    }

    public DataLayerException(string message)
        : base(message)
    {
    }

    public DataLayerException(string message, Exception inner)
        : base(message, inner)
    {
    }

    protected DataLayerException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}