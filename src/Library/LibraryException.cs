using Proyecto_Chatbot;
using System;
using System.Runtime.Serialization;

namespace  Proyecto_Chatbot;

[Serializable]
internal class LibraryException : Exception
{
    public LibraryException()
    {
    }
    
    public LibraryException(string message)
    : base(message)
    {
    }
    
    public LibraryException(string message, Exception innerException)
    : base(message, innerException)
    {
    }
    
    protected LibraryException(SerializationInfo info, StreamingContext context)
    : base(info, context)
    {
    }
}