using System.Runtime.InteropServices.ComTypes;
using Telegram.Bot.Types;
using Proyecto_Chatbot.Catalogos;
using Proyecto_Chatbot.Usuarios;
using System;
using System.Collections.Generic;

//Desabilitación de Warnings chequeados
#pragma warning disable SA1623 // PropertySummaryDocumentationMustMatchAccessors

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Handler de la cadena de responsabilidades que implementa el comando "Eliminar categoría".
    /// Este handler forma parte de las USER STORIES, ya que es una acción que solo puede realizar
    /// el administrador. Pensamos que, como en la primera historia de usuario indica que el admin
    /// puede crear categorías, el mismo también puede eliminarlas.
    /// "Cómo administrador, quiero poder indicar categorías sobre las cuales se realizarán las ofertas
    /// de servicios para que de esa forma, los trabajadoras puedan clasificarlos".
    /// </summary>
    public class EliminarCategoriaHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando.
        /// </summary>
        public CategoriaState State { get; private set; }

        /// <summary>
        /// Los datos que va obteniendo el comando en los diferentes estados.
        /// </summary>
        public CategoriaData Data { get; private set; } = new CategoriaData();

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="EliminarCategoriaHandler"/>.
        /// </summary>
        /// <param name="next">siguiente handler.</param>
        public EliminarCategoriaHandler(BaseHandler next)
            : base(new string[] { "Eliminar una categoria" }, next)
        {
        }

        /// <inheritdoc/>
        protected override bool CanHandle(Mensaje mensaje)
        {
            if (!DictVerificacionPasos.ContainsKey(mensaje.Id))
            {
                List<Object> estado = new List<Object>();
                estado.Add(CategoriaState.Start.ToString());
                estado.Add("EliminarCategoriaHandler");
                DictVerificacionPasos[mensaje.Id] = estado;
            }

            if (DictVerificacionPasos[mensaje.Id][0].ToString() == CategoriaState.Start.ToString())
            {
                return base.CanHandle(mensaje);
            }

            if (DictVerificacionPasos[mensaje.Id][1].ToString() == "EliminarCategoriaHandler")
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
            DictVerificacionPasos[mensaje.Id][1] = "EliminarCategoriaHandler";
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
                    DictVerificacionPasos[mensaje.Id][0] = CategoriaState.Start.ToString();
                    Singleton<CatalogoCategorias>.Instance.EliminarCategoria(this.Data.NombreCategoria);
                    Singleton<CatalogoCategorias>.Instance.ConvertToJson();
                    response = "La categoria fue eliminada";
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
            this.State = CategoriaState.Start;
            this.Data = new CategoriaData();
        }

        /// <summary>
        /// Indica los diferentes estados que puede tener el comando.
        /// </summary>
        public enum CategoriaState
        {
            /// <summary>
            /// El estado inicial del comando. En este estado el comando pide los datos iniciales de la categoría y pasa al
            /// siguiente estado.
            /// </summary>
            Start,
            /// <summary>
            /// El estado del nombre de la categoría.
            /// </summary>
            NombreCategoria
        }

        /// <summary>
        /// Constructor de la clase CategoriaData. Contiene getters y setters.
        /// </summary>
        public class CategoriaData
        {
            /// <summary>
            /// Representa el nombre de la categoría a eliminar.
            /// </summary>
            public string NombreCategoria { get; set; }
        }
    }
}