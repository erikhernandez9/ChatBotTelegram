using Telegram.Bot.Types;
using Proyecto_Chatbot.Catalogos;
using Proyecto_Chatbot.Usuarios;
using System;
using System.Collections.Generic;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Handler del patrón de cadena de responsabilidades que implementa el comando "Eliminar oferta".
    /// Este handler forma parte de las USER STORIES, ya que es una acción que solo puede realizar
    /// el administrador. Se eliminan ofertas en caso de que sean inadecuadas.
    /// "Como administrador, quiero poder dar de baja ofertas de servicios, avisando al oferente para
    /// que de esa forma, pueda evitar ofertas inadecudas".
    /// </summary>
    public class EliminarOfertaHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando.
        /// </summary>
        public OfertaState State { get; private set; }

        /// <summary>
        /// Los datos que va obteniendo el comando en los diferentes estados.
        /// </summary>
        public OfertaData Data { get; private set; } = new OfertaData();

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="EliminarOfertaHandler"/>.
        /// </summary>
        /// <param name="next">siguiente handler.</param>
        public EliminarOfertaHandler(BaseHandler next)
            : base(new string[] { "Eliminar oferta" }, next)
        {
        }

        /// <inheritdoc/>
        protected override bool CanHandle(Mensaje mensaje)
        {
            if (!DictVerificacionPasos.ContainsKey(mensaje.Id))
            {
                List<Object> estado = new List<Object>();
                estado.Add(OfertaState.Start.ToString());
                estado.Add("EliminarOfertaHandler");
                DictVerificacionPasos[mensaje.Id] = estado;
            }

            if (DictVerificacionPasos[mensaje.Id][0].ToString() == OfertaState.Start.ToString())
            {
                return base.CanHandle(mensaje);
            }

            if (DictVerificacionPasos[mensaje.Id][1].ToString() == "EliminarOfertaHandler")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Verifica los estados y responde según el estado, verificandolo con el diccionario.
        /// </summary>
        /// <param name="mensaje">mensaje.</param>
        /// <param name="response">contestación al mensaje.</param>
        protected override void InternalHandle(Mensaje mensaje, out string response)
        {
            DictVerificacionPasos[mensaje.Id][1] = "EliminarOfertaHandler";
            Administrador administrador = Singleton<ListaAdministradores>.Instance.SeleccionarAdministrador(mensaje.Id);
            if (administrador != null)
            {
                if (DictVerificacionPasos[mensaje.Id][0].ToString() == OfertaState.Start.ToString())
                {
                    DictVerificacionPasos[mensaje.Id][0] = OfertaState.IdOferta.ToString();
                    response = $" {Singleton<CatalogoOfertas>.Instance.Imprimir()}\nIngrese la ID de la oferta deseada";
                }
                else if (DictVerificacionPasos[mensaje.Id][0].ToString() == OfertaState.IdOferta.ToString())
                {
                    this.Data.IdOferta = Int32.Parse(mensaje.Text) - 1;
                    DictVerificacionPasos[mensaje.Id][0] = OfertaState.Start.ToString();
                    Singleton<CatalogoOfertas>.Instance.EliminarOferta(this.Data.IdOferta);
                    Singleton<CatalogoOfertas>.Instance.ConvertToJson();
                    response = "La oferta fue eliminada";
                }
                else
                {
                    response = string.Empty;
                }
            }
            else
            {
                if (DictVerificacionPasos[mensaje.Id][0].ToString() == OfertaState.Start.ToString())
                {
                    response = "No tenés permisos para realizar esta operación.";
                }
                else
                {
                    response = string.Empty;
                }
            }
        }

        /// <summary>
        /// Método que permite cambiar estados dentro de la clase a la hora de tener que modificarlos
        /// entre mensajes.
        /// </summary>
        protected override void InternalCancel()
        {
            this.State = OfertaState.Start;
            this.Data = new OfertaData();
        }

        /// <summary>
        /// Indica los diferentes estados que puede tener el comando.
        /// </summary>
        public enum OfertaState
        {
            /// <summary>
            /// El estado inicial del comando. En este estado el comando pide los datos iniciales de la oferta y pasa al
            /// siguiente estado.
            /// </summary>
            Start,
            /// <summary>
            /// El estado del id de la oferta.
            /// </summary>
            IdOferta,
        }

        /// <summary>
        /// Constructor de la clase OfertaData. Contiene getters y setters.
        /// </summary>
        public class OfertaData
        {
            /// <summary>
            /// Representa el id de la oferta a eliminar.
            /// </summary>
            public int IdOferta { get; set; }
        }
    }
}