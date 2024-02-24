using System;
using System.Linq;
using Telegram.Bot.Types;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "ayuda".
    /// </summary>
    public class HelpHandler : BaseHandler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="HelpHandler"/>. Esta clase procesa el mensaje "ayuda".
        /// </summary>
        /// <param name="next">El próximo "handler".</param>
        public HelpHandler(BaseHandler next)
            : base(next)
        {
            this.Keywords = new string[] { "ayuda", "Ayuda" };
        }

        /// <summary>
        /// Procesa el mensaje "ayuda" y retorna true; retorna false en caso contrario.
        /// </summary>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        protected override void InternalHandle(Mensaje mensaje, out string response)
        {
            response = $"Lista de comandos: \n Hola - Saludo \n Chau - Cerrar bot \n Ofertas - Opciones de ordenado de ofertas \n Categorias - lista de categorías. \n Mis contrataciones - Muestra lista de contrataciones vigentes. \n Distancia - distancia entre dos ubicaciones. \n Dirección - Carga su dirección actual.";
        }
    }
}