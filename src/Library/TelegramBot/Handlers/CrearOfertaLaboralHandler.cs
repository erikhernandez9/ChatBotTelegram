using Telegram.Bot.Types;
using Proyecto_Chatbot.Usuarios;
using Proyecto_Chatbot.Catalogos;
using System;
using System.Collections.Generic;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Handler del patrón Chain of Responsability, procesa el mensaje "Crear oferta laboral" y
    /// crea una oferta, preguntando por la categoría, el precio y una descripción.
    /// <remarks>
    /// Este handler pertenece a las USER STORIES.
    /// "Como trabajador, quiero poder hacer ofertas de servicios; mi oferta indicará en qué categoría
    /// quiero publicar, tendrá una descripción del servicio ofertado, y un precio para que de esa forma,
    /// mis ofertas sean ofrecidas a quienes quieren contratar servicios."
    /// </remarks>
    /// </summary>
    public class CrearOfertaLaboralHandler : BaseHandler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CrearOfertaLaboralHandler"/>.
        /// </summary>
        /// <param name="next">siguiente handler.</param>
        public CrearOfertaLaboralHandler(BaseHandler next)
            : base(new string[] { "Crear oferta laboral" }, next)
        {
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
            /// El estado del nombre de la categoría. Este estado es auxiliar para poder buscar la  existencia de la categoría.
            /// En este estado el comando pide los datos del nombre de la categoría y pasa al siguiente estado.
            /// </summary>
            NombreCategoria,
            /// <summary>
            /// El estado del nombre de la categoría. En este estado el comando pide los datos del nombre de la categoría y pasa al
            /// siguiente estado.
            /// </summary>
            CategoriaOferta,
            /// <summary>
            /// El estado del trabajador que pone la oferta. En este estado el comando pide el objeto del trabajador y pasa al
            /// siguiente estado.
            /// </summary>
            TrabajadorOferta,
            /// <summary>
            /// El estado del precio de la oferta. En este estado el comando pide el precio de la oferta y pasa al
            /// siguiente estado.
            /// </summary>
            PrecioOferta,
            /// <summary>
            /// El estado de la descripción de la oferta. En este estado el comando pide la descripción de la oferta y pasa al
            /// siguiente estado.
            /// </summary>
            DescripcionOferta,
        }
        /// <summary>
        /// Estado del comando.
        /// </summary>
        /// <value></value>
        public OfertaState State { get; private set; }
        /// <summary>
        /// Los datos que va obteniendo el comando en los diferentes estados.
        /// </summary>
        public OfertaData Data { get; private set; } = new OfertaData();
        /// <inheritdoc/>
        protected override bool CanHandle(Mensaje mensaje)
        {
            if (!DictVerificacionPasos.ContainsKey(mensaje.Id))
            {
                List<Object> estado = new List<Object>();
                estado.Add(OfertaState.Start.ToString());
                estado.Add("CrearOfertaLaboralHandler");
                DictVerificacionPasos[mensaje.Id] = estado;
            }

            if (DictVerificacionPasos[mensaje.Id][0].ToString() == OfertaState.Start.ToString())
            {
                return base.CanHandle(mensaje);
            }

            if (DictVerificacionPasos[mensaje.Id][1].ToString() == "CrearOfertaLaboralHandler")
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
            DictVerificacionPasos[mensaje.Id][1] = "CrearOfertaLaboralHandler";
            Trabajador trabajador = Singleton<ListaTrabajadores>.Instance.SeleccionarTrabajador(mensaje.Id);
            if (trabajador != null)
            {
            if (DictVerificacionPasos[mensaje.Id][0].ToString() == OfertaState.Start.ToString())
            {
                DictVerificacionPasos[mensaje.Id][0] = OfertaState.CategoriaOferta.ToString();
                Singleton<CatalogoCategorias>.Instance.LoadFromJson(Singleton<CatalogoCategorias>.Instance.ShowFile());
                response = $"Lista de categorías: {Singleton<CatalogoCategorias>.Instance.Imprimir()}\nEscriba la categoría asociada a su oferta";
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString() == OfertaState.CategoriaOferta.ToString())
            {
                this.Data.CategoriaOferta = null;
                this.Data.NombreCategoria = mensaje.Text;
                this.Data.CategoriaOferta = Singleton<CatalogoCategorias>.Instance.SeleccionarCategorias(this.Data.NombreCategoria);
                if (this.Data.CategoriaOferta == null)
                {
                    response = "La categoria no existe. Ingrese nuevamente";
                    DictVerificacionPasos[mensaje.Id][0] = OfertaState.Start.ToString();
                }
                else
                {
                    DictVerificacionPasos[mensaje.Id][0] = OfertaState.PrecioOferta.ToString();
                    response = "Ingrese precio de la oferta en pesos";
                }
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString() == OfertaState.PrecioOferta.ToString())
            {
                this.Data.PrecioOferta = Int32.Parse(mensaje.Text);
                DictVerificacionPasos[mensaje.Id][0] = OfertaState.DescripcionOferta.ToString();
                response = "Ingrese la descripción para la oferta";
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString() == OfertaState.DescripcionOferta.ToString())
            {
                this.Data.DescripcionOferta = mensaje.Text;
                DictVerificacionPasos[mensaje.Id][0] = OfertaState.Start.ToString();
                this.Data.TrabajadorOferta = trabajador;
                Singleton<CatalogoOfertas>.Instance.AgregarOferta(this.Data.CategoriaOferta, this.Data.TrabajadorOferta, this.Data.DescripcionOferta, this.Data.PrecioOferta);
                response = "La oferta fue creada con éxito";
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
        /// Constructor de la clase OfertaData. Contiene getters y setters
        /// que toman datos de categoria y trabajador, sumando los datos propios de la oferta.
        /// </summary>
        public class OfertaData
        {
            /// <summary>
            /// Representa y guarda el nombre de la categoría a la que pertenece la oferta.
            /// Variable auxiliar.
            /// </summary>
            public string NombreCategoria { get; set; }

            /// <summary>
            /// Representa y guarda el nombre de la categoría a la que pertenece la oferta.
            /// </summary>
            public Categoria CategoriaOferta { get; set; }

            /// <summary>
            /// Representa y guarda el objeto Trabajador relacionado al trabajador que creó la oferta.
            /// </summary>
            public Trabajador TrabajadorOferta { get; set; }

            /// <summary>
            /// Representa y guarda el precio de la oferta.
            /// </summary>
            public int PrecioOferta { get; set; }

            /// <summary>
            /// Representa y guarda la descripción de la oferta.
            /// </summary>
            public string DescripcionOferta { get; set; }

        }
    }
}
