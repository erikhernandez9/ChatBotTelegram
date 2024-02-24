using System.Collections.Generic;
using System;
using System.Linq;
using Telegram.Bot.Types;

//Desabilitación de Warnings revisados
#pragma warning disable SA1623 // PropertySummaryDocumentationMustMatchAccessors

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Clase base para implementar el patrón Chain of Responsibility. En ese patrón se pasa un mensaje a través de una
    /// cadena de "handlers" que pueden procesar o no el mensaje. Cada "handler" decide si procesa el mensaje, o si se lo
    /// pasa al siguiente. Esta clase base implmementa la responsabilidad de recibir el mensaje y pasarlo al siguiente
    /// "handler" en caso que el mensaje no sea procesado. La responsabilidad de decidir si el mensaje se procesa o no, y
    /// de procesarlo, se delega a las clases sucesoras de esta clase base.
    /// </summary>
    public abstract class BaseHandler : IHandler
    {
        /// <summary>
        /// Obtiene el próximo "handler".
        /// </summary>
        /// <value>El "handler" que será invocado si este "handler" no procesa el mensaje.</value>
        public IHandler Next { get; set; }

        /// <summary>
        /// Obtiene o asigna el conjunto de palabras clave que este "handler" puede procesar.
        /// </summary>
        /// <value>Un array de palabras clave.</value>
        public string[] Keywords { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BaseHandler"/>.
        /// </summary>
        /// <param name="next">El próximo "handler".</param>
        public BaseHandler(IHandler next)
        {
            this.Next = next;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BaseHandler"/> con una lista de comandos.
        /// </summary>
        /// <param name="keywords">La lista de comandos.</param>
        /// <param name="next">El próximo "handler".</param>
        public BaseHandler(string[] keywords, IHandler next)
        {
            this.Keywords = keywords;
            this.Next = next;
        }

        /// <summary>
        /// Este método debe ser sobreescrito por las clases sucesores. La clase sucesora procesa el mensaje y retorna
        /// la respuesta al mensaje.
        /// </summary>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        protected virtual void InternalHandle(Mensaje mensaje, out string response)
        {
            throw new InvalidOperationException("Este método debe ser sobrescrito");
        }

        /// <summary>
        /// Este método puede ser sobreescrito en las clases sucesores que procesan varios mensajes cambiando de estado
        /// entre mensajes deben sobreescribir este método para volver al estado inicial. En la clase base no hace nada.
        /// </summary>
        protected virtual void InternalCancel()
        {
            // Intencionalmente en blanco.
        }

        /// <summary>
        /// Determina si este "handler" puede procesar el mensaje. En la clase base se utiliza el array
        /// <see cref="BaseHandler.Keywords"/> para buscar el texto en el mensaje ignorando mayúsculas y minúsculas. Las
        /// clases sucesores pueden sobreescribir este método para proveer otro mecanismo para determina si procesan o no
        /// un mensaje.
        /// </summary>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <returns>true si el mensaje puede ser pocesado; false en caso contrario.</returns>
        protected virtual bool CanHandle(Mensaje mensaje)
        {
            // Cuando no hay palabras clave este método debe ser sobreescrito por las clases sucesoras y por lo tanto
            // este método no debería haberse invocado.
            if (this.Keywords == null || this.Keywords.Length == 0)
            {
                throw new InvalidOperationException("No hay palabras clave que puedan ser procesadas");
            }

            return this.Keywords.Any(s => mensaje.Text.Equals(s, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Procesa el mensaje o la pasa al siguiente "handler" si existe.
        /// </summary>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        /// <returns>El "handler" que procesó el mensaje si el mensaje fue procesado; null en caso contrario.</returns>
        public IHandler Handle(Mensaje mensaje, out string response)
        {
            if (this.CanHandle(mensaje))
            {
                this.InternalHandle(mensaje, out response);
                return this;
            }
            else if (this.Next != null)
            {
                return this.Next.Handle(mensaje, out response);
            }
            else
            {
                response = String.Empty;
                return null;
            }
        }

        /// <summary>
        /// Retorna este "handler" al estado inicial. En los "handler" sin estado no hace nada. Los "handlers" que
        /// procesan varios mensajes cambiando de estado entre mensajes deben sobreescribir este método para volver al
        /// estado inicial.
        /// </summary>
        public virtual void Cancel()
        {
            this.InternalCancel();
            if (this.Next != null)
            {
                this.Next.Cancel();
            }
        }

        /// <summary>
        /// Es un diccionario para verificar el estado de los usuarios que se loguean al bot;
        /// la key es de tipo long y es la ID del usuario. El value es un string que guarda el estado
        /// del usuario que utilice el bot.
        /// </summary>
        public static Dictionary<long, List<Object>> DictVerificacionPasos { get; set; } = new Dictionary<long, List<Object>>();
    }
}