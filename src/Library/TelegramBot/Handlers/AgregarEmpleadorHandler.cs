using Telegram.Bot.Types;
using Proyecto_Chatbot.Usuarios;
using System;
using System.Collections.Generic;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "Agregar empleador" 
    /// y agrega un empleador con todos sus datos. Este handler es un USER STORY. Permite que un usuario se registre
    /// en la plataforma, ingresando el mensaje "Quiero registrarme como empleador." Se solicitan todos los datos personales
    /// necesarios durante el registro y se guardan en una lista de empleadores.
    /// 
    /// "Como empleador, quiero registrarme en la plataforma, indicando mis datos personales e información de contacto para
    ///  que de esa forma, pueda proveer información de contacto a quienes quieran contratar mis servicios."
    /// </summary>
    public class AgregarEmpleadorHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando.
        /// </summary>
        /// <value></value>
        public EmpleadorState State { get; private set; }

        /// <summary>
        /// Los datos que va obteniendo el comando en los diferentes estados.
        /// </summary>
        public EmpleadorData Data { get; private set; } = new EmpleadorData();

        /// <summary>
        /// Inicializa una nueva instancia de la clase. <see cref="AgregarEmpleadorHandler"/>.
        /// </summary>
        /// <param name="next">siguiente handler.</param>
        public AgregarEmpleadorHandler(BaseHandler next)
            : base(new string[] { "Quiero registrarme como empleador" }, next)
        {
        }

        /// <inheritdoc/>
        protected override bool CanHandle(Mensaje mensaje)
        {
            if (!DictVerificacionPasos.ContainsKey(mensaje.Id))
            {
                List<Object> estado = new List<Object>();
                estado.Add(EmpleadorState.Start.ToString());
                estado.Add("AgregarEmpleadorHandler");
                DictVerificacionPasos[mensaje.Id] = estado;
            }

            if (DictVerificacionPasos[mensaje.Id][0].ToString() == EmpleadorState.Start.ToString())
            {
                return base.CanHandle(mensaje);
            }

            if (DictVerificacionPasos[mensaje.Id][1].ToString() == "AgregarEmpleadorHandler")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Verifica los estados y responde según el estado del empleador, verificandolo con el diccionario.
        /// </summary>
        /// <param name="mensaje">mensaje.</param>
        /// <param name="response">contestación al mensaje.</param>
        protected override void InternalHandle(Mensaje mensaje, out string response)
        {
            DictVerificacionPasos[mensaje.Id][1] = "AgregarEmpleadorHandler";
            this.Data.IdEmpleador = mensaje.Id;
            if (DictVerificacionPasos[mensaje.Id][0].ToString() ==  EmpleadorState.Start.ToString())
            {
                DictVerificacionPasos[mensaje.Id][0] = EmpleadorState.NombreEmpleador.ToString();
                response = "Bueno.. Ingrese tu nombre";
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString()  == EmpleadorState.NombreEmpleador.ToString())
            {
                this.Data.NombreEmpleador = mensaje.Text;
                DictVerificacionPasos[mensaje.Id][0] = EmpleadorState.EmailEmpleador.ToString();
                response = "Ingrese tu email";
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString()  ==  EmpleadorState.EmailEmpleador.ToString())
            {
                this.Data.EmailEmpleador = mensaje.Text;
                DictVerificacionPasos[mensaje.Id][0] = EmpleadorState.TelefonoEmpleador.ToString();
                response = "Ingrese tu numero de telefono";
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString()  ==  EmpleadorState.TelefonoEmpleador.ToString())
            {
                this.Data.TelefonoEmpleador = mensaje.Text;
                DictVerificacionPasos[mensaje.Id][0] = EmpleadorState.UbicacionEmpleador.ToString();
                response = "Ingrese tu dirección";
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString()  == EmpleadorState.UbicacionEmpleador.ToString())
            {
                this.Data.UbicacionEmpleador = mensaje.Text;
                DictVerificacionPasos[mensaje.Id][0] = EmpleadorState.Start.ToString();
                try
                {
                    Singleton<ListaEmpleadores>.Instance.AgregarEmpleador(this.Data.NombreEmpleador, this.Data.EmailEmpleador, this.Data.TelefonoEmpleador, this.Data.IdEmpleador, this.Data.UbicacionEmpleador);
                    response = "Genial! Te has registrado como empleador";
                }
                catch(ExcepcionConstructor)
                {
                    response = "Valores incorrectos. Escribi 'Quiero registrarme como empleador' para volver a registrarte";
                }
            }
            else
            {
                response = string.Empty;
            }
        }

        /// <summary>
        /// Método para cambiar estados dentro de la clase a la hora de tener que modificarlos
        /// entre mensajes.
        /// </summary>
        protected override void InternalCancel()
        {
            this.State = EmpleadorState.Start;
            this.Data = new EmpleadorData();
        }

        /// <summary>
        /// Indica los diferentes estados que puede tener el comando AgregarEmpleador.
        /// </summary>
        public enum EmpleadorState
        {
            /// <summary>
            /// El estado inicial del comando. En este estado el comando pide los datos del empleador y pasa al
            /// siguiente estado.
            /// </summary>
            Start,
            /// <summary>
            /// El estado del nombre del empleador. En este estado el comando pide los datos del empleador y pasa al
            /// siguiente estado.
            /// </summary>
            NombreEmpleador,
            /// <summary>
            /// El estado del email del empleador. En este estado el comando pide los datos del empleador y pasa al
            /// siguiente estado.
            /// </summary>
            EmailEmpleador,
            /// <summary>
            /// El estado del telefono del empleador. En este estado el comando pide los datos del empleador y pasa al
            /// siguiente estado.
            /// </summary>
            TelefonoEmpleador,
            /// <summary>
            /// El estado del id del empleador. En este estado el comando pide los datos del empleador y pasa al
            /// siguiente estado.
            /// </summary>
            IdEmpleador,
            /// <summary>
            /// El estado de la ubicación del empleador. En este estado el comando pide los datos del empleador.
            /// </summary>
            UbicacionEmpleador
        }

        /// <summary>
        /// Representa los datos que va obteniendo el comando AgregarEmpleador en los diferentes estados.
        /// </summary>
        public class EmpleadorData
        {
            /// <summary>
            /// Representa el nombre del empleador obtenido en AgregarEmpleador.
            /// </summary>
            public string NombreEmpleador { get; set; }

            /// <summary>
            /// Representa el email del empleador obtenido en AgregarEmpleador.
            /// </summary>
            public string EmailEmpleador { get; set; }

            /// <summary>
            /// Representa el teléfono del empleador obtenido en AgregarEmpleador.
            /// </summary>
            public string TelefonoEmpleador { get; set; }

            /// <summary>
            /// Representa la ubicación del empleador obtenido en AgregarEmpleador.
            /// </summary>
            public string UbicacionEmpleador { get; set; }

            /// <summary>
            /// Representa el id del empleador obtenido en AgregarEmpleador.
            /// </summary>
            public long IdEmpleador { get; set; }

        }
    }
}