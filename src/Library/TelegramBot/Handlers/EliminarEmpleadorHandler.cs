using System.Runtime.InteropServices.ComTypes;
using Telegram.Bot.Types;
using Proyecto_Chatbot.Usuarios;
using System;
using System.Collections.Generic;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Handler de la cadena de responsabilidades que implementa el comando "Eliminar empleador".
    /// Este handler forma parte de las USER STORIES, ya que es una acción que solo puede realizar
    /// el administrador. Pensamos que, como en la primera historia de usuario indica que el admin
    /// puede crear categorías, y así como establecimos que puede eliminarlas, también puede eliminar
    /// empleadores bajo algún criterio.
    /// "Cómo administrador, quiero poder indicar categorías sobre las cuales se realizarán las ofertas
    /// de servicios para que de esa forma, los trabajadoras puedan clasificarlos."
    /// </summary>
    public class EliminarEmpleadorHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando.
        /// </summary>
        public EmpleadorState State { get; private set; }

        /// <summary>
        /// Los datos que va obteniendo el comando en los diferentes estados.
        /// </summary>
        public EmpleadorData Data { get; private set; } = new EmpleadorData();

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="EliminarEmpleadorHandler"/>.
        /// </summary>
        /// <param name="next">siguiente handler.</param>
        public EliminarEmpleadorHandler(BaseHandler next)
            : base(new string[] { "Eliminar empleador" }, next)
        {
        }

        /// <inheritdoc/>
        protected override bool CanHandle(Mensaje mensaje)
        {
            if (!DictVerificacionPasos.ContainsKey(mensaje.Id))
            {
                List<Object> estado = new List<Object>();
                estado.Add(EmpleadorState.Start.ToString());
                estado.Add("EliminarEmpleadorHandler");
                DictVerificacionPasos[mensaje.Id] = estado;
            }

            if (DictVerificacionPasos[mensaje.Id][0].ToString() == EmpleadorState.Start.ToString())
            {
                return base.CanHandle(mensaje);
            }

            if (DictVerificacionPasos[mensaje.Id][1].ToString() == "EliminarEmpleadorHandler")
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
            DictVerificacionPasos[mensaje.Id][1] = "EliminarEmpleadorHandler";
            Administrador administrador = Singleton<ListaAdministradores>.Instance.SeleccionarAdministrador(mensaje.Id);
            if (administrador != null)
            {
                if (DictVerificacionPasos[mensaje.Id][0].ToString() == EmpleadorState.Start.ToString())
                {
                    DictVerificacionPasos[mensaje.Id][0] = EmpleadorState.IdEmpleador.ToString();
                    response = "Ingrese el id del empleador";
                }
                else if (DictVerificacionPasos[mensaje.Id][0].ToString() == EmpleadorState.IdEmpleador.ToString())
                {
                    this.Data.IdEmpleador = Int64.Parse(mensaje.Text);
                    Empleador empleador = Singleton<ListaEmpleadores>.Instance.SeleccionarEmpleador(this.Data.IdEmpleador);
                    if (empleador != null)
                    {
                        DictVerificacionPasos[mensaje.Id][0] = EmpleadorState.Start.ToString();
                        Singleton<ListaEmpleadores>.Instance.EliminarEmpleador(this.Data.IdEmpleador);
                        Singleton<ListaEmpleadores>.Instance.ConvertToJson();
                        response = "El empleador fue eliminado";
                        Console.WriteLine(Singleton<ListaEmpleadores>.Instance.SeleccionarEmpleador(this.Data.IdEmpleador).Nombre);
                    }
                    else
                    {
                        response = "El empleador no existe. Volve a ingresar el id";
                    }
                }
                else
                {
                    response = string.Empty;
                }
            }
            else
            {
                if (DictVerificacionPasos[mensaje.Id][0].ToString() == EmpleadorState.Start.ToString())
                {
                    response = $"No tenés permisos para realizar esta operación. {mensaje.Id}";
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
            this.State = EmpleadorState.Start;
            this.Data = new EmpleadorData();
        }

        /// <summary>
        /// Indica los diferentes estados que puede tener el comando.
        /// </summary>
        public enum EmpleadorState
        {
            /// <summary>
            /// El estado inicial del comando. En este estado el comando pide los datos iniciales del empleador y pasa al
            /// siguiente estado.
            /// </summary>
            Start,
            /// <summary>
            /// El estado del nombre del id del empleador.
            /// </summary>
            IdEmpleador
        }

        /// <summary>
        /// Constructor de la clase EmpleadorData. Contiene getters y setters.
        /// </summary>
        public class EmpleadorData
        {
            /// <summary>
            /// Representa el id del empleador a eliminar.
            /// </summary>
            public long IdEmpleador { get; set; }
        }
    }
}