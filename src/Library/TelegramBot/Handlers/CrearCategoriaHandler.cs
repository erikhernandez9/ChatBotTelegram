using System.ComponentModel;
using System;
using Proyecto_Chatbot.Usuarios;
using Telegram.Bot.Types;
using Proyecto_Chatbot.Catalogos;
using System.Collections.Generic;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Handler del patrón Chain of Responsability, procesa el mensaje "Crear categoría" y
    /// crea una categoría, preguntando por su nombre y descripción. Quien puede realizar esta
    /// operación es el administrador, si estamos logueados como empleador o trabajador no tendremos
    /// acceso a este comando.
    /// <remarks>
    /// Este handler pertenece a las USER STORIES.
    /// "Cómo administrador, quiero poder indicar categorías sobre las cuales se realizarán las ofertas 
    /// de servicios para que de esa forma, los trabajadoras puedan clasificarlos."
    /// Esto porque el administrador crea las categorías para que se puedan ingresar las ofertas de servicios.
    /// </remarks>
    /// </summary>
    public class CrearCategoriaHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando.
        /// </summary>
        /// <value></value>
        public CategoriaState State { get; private set; }

        /// <summary>
        /// Los datos que va obteniendo el comando en los diferentes estados.
        /// </summary>
        public CategoriaData Data { get; private set; } = new CategoriaData();

        /// <summary>
        /// Inicializa una instancia de la clase <see cref="CrearCategoriaHandler"/>.
        /// </summary>
        /// <param name="next">siguiente handler.</param>
        public CrearCategoriaHandler(BaseHandler next)
            : base(new string[] { "Crear categoria" }, next)
        {
        }

        /// <inheritdoc/>
        protected override bool CanHandle(Mensaje mensaje)
        {
            if (!DictVerificacionPasos.ContainsKey(mensaje.Id))
            {
                List<Object> estado = new List<Object>();
                estado.Add(CategoriaState.Start.ToString());
                estado.Add("CrearCategoriaHandler");
                DictVerificacionPasos[mensaje.Id] = estado;
            }
            
            if (DictVerificacionPasos[mensaje.Id][0].ToString() == CategoriaState.Start.ToString())
            {
                return base.CanHandle(mensaje);
            }

            if (DictVerificacionPasos[mensaje.Id][1].ToString() == "CrearCategoriaHandler")
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
            DictVerificacionPasos[mensaje.Id][1] = "CrearCategoriaHandler";
            Administrador administrador = Singleton<ListaAdministradores>.Instance.SeleccionarAdministrador(mensaje.Id);
            if (administrador != null)
            {
                if (DictVerificacionPasos[mensaje.Id][0].ToString() == CategoriaState.Start.ToString())
                {
                    DictVerificacionPasos[mensaje.Id][0] = CategoriaState.NombreCategoria.ToString();
                    response = "Ingrese el nombre de la categoría";
                }
                else if (DictVerificacionPasos[mensaje.Id][0].ToString() == CategoriaState.NombreCategoria.ToString())
                {
                    this.Data.NombreCategoria = mensaje.Text;
                    DictVerificacionPasos[mensaje.Id][0] = CategoriaState.DescripcionCategoria.ToString();
                    response = "Ingrese la descripcion de la categoría";
                }
                else if (DictVerificacionPasos[mensaje.Id][0].ToString() == CategoriaState.DescripcionCategoria.ToString())
                {
                    this.Data.DescripcionCategoria = mensaje.Text;
                    DictVerificacionPasos[mensaje.Id][0] = CategoriaState.Start.ToString();
                    Singleton<CatalogoCategorias>.Instance.AgregarCategoria(this.Data.NombreCategoria, this.Data.DescripcionCategoria);
                    response = "La categoria fue creada";
                }
                else
                {
                    response = string.Empty;
                }
            }
            else
            {
                if (DictVerificacionPasos[mensaje.Id][0].ToString() == CategoriaState.Start.ToString())
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
            this.State = CategoriaState.Start;
            this.Data = new CategoriaData();
        }

        /// <summary>
        /// Indica los diferentes estados que puede tener el comando.
        /// </summary>
        public enum CategoriaState
        {
            /// <summary>
            /// El estado inicial del comando. En este estado el comando pide los datos iniciales y pasa al
            /// siguiente estado.
            /// </summary>
            Start,
            /// <summary>
            /// El estado inicial del comando. En este estado el comando pide los datos del nombre de la categoría y pasa al
            /// siguiente estado.
            /// </summary>
            NombreCategoria,
            /// <summary>
            /// El estado inicial del comando. En este estado el comando pide los datos de la descripción de la categoría
            /// </summary>
            DescripcionCategoria
        }

        /// <summary>
        /// Constructor de la clase CategoriaData.
        /// Representa los datos que va obteniendo el comando CrearCategoria en los diferentes estados.
        /// </summary>
        public class CategoriaData
        {
            /// <summary>
            /// Representa y guarda el nombre de la categoría.
            /// </summary>
            public string NombreCategoria { get; set; }

            /// <summary>
            /// Representa y guarda la descripción de la categoría.
            /// </summary>
            public string DescripcionCategoria { get; set; }

        }
    }
}