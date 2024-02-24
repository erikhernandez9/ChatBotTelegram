using Telegram.Bot.Types;
using Proyecto_Chatbot.Usuarios;
using System;
using System.Collections.Generic;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Handler de la cadena de responsabilidades que implementa el comando "Eliminar trabajador".
    /// Este handler forma parte de las USER STORIES, ya que es una acción que solo puede realizar
    /// el administrador. Pensamos que, como en la primera historia de usuario indica que el admin
    /// puede crear categorías, y así como establecimos que puede eliminarlas, también puede eliminar
    /// trabajadores bajo algún criterio.
    /// "Cómo administrador, quiero poder indicar categorías sobre las cuales se realizarán las ofertas
    /// de servicios para que de esa forma, los trabajadoras puedan clasificarlos".
    /// </summary>
    public class EliminarTrabajadorHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando.
        /// </summary>
        public TrabajadorState State { get; private set; }

        /// <summary>
        /// Los datos que va obteniendo el comando en los diferentes estados.
        /// </summary>
        public TrabajadorData Data { get; private set; } = new TrabajadorData();

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="EliminarTrabajadorHandler"/>.
        /// </summary>
        /// <param name="next">siguiente handler.</param>
        public EliminarTrabajadorHandler(BaseHandler next)
            : base(new string[] { "Eliminar trabajador" }, next)
        {
        }

        /// <inheritdoc/>
        protected override bool CanHandle(Mensaje mensaje)
        {
            if (!DictVerificacionPasos.ContainsKey(mensaje.Id))
            {
                List<Object> estado = new List<Object>();
                estado.Add(TrabajadorState.Start.ToString());
                estado.Add("EliminarTrabajadorHandler");
                DictVerificacionPasos[mensaje.Id] = estado;
            }

            if (DictVerificacionPasos[mensaje.Id][0].ToString() == TrabajadorState.Start.ToString())
            {
                return base.CanHandle(mensaje);
            }

            if (DictVerificacionPasos[mensaje.Id][1].ToString() == "EliminarTrabajadorHandler")
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
            DictVerificacionPasos[mensaje.Id][1] = "EliminarTrabajadorHandler";
            if (DictVerificacionPasos[mensaje.Id][0].ToString() == TrabajadorState.Start.ToString())
            {
                DictVerificacionPasos[mensaje.Id][0] = TrabajadorState.IdTrabajador.ToString();
                response = "Ingrese el id del trabajador";
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString() == TrabajadorState.IdTrabajador.ToString())
            {

                this.Data.IdTrabajador = long.Parse(mensaje.Text);
                DictVerificacionPasos[mensaje.Id][0] = TrabajadorState.Start.ToString();
                Singleton<ListaTrabajadores>.Instance.EliminarTrabajador(this.Data.IdTrabajador);
                Singleton<ListaTrabajadores>.Instance.ConvertToJson();
                response = "El trabajador fue eliminado";
            }
            else
            {
                response = string.Empty;
            }
        }

        /// <summary>
        /// Método que permite cambiar estados dentro de la clase a la hora de tener que modificarlos
        /// entre mensajes.
        /// </summary>
        protected override void InternalCancel()
        {
            this.State = TrabajadorState.Start;
            this.Data = new TrabajadorData();
        }

        /// <summary>
        /// Indica los diferentes estados que puede tener el comando.
        /// </summary>
        public enum TrabajadorState
        {
            /// <summary>
            /// El estado inicial del comando. En este estado el comando pide los datos iniciales del trabajador y pasa al
            /// siguiente estado.
            /// </summary>
            Start,
            /// <summary>
            /// El estado del id del trabajador.
            /// </summary>
            IdTrabajador
        }

        /// <summary>
        /// Constructor de la clase TrabajadorData. Contiene getters y setters.
        /// </summary>
        public class TrabajadorData
        {
            /// <summary>
            /// Representa el id del trabajador a eliminar.
            /// </summary>
            public long IdTrabajador { get; set; }
        }
    }
}