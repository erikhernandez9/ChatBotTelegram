using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Nito.AsyncEx;
using Proyecto_Chatbot;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "foto".
    /// </summary>
    public class PhotoHandler : BaseHandler
    {
        private TelegramBotClient bot;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PhotoHandler"/>. Esta clase procesa el mensaje "foto".
        /// </summary>
        /// <param name="next">El próximo "handler".</param>
        /// <param name="bot">El bot para enviar la foto.</param>
        public PhotoHandler(TelegramBotClient bot, BaseHandler next)
            : base(new string[] { "foto" }, next)
        {
            this.bot = bot;
        }

        /// <summary>
        /// Procesa el mensaje "foto" y retorna true; retorna false en caso contrario.
        /// </summary>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        protected override void InternalHandle(Mensaje mensaje, out string response)
        {
            AsyncContext.Run(() => this.SendProfileImage(mensaje));
            response = String.Empty;
        }

        /// <summary>
        /// Envía una imagen como respuesta al mensaje recibido. Como ejemplo enviamos siempre la misma foto.
        /// </summary>
        private async Task SendProfileImage(Mensaje mensaje)
        {
            // Can be null during testing
            if (this.bot != null)
            {
                await this.bot.SendChatActionAsync(mensaje.Id, ChatAction.UploadPhoto);

                const string filePath = @"profile.jpeg";
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();

                await this.bot.SendPhotoAsync(
                    chatId: mensaje.Id,
                    photo: new InputOnlineFile(fileStream, fileName),
                    caption: "Te ves bien!");
            }
        }
    }
}