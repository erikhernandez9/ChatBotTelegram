using Telegram.Bot.Types;
using Proyecto_Chatbot.Catalogos;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "Mis contrataciones".
    /// </summary>
    public class CatalogoContratacionesHandler : BaseHandler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CatalogoContratacionesHandler"/>. Esta clase procesa el mensaje "Catalogo Contrataciones".
        /// </summary>
        /// <param name="next">El próximo "handler".</param>
        public CatalogoContratacionesHandler(BaseHandler next)
            : base(next)
        {
            this.Keywords = new string[] { "Mis contrataciones" };
        }

        /// <summary>
        /// Procesa el mensaje "mis contrataciones" y retorna true; retorna false en caso contrario.
        /// Muestra la lista de contrataciones vigentes.
        /// </summary>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        protected override void InternalHandle(Mensaje mensaje, out string response)
        {
            response = $"Lista de contrataciones vigentes: \n {Singleton<CatalogoContratacion>.Instance.Imprimir()}\n";
        }
    }
}