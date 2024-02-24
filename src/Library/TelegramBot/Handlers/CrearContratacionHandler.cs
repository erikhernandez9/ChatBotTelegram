using Telegram.Bot.Types;
using Proyecto_Chatbot.Usuarios;
using Proyecto_Chatbot.Catalogos;
using System;
using System.Collections.Generic;

//Desabilitación de Warnings chequeados
#pragma warning disable CA1305

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Handler del patrón Chain of Responsability que crea una contratacion al ingresar el mensaje
    /// "Crear una contratacion". Crea una contratación pudiendo ordenar las ofertas para verlas como 
    ///  el usuario desee, por precio, ubicacion, calificación, etc.
    /// USER STORY:
    /// "Como empleador, quiero buscar ofertas de trabajo, opcionalmente filtrando por categoría para 
    ///  que de esa forma, pueda contratar un servicio"
    /// </summary>
    public class CrearContratacionHandler : BaseHandler
    {
        /// <summary>
        /// Estado del comando.
        /// </summary>
        /// <value></value>
        public ContratacionState State { get; private set; }

        /// <summary>
        /// El estado inicial del comando. En este estado el comando pide los datos del nombre de la categoría y pasa al
        /// siguiente estado. 
        /// </summary>
        public ContratacionData Data { get; private set; } = new ContratacionData();
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CrearContratacionHandler"/>.
        /// </summary>
        /// <param name="next">siguiente handler.</param>
        public CrearContratacionHandler(BaseHandler next)
            : base(new string[] { "Crear una contratacion" }, next)
        {
        }

        /// <inheritdoc/>
        protected override bool CanHandle(Mensaje mensaje)
        {
            if (!DictVerificacionPasos.ContainsKey(mensaje.Id))
            {
                List<Object> estado = new List<Object>();
                estado.Add(ContratacionState.Start.ToString());
                estado.Add("CrearContratacionHandler");
                DictVerificacionPasos[mensaje.Id] = estado;
            }

            if (DictVerificacionPasos[mensaje.Id][0].ToString() == ContratacionState.Start.ToString())
            {
                return base.CanHandle(mensaje);
            }

            if (DictVerificacionPasos[mensaje.Id][1].ToString() == "CrearContratacionHandler")
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
            DictVerificacionPasos[mensaje.Id][1] = "CrearContratacionHandler";
            Empleador empleador = Singleton<ListaEmpleadores>.Instance.SeleccionarEmpleador(mensaje.Id);
            if (empleador != null)
            {
            if (DictVerificacionPasos[mensaje.Id][0].ToString() == ContratacionState.Start.ToString())
            {
                DictVerificacionPasos[mensaje.Id][0] = ContratacionState.NombreCategoriaContratacion.ToString();
                Singleton<CatalogoCategorias>.Instance.LoadFromJson(Singleton<CatalogoCategorias>.Instance.ShowFile());
                response = $"Lista de categorías: {Singleton<CatalogoCategorias>.Instance.Imprimir()}\nEscriba la categoría asociada a su oferta";
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString() == ContratacionState.NombreCategoriaContratacion.ToString())
            {
                this.Data.NombreCategoriaContratacion = mensaje.Text;
                DictVerificacionPasos[mensaje.Id][0] = ContratacionState.NumeroContratacion.ToString();
                Singleton<CatalogoOfertas>.Instance.LoadFromJson(Singleton<CatalogoOfertas>.Instance.ShowFile());
                response = "Indique cómo quiere ordenar las ofertas:\n1. Ordenar por calificacion\n2. Ordenar por precio\n3. Ordenar por ubicación\n4. Ordenar por categoria\nPor favor, indique el número correspondiente de la opción deseada";
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString() == ContratacionState.NumeroContratacion.ToString())
            {
                this.Data.NumeroContratacion = mensaje.Text;
                this.Data.EmpleadorContratacion = empleador;
                DictVerificacionPasos[mensaje.Id][0] = ContratacionState.OfertaContratacion.ToString();
                switch (this.Data.NumeroContratacion)
                {
                    case "1":
                        response = $"Lista de ofertas odernadas por calificación: {Singleton<CatalogoOfertas>.Instance.ImprimirFiltradoCalificacion(this.Data.NombreCategoriaContratacion)}\nIndique el número de opción deseada";
                        break;
                    case "2":
                        response = $"Lista de ofertas odernadas por precio: {Singleton<CatalogoOfertas>.Instance.ImprimirFiltradoPrecio(this.Data.NombreCategoriaContratacion)}\nIndique el número de opción deseada";
                        break;
                    case "3":
                        response = $"Lista de ofertas odernadas por ubicación: {Singleton<CatalogoOfertas>.Instance.ImprimirFiltradoUbicacion(this.Data.NombreCategoriaContratacion, this.Data.EmpleadorContratacion.Id.ToString(System.Globalization.CultureInfo.CurrentCulture))}\nIndique el número de opción deseada";
                        break;
                    case "4":
                        response = $"Lista de ofertas odernadas por categoria: {Singleton<CatalogoOfertas>.Instance.ImprimirFiltradoCategorias(this.Data.NombreCategoriaContratacion)}\nIndique el número de opción deseada";
                        break;
                    default:
                        response = string.Empty;
                        break;
                }
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString() == ContratacionState.OfertaContratacion.ToString())
            {
                switch (this.Data.NumeroContratacion)
                {
                    case "1":
                        this.Data.OfertaContratacion = Singleton<CatalogoOfertas>.Instance.BuscarOfertasPorCalificacion(this.Data.NombreCategoriaContratacion)[Int32.Parse(mensaje.Text) - 1];
                        break;
                    case "2":
                        this.Data.OfertaContratacion = Singleton<CatalogoOfertas>.Instance.BuscarOfertasPorPrecio(this.Data.NombreCategoriaContratacion)[Int32.Parse(mensaje.Text) - 1];
                        break;
                    case "3":
                        this.Data.OfertaContratacion = Singleton<CatalogoOfertas>.Instance.BuscarOfertasPorCalificacion(this.Data.NombreCategoriaContratacion)[Int32.Parse(mensaje.Text) - 1];
                        break;
                    case "4":
                        this.Data.OfertaContratacion = Singleton<CatalogoOfertas>.Instance.BuscarOfertaPorCategoria(this.Data.NombreCategoriaContratacion)[Int32.Parse(mensaje.Text) - 1];
                        break;
                    default:
                        response = string.Empty;
                        break;
                }

                DictVerificacionPasos[mensaje.Id][0] = ContratacionState.DescripcionContratacion.ToString();
                response = "Ingrese la descripcion de la contratacion";
            }
            else if (DictVerificacionPasos[mensaje.Id][0].ToString() == ContratacionState.DescripcionContratacion.ToString())
            {
                this.Data.DescripcionContratacion = mensaje.Text;
                DictVerificacionPasos[mensaje.Id][0] = ContratacionState.Start.ToString();
                Singleton<CatalogoContratacion>.Instance.AgregarContratacion(this.Data.OfertaContratacion, this.Data.EmpleadorContratacion, this.Data.DescripcionContratacion);
                response = "La contratacion fue creada con éxito}";
            }
            else
            {
                response = string.Empty;
            }
            }
            else
            {
                if (DictVerificacionPasos[mensaje.Id][0].ToString() == ContratacionState.Start.ToString())
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
            this.State = ContratacionState.Start;
            this.Data = new ContratacionData();
        }

        /// <summary>
        /// Indica los diferentes estados que puede tener el comando.
        /// </summary>
        public enum ContratacionState
        {
            /// <summary>
            /// El estado inicial del comando. En este estado el comando pide los datos iniciales de la oferta y pasa al
            /// siguiente estado.
            /// </summary>
            Start,
            /// <summary>
            /// El estado de la oferta relacionada a la contratación. En este estado el comando pide los datos del nombre de la categoría
            /// y pasa al siguiente estado.
            /// </summary>
            OfertaContratacion,
            /// <summary>
            /// El estado del empleador que realiza la contratación. En este estado el comando pide los datos del nombre de la categoría y pasa al
            /// siguiente estado.
            /// </summary>
            EmpleadorContratacion,
            /// <summary>
            /// El estado del nombre de la categoría relacionada a la contratación. En este estado el comando pide los datos del nombre de la categoría y pasa al
            /// siguiente estado.
            /// </summary>
            NombreCategoriaContratacion,
            /// <summary>
            /// El estado del número que será el id de la contratación, valor numérico para identificarla.
            /// En este estado el comando pide los datos del nombre de la categoría y pasa al
            /// siguiente estado.
            /// </summary>
            NumeroContratacion,
            /// <summary>
            /// El estado de la descripción de la contratación. En este estado el comando pide los datos del nombre de la categoría y pasa al
            /// siguiente estado.
            /// </summary>
            DescripcionContratacion
        }

        /// <summary>
        /// Constructor de la clase ContratacionData. Contiene getters y setters
        /// que toman datos de categoria y trabajador, sumando los datos propios de la oferta.
        /// </summary>
        public class ContratacionData
        {
            /// <summary>
            /// Representa la oferta laboral relacionada a la contratación.
            /// </summary>
            public OfertaLaboral OfertaContratacion { get; set; }

            /// <summary>
            /// Representa la categoría relacionada a la oferta de la contratación.
            /// </summary>
            public string NombreCategoriaContratacion { get; set; }

            /// <summary>
            /// Representa el empleador que coloca la contratación.
            /// </summary>
            public Empleador EmpleadorContratacion { get; set; }

            /// <summary>
            /// Representa el número identificador de la contratación.
            /// </summary>
            public string NumeroContratacion { get; set; }

            /// <summary>
            /// Representa la descripción de la contratacin.
            /// </summary>
            public string DescripcionContratacion { get; set; }

        }
    }
}
