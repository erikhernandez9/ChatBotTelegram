using Telegram.Bot.Types;
using System.Text;
using System.Collections.Generic;
using Proyecto_Chatbot.Catalogos;
using System.Linq;
using System;
using Newtonsoft.Json;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "listar categorias". Lista todas las categorías.
    /// Es un handler de prueba para observar.
    /// </summary>
    public class ListarCategoriasHandler : BaseHandler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase.
        /// </summary>
        /// <param name="next">El próximo "handler".</param>
        public ListarCategoriasHandler(BaseHandler next)
            : base(next)
        {
            this.Keywords = new string[] { "Listar categorias" };
        }

        /// <summary>
        /// Procesa el mensaje "catalogo" y retorna true; retorna false en caso contrario.
        /// </summary>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        protected override void InternalHandle(Mensaje mensaje, out string response)
        {
            Singleton<CatalogoCategorias>.Instance.LoadFromJson(Singleton<CatalogoCategorias>.Instance.ShowFile());
            response = $"Lista: {Singleton<CatalogoCategorias>.Instance.Imprimir()}\n";
        }
    }
}