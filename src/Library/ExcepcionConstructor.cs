using System;
using System.Runtime.Serialization;

namespace Proyecto_Chatbot
{
    /// <summary>
    /// Constructor de excepciones para poder aplicarlas dentro de las clases.
    /// HERENCIA: Es hija de la clase Exception. Como se usó herencia, el principio
    /// aplicado es OCP.
    /// </summary>
    [Serializable]
    public class ExcepcionConstructor : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcepcionConstructor"/> class.
        /// </summary>
        public ExcepcionConstructor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcepcionConstructor"/> class.
        /// </summary>
        /// <param name="message">mensaje.</param>
        public ExcepcionConstructor(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcepcionConstructor"/> class.
        /// </summary>
        /// <param name="message">mensaje.</param>
        /// <param name="innerException">excepcion interna.</param>
        public ExcepcionConstructor(string message, Exception innerException)
        : base(message, innerException)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcepcionConstructor"/> class.
        /// </summary>
        /// <param name="info">info de serialización.</param>
        /// <param name="context">contexto.</param>
        protected ExcepcionConstructor(SerializationInfo info, StreamingContext context)
        : base(info, context) 
        {
        }
    }   
}