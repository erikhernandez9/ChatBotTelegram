//--------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//--------------------------------------------------------------------------------
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Proyecto_Chatbot.Catalogos;
using Proyecto_Chatbot.Locations.Client;
using Proyecto_Chatbot.TelegramBot;
using Proyecto_Chatbot.Usuarios;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Proyecto_Chatbot
{
    /// <summary>
    /// Un programa que implementa un bot de Telegram.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// instancia del bot.
        /// </summary>
        private static TelegramBotClient bot;
        
        /// <summary>
        /// token del bot.
        /// </summary>
        private static string token;

       /// <summary>
       /// Esta clase es un POCO (Plain Old CLR Object), no depende de nada en especial y es una clase simple.
       /// </summary>
        private class BotSecret
        {
            public string Token { get; set; }
        }

        /// <summary>
        /// Una interfaz requerida para configurar el servicio que lee el token secreto del bot.
        /// </summary>
        private interface ISecretService
        {
            string Token { get; }
        }

        /// <summary>
        /// Una clase que provee el servicio de leer el token secreto del bot.
        /// </summary>
        private class SecretService : ISecretService
        {
            private readonly BotSecret _secrets;

            public SecretService(IOptions<BotSecret> secrets)
            {
                this._secrets = secrets.Value ?? throw new ArgumentNullException(nameof(secrets));
            }

            public string Token
            {
                get { return this._secrets.Token; }
            }
        }

        private static void Start()
        {
            // Lee una variable de entorno NETCORE_ENVIRONMENT que si no existe o tiene el valor 'development' indica
            // que estamos en un ambiente de desarrollo.
            var developmentEnvironment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            var isDevelopment =
                string.IsNullOrEmpty(developmentEnvironment) ||
                developmentEnvironment.ToLower() == "development";

            var builder = new ConfigurationBuilder();
            builder
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // En el ambiente de desarrollo el token secreto del bot se toma de la configuración secreta.
            if (isDevelopment)
            {
                builder.AddUserSecrets<Program>();
            }

            var configuration = builder.Build();

            IServiceCollection services = new ServiceCollection();

            // Mapeamos la implementación de las clases para  inyección de dependencias.
            services
                .Configure<BotSecret>(configuration.GetSection(nameof(BotSecret)))
                .AddSingleton<ISecretService, SecretService>();

            var serviceProvider = services.BuildServiceProvider();
            var revealer = serviceProvider.GetService<ISecretService>();
            token = revealer.Token;
        }

        private static IHandler firstHandler;

        /// <summary>
        /// Punto de entrada al programa.
        /// </summary>
        public static void Main()
        {
            Start();
            if (System.IO.File.Exists("Categorias.json"))
            {
                Singleton<CatalogoCategorias>.Instance.LoadFromJson(Singleton<CatalogoCategorias>.Instance.ShowFile());
            }

            if (System.IO.File.Exists("Trabajadores.json"))
            {
                Singleton<ListaTrabajadores>.Instance.LoadFromJson(Singleton<ListaTrabajadores>.Instance.ShowFile());
            }

            if (System.IO.File.Exists("Empleadores.json"))
            {
                Singleton<ListaEmpleadores>.Instance.LoadFromJson(Singleton<ListaEmpleadores>.Instance.ShowFile());
            }

            if (System.IO.File.Exists("Administradores.json"))
            {
                Singleton<ListaAdministradores>.Instance.LoadFromJson(Singleton<ListaAdministradores>.Instance.ShowFile());
            }

            if (System.IO.File.Exists("Contrataciones.json"))
            {
                Singleton<CatalogoContratacion>.Instance.LoadFromJson(Singleton<CatalogoContratacion>.Instance.ShowFile());
            }

            if (System.IO.File.Exists("Ofertas.json"))
            {
                Singleton<CatalogoOfertas>.Instance.LoadFromJson(Singleton<CatalogoOfertas>.Instance.ShowFile());
            }
            Singleton<CatalogoContratacion>.Instance.ContratacionesNeutras();

            LocationApiClient client = new LocationApiClient();
            bot = new TelegramBotClient(token);
            firstHandler =
                new HelloHandler(
                new GoodByeHandler(
                new EliminarCategoriaHandler(
                new CrearCategoriaHandler(
                new HelpHandler(
                new ListarCategoriasHandler(
                new CrearContratacionHandler(
                new CatalogoContratacionesHandler(
                new CalificarTrabajadorHandler(
                new CalificarEmpleadorHandler(
                new EliminarOfertaHandler(
                new CrearCategoriaHandler(
                new CrearOfertaLaboralHandler(
                new AgregarTrabajadorHandler(
                new EliminarTrabajadorHandler(
                new AgregarEmpleadorHandler(
                new EliminarEmpleadorHandler(
                new StartHandler(
                new AddressHandler(new AddressFinder(client),
                new DistanceHandler(new DistanceCalculator(client),
                null))))))))))))))))))));

            var cts = new CancellationTokenSource();

            // Comenzamos a escuchar mensajes. Esto se hace en otro hilo (en background). El primer método
            // HandleUpdateAsync es invocado por el bot cuando se recibe un mensaje. El segundo método HandleErrorAsync
            // es invocado cuando ocurre un error.
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions()
                {
                    AllowedUpdates = Array.Empty<UpdateType>(),
                },
                cts.Token);
            Console.WriteLine($"Bot is up!");

            // Esperamos a que el usuario aprete Enter en la consola para terminar el bot.
            Console.ReadLine();

            // Terminamos el bot.
            cts.Cancel();
        }

        /// <summary>
        /// Maneja las actualizaciones del bot (todo lo que llega), incluyendo mensajes, ediciones de mensajes,
        /// respuestas a botones, etc. En este ejemplo sólo manejamos mensajes de texto.
        /// </summary>
        /// <param name="botClient">el cliente del bot.</param>
        /// <param name="update">El update del mensaje.</param>
        /// <param name="cancellationToken">cancellation token para la excepción.</param>
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                // Sólo respondemos a mensajes de texto
                if (update.Type == UpdateType.Message)
                {
                    await HandleMessageReceived(botClient, update.Message);
                }
            }
            catch (Exception e)
            {
                await HandleErrorAsync(botClient, e, cancellationToken);
            }
        }

        /// <summary>
        /// Maneja los mensajes que se envían al bot a través de handlers de una chain of responsibility.
        /// </summary>
        /// <param name="botClient">el cliente del bot.</param>
        /// <param name="message">El mensaje recibido.</param>
        private static async Task HandleMessageReceived(ITelegramBotClient botClient, Message message)
        {
            Console.WriteLine($"Received a message from {message.From.FirstName} saying: {message.Text}");

            string response = string.Empty;

            Mensaje mensaje = new Mensaje(message.Chat.Id, message.Text);

            firstHandler.Handle(mensaje, out response);

            if (!string.IsNullOrEmpty(response))
            {
                await bot.SendTextMessageAsync(message.Chat.Id, response);
            }
        }

        /// <summary>
        /// Manejo de excepciones. Por ahora simplemente la imprimimos en la consola.
        /// </summary>
        /// <param name="botClient">El bot.</param>
        /// <param name="exception">Excepción correspondiente.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(exception.Message);
            return Task.CompletedTask;
        }
    }
}
