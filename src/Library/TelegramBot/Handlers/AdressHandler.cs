using Telegram.Bot.Types;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "dirección".
    /// </summary>
    public class AddressHandler : BaseHandler
    {
        /// <summary>
        /// El estado del comando.
        /// </summary>
        public AddressState State { get; set; }

        /// <summary>
        /// Los datos que va obteniendo el comando en los diferentes estados.
        /// </summary>
        public AddressData Data { get; set; } = new AddressData();

        // Un buscador de direcciones. Permite que la forma de encontrar una dirección se determine en tiempo de
        // ejecución: en el código final se asigna un objeto que use una API para buscar direcciones; y en los casos de
        // prueba se asigne un objeto que retorne un resultado que puede ser configurado desde el caso de prueba.
        private IAddressFinder finder;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="AddressHandler"/>.
        /// </summary>
        /// <param name="finder">Un buscador de direcciones.</param>
        /// <param name="next">El próximo "handler".</param>
        public AddressHandler(IAddressFinder finder, BaseHandler next)
            : base(new string[] { "dirección", "direccion" }, next)
        {
            this.finder = finder;
            this.State = AddressState.Start;
        }

        /// <inheritdoc/>
        protected override bool CanHandle(Mensaje mensaje)
        {
            if (this.State ==  AddressState.Start)
            {
                return base.CanHandle(mensaje);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Procesa todos los mensajes y retorna true siempre.
        /// </summary>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado indicando que el mensaje no pudo se procesado.</param>
        protected override void InternalHandle(Mensaje mensaje, out string response)
        {
            if (this.State ==  AddressState.Start)
            {
                // En el estado Start le pide la dirección y pasa al estado AddressPrompt.
                this.State = AddressState.AddressPrompt;
                response = "Vamos a buscar una dirección. Decime qué dirección querés buscar por favor";
            }
            else if ((this.State == AddressState.AddressPrompt) && (this.finder != null))
            {
                // En el estado AddressPrompt el mensaje recibido es la respuesta con la dirección.
                this.Data.Address = mensaje.Text;

                this.Data.Result = this.finder.GetLocation(this.Data.Address);

                if (this.Data.Result.Found)
                {
                    // Si encuentra la dirección pasa nuevamente al estado Initial.
                    response = $"La dirección es en {this.Data.Result.Latitude},{this.Data.Result.Longitude}";
                    this.State = AddressState.Start;
                }
                else
                {
                    // Si no encuentra la dirección se la pide de nuevo y queda en el estado AddressPrompt.
                    response = "No encuentro la dirección. Decime qué dirección querés buscar por favor";
                }
            }
            else if ((this.State == AddressState.AddressPrompt) && (this.finder == null))
            {
                // En el estado AddressPrompt si o hay un buscador de direcciones hay que responder que hubo un error
                // y volver al estado inicial.
                response = "Error interno de configuración, no puedo buscar direcciones";
                this.State = AddressState.Start;
            }
            else
            {
                response = string.Empty;
            }
        }

        /// <summary>
        /// Retorna este "handler" al estado inicial.
        /// </summary>
        protected override void InternalCancel()
        {
            this.State = AddressState.Start;
            this.Data = new AddressData();
        }

        /// <summary>
        /// Indica los diferentes estados que puede tener el comando AddressHandler.
        /// </summary>
        public enum AddressState
        {
            /// <summary>
            /// El estado inicial del comando. En este estado el comando pide los datos del trabajador y pasa al
            /// siguiente estado.
            /// </summary>
            Start,
            /// <summary>
            /// El estado donde se guarda la dirección.
            /// </summary>
            AddressPrompt
        }

        /// <summary>
        /// Representa los datos que va obteniendo el comando AddressHandler en los diferentes estados.
        /// </summary>
        public class AddressData
        {
            /// <summary>
            /// La dirección que se ingresó en el estado AddressState.AddressPrompt.
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// El resultado de la búsqueda de la dirección ingresada.
            /// </summary>
            public IAddressResult Result { get; set; }
        }
    }
}