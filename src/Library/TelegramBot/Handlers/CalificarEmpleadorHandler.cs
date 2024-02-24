using System.Text;
using System.ComponentModel;
using System;
using Proyecto_Chatbot.Usuarios;
using Telegram.Bot.Types;
using Proyecto_Chatbot.Catalogos;
using System.Collections.Generic;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Handler del patrón Chain of Responsability, procesa el mensaje "Calificar empleador". Sirve para que
    /// el trabajador califique a un empleador (al empleador perteneciente a la contratación indicada).
    /// Este handler, además, nos avisa si realizamos una calificación en el rango incorrecto (de 0 a 5)
    /// y también nos avisa si no tenemos permisos ya que esta acción solo la hace el trabajador.
    /// <remarks>
    /// Este handler pertenece a las USER STORIES.
    /// "Como trabajador, quiero poder calificar a un empleador; el empleador me tiene que calificar a mi también, 
    /// si no me califica en un mes, la calificación será neutral, para que de esa forma pueda definir la reputación de mi empleador."
    /// </remarks>
    /// </summary>
    public class CalificarEmpleadorHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando.
        /// </summary>
        public CalificarState State { get; private set; }

        /// <summary>
        /// Los datos que va obteniendo el comando en los diferentes estados.
        /// </summary>
        public CalificarData Data { get; private set; } = new CalificarData();

        /// <summary>
        /// Inicializa una instancia de la clase <see cref="CalificarEmpleadorHandler"/>.
        /// </summary>
        /// <param name="next">siguiente handler.</param>
        public CalificarEmpleadorHandler(BaseHandler next)
            : base(new string[] { "Calificar empleador" }, next)
        {
        }

        /// <inheritdoc/>
        protected override bool CanHandle(Mensaje mensaje)
        {
            if (!DictVerificacionPasos.ContainsKey(mensaje.Id))
            {
                List<Object> estado = new List<Object>();
                estado.Add(CalificarState.Start.ToString());
                estado.Add("CalificarEmpleadorHandler");
                DictVerificacionPasos[mensaje.Id] = estado;
            }
            
            if (DictVerificacionPasos[mensaje.Id][0].ToString() == CalificarState.Start.ToString())
            {
                return base.CanHandle(mensaje);
            }

            if (DictVerificacionPasos[mensaje.Id][1].ToString() == "CalificarEmpleadorHandler")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Procesa todos los mensajes y retorna true siempre.
        /// </summary>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado indicando que el mensaje no pudo se procesado.</param>
        protected override void InternalHandle(Mensaje mensaje, out string response)
        {
            DictVerificacionPasos[mensaje.Id][1] = "CalificarEmpleadorHandler";
            Trabajador trabajador = Singleton<ListaTrabajadores>.Instance.SeleccionarTrabajador(mensaje.Id);
            if (trabajador != null)
            {
                if (DictVerificacionPasos[mensaje.Id][0].ToString() == CalificarState.Start.ToString())
                {
                    DictVerificacionPasos[mensaje.Id][0] = CalificarState.Contratacion.ToString();
                    StringBuilder listado = new StringBuilder();
                    listado.Append("Seleccione la contratacion a elegir (Solamente ingrese el numero)");
                    int cont = 0;
                    List<Contratacion> lista = Singleton<CatalogoContratacion>.Instance.FiltrarContratacionesPorTrabajador(mensaje.Id);
                    if (lista.Count != 0)
                    {
                        foreach (Contratacion contratacion in Singleton<CatalogoContratacion>.Instance.FiltrarContratacionesPorTrabajador(mensaje.Id))
                        {
                            cont++;
                            listado.Append($"\n{cont}. empleador asociado: {contratacion.Empleador.Nombre}, descripción: {contratacion.Descripcion}");
                        }

                        response = listado.ToString();
                    }
                    else
                    {
                        response = "No tenes ninguna contratacion";
                        DictVerificacionPasos[mensaje.Id][0] = CalificarState.Start.ToString();
                    }
                }
                else if (DictVerificacionPasos[mensaje.Id][0].ToString() == CalificarState.Contratacion.ToString())
                {
                    this.Data.Contratacion = mensaje.Text;
                    DictVerificacionPasos[mensaje.Id][0] = CalificarState.Puntaje.ToString();
                    response = "Ingrese el puntaje (del 0 al 5)";
                }
                else if (DictVerificacionPasos[mensaje.Id][0].ToString() == CalificarState.Puntaje.ToString())
                {
                    this.Data.Puntaje = mensaje.Text;
                    DictVerificacionPasos[mensaje.Id][0] = CalificarState.Descripcion.ToString();
                    response = "Escribi la descripcion";
                }
                else if (DictVerificacionPasos[mensaje.Id][0].ToString() == CalificarState.Descripcion.ToString())
                {
                    this.Data.Descripcion = mensaje.Text;
                    DictVerificacionPasos[mensaje.Id][0] = CalificarState.Start.ToString();
                    try
                    {
                        Singleton<CatalogoContratacion>.Instance.FiltrarContratacionesPorTrabajador(mensaje.Id)[Int32.Parse(this.Data.Contratacion) - 1].CalificarEmpleador(this.Data.Descripcion,Int32.Parse(this.Data.Puntaje));
                        Singleton<CatalogoContratacion>.Instance.ConvertToJson();
                        response = $"Muy bien, Calificaste a {Singleton<CatalogoContratacion>.Instance.FiltrarContratacionesPorTrabajador(mensaje.Id)[Int32.Parse(this.Data.Contratacion) - 1].Empleador.Nombre}";
                    }
                    catch (ExcepcionConstructor)
                    {
                        response = "Valores incorrectos. Ingrese 'Calificar empleador' para calificar nuevamente";
                    }
                }
                else
                {
                    response = string.Empty;
                }
            }
            else
            {
                if (DictVerificacionPasos[mensaje.Id][0].ToString() == CalificarState.Start.ToString())
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
        /// Método para cambiar estados dentro de la clase a la hora de tener que modificarlos
        /// entre mensajes.
        /// </summary>
        protected override void InternalCancel()
        {
            this.State = CalificarState.Start;
            this.Data = new CalificarData();
        }

        /// <summary>
        /// Indica los diferentes estados que puede tener el comando Calificar.
        /// </summary>
        public enum CalificarState
        {
            /// <summary>
            /// El estado inicial del comando. En este estado el comando pide los datos iniciales del empleador y pasa al
            /// siguiente estado.
            /// </summary>
            Start,
            /// <summary>
            /// El estado inicial que indica a qué contratación refiere la calificación.
            /// </summary>
            Contratacion,
            /// <summary>
            /// El estado donde se indica que puntaje será el de la calificación del trabajador.
            /// </summary>
            Puntaje,
            /// <summary>
            /// El estado donde se indica la descipción general de la calificiación.
            /// </summary>
            Descripcion
        }

        /// <summary>
        /// Representa los datos que va obteniendo el comando CalificarTrabajador en los diferentes estados.
        /// </summary>
        public class CalificarData
        {
            /// <summary>
            /// Representa y guarda el puntaje proporcionado por el empleador.
            /// </summary>
            public string Puntaje { get; set; }
            /// <summary>
            /// Representa a qué contratación refiere la calificación.
            /// </summary>
            public string Contratacion { get; set; }
            /// <summary>
            /// Representa la descripción de la calificación proporcionada por el empleador.
            /// </summary>
            public string Descripcion { get; set; }
        }
    }
}