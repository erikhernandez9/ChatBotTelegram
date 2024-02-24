using Telegram.Bot.Types;
using Proyecto_Chatbot.Usuarios;
using System;
using System.Collections.Generic;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "Agregar trabajador" 
    /// y agrega un empleador con todos sus datos. Este handler es un USER STORY. Permite que un usuario se registre
    /// en la plataforma como trabajador, ingresando el mensaje "Quiero registrarme como trabajador." Se solicitan todos los datos personales
    /// necesarios durante el registro y se guardan en una lista de trabajadores.
    /// 
    /// "Como trabajador, quiero registrarme en la plataforma, indicando mis datos personales e información de contacto para
    ///  que de esa forma, pueda proveer información de contacto a quienes quieran contratar mis servicios."
    /// </summary>
    public class AgregarTrabajadorHandler : BaseHandler
    {
        /// <summary>
        /// El estado del comando.
        /// </summary>
        public TrabajadorState State { get; private set; }
        
        /// <summary>
        /// Los datos que va obteniendo el comando en los diferentes estados.
        /// </summary>
        public TrabajadorData Data { get; private set; } = new TrabajadorData();

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="AgregarTrabajadorHandler"/>.
        /// </summary>
        /// <param name="next">siguiente handler.</param>
        public AgregarTrabajadorHandler(BaseHandler next)
            : base(new string[] { "Quiero registrarme como trabajador" }, next)
        {
        }

        /// <inheritdoc/>
        protected override bool CanHandle(Mensaje mensaje)
        {
            if (!DictVerificacionPasos.ContainsKey(mensaje.Id))
            {
                List<Object> estado = new List<Object>();
                estado.Add(TrabajadorState.Start.ToString());
                estado.Add("AgregarTrabajadorHandler");
                DictVerificacionPasos[mensaje.Id] = estado;
            }
            if (DictVerificacionPasos[mensaje.Id][0].ToString() == TrabajadorState.Start.ToString())
            {
                return base.CanHandle(mensaje);
            }
            if (DictVerificacionPasos[mensaje.Id][1].ToString() == "AgregarTrabajadorHandler")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Verifica los estados y responde según el estado del trabajador, verificandolo con el diccionario.
        /// </summary>
        /// <param name="mensaje">mensaje.</param>
        /// <param name="response">contestación al mensaje.</param>
        protected override void InternalHandle(Mensaje mensaje, out string response)
        {
            DictVerificacionPasos[mensaje.Id][1] = "AgregarTrabajadorHandler";
            if (DictVerificacionPasos[mensaje.Id][0].ToString() ==  TrabajadorState.Start.ToString())
            {
                DictVerificacionPasos[mensaje.Id][0] = TrabajadorState.NombreTrabajador.ToString();
                response = "Bueno.. Ingrese tu nombre";
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString() == TrabajadorState.NombreTrabajador.ToString())
            {
                this.Data.NombreTrabajador = mensaje.Text;
                DictVerificacionPasos[mensaje.Id][0] = TrabajadorState.EmailTrabajador.ToString();
                response = "Ingrese tu email";
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString() ==  TrabajadorState.EmailTrabajador.ToString())
            {
                this.Data.EmailTrabajador = mensaje.Text;
                DictVerificacionPasos[mensaje.Id][0] = TrabajadorState.TelefonoTrabajador.ToString();
                response = "Ingrese tu numero de telefono";
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString() ==  TrabajadorState.TelefonoTrabajador.ToString())
            {
                this.Data.TelefonoTrabajador = mensaje.Text;
                DictVerificacionPasos[mensaje.Id][0] = TrabajadorState.UbicacionTrabajador.ToString();
                response = "Ingrese tu dirección";
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString() == TrabajadorState.UbicacionTrabajador.ToString())
            {
                this.Data.UbicacionTrabajador = mensaje.Text;
                DictVerificacionPasos[mensaje.Id][0] = TrabajadorState.Start.ToString();
                this.Data.IdTrabajador = mensaje.Id;
                try
                {
                    Singleton<ListaTrabajadores>.Instance.AgregarTrabajador(this.Data.NombreTrabajador, this.Data.EmailTrabajador, this.Data.TelefonoTrabajador, this.Data.IdTrabajador, this.Data.UbicacionTrabajador);
                    response = "Genial! Te has registrado como trabajador";
                }
                catch(ExcepcionConstructor)
                {
                    response = "Valores incorrectos. Escribi 'Quiero registrarme como trabajador' para volver a registrarte";
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
            this.State = TrabajadorState.Start;
            this.Data = new TrabajadorData();
        }
        /// <summary>
        /// Indica los diferentes estados que puede tener el comando AgregarEmpleador.
        /// </summary>
        public enum TrabajadorState
        {
            /// <summary>
            /// El estado inicial del comando. En este estado el comando pide los datos del trabajador y pasa al
            /// siguiente estado.
            /// </summary>
            Start,
            /// <summary>
            /// El estado del nombre del trabajador. En este estado el comando pide los datos del trabajador y pasa al
            /// siguiente estado.
            /// </summary>
            NombreTrabajador,
            /// <summary>
            /// El estado del email del trabajador. En este estado el comando pide los datos del trabajador y pasa al
            /// siguiente estado.
            /// </summary>
            EmailTrabajador,
            /// <summary>
            /// El estado del telefono del trabajador. En este estado el comando pide los datos del trabajador y pasa al
            /// siguiente estado.
            /// </summary>
            TelefonoTrabajador,
            /// <summary>
            /// El estado del id del trabajador. En este estado el comando pide los datos del trabajador y pasa al
            /// siguiente estado.
            /// </summary>
            IdTrabajador,
            /// <summary>
            /// El estado de la ubicación del trabajador. En este estado el comando pide los datos del trabajador.
            /// </summary>
            UbicacionTrabajador
        }
        /// <summary>
        /// Representa los datos que va obteniendo el comando AgregarTrabajador en los diferentes estados.
        /// </summary>
        public class TrabajadorData
        {
            /// <summary>
            /// Representa el nombre del trabajador obtenido en AgregarEmpleador.
            /// </summary>
            public string NombreTrabajador { get; set; }
            /// <summary>
            /// Representa el email del trabajador obtenido en AgregarEmpleador.
            /// </summary>
            public string EmailTrabajador { get; set; }
            /// <summary>
            /// Representa el teléfono del trabajador obtenido en AgregarEmpleador.
            /// </summary>
            public string TelefonoTrabajador { get; set; }
            /// <summary>
            /// Representa la ubicación del trabajador obtenido en AgregarEmpleador.
            /// </summary>
            public string UbicacionTrabajador { get; set; }
            /// <summary>
            /// Representa el id del trabajador obtenido en AgregarEmpleador.
            /// </summary>
            public long IdTrabajador { get; set; }

        }
    }
}
