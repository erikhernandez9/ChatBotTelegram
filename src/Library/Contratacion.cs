using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Proyecto_Chatbot.Usuarios;

namespace Proyecto_Chatbot
{
    /// <summary>
    /// Clase que representa la contratacion de un servicio.
    /// Se utilizó el principio SRP, ya que la clase contiene solo a la categoría,
    /// y tiene la responsabilidad de corroborar si la misma es válida cuando se crea.
    /// </summary>
    public class Contratacion : IJsonConvertible
    {
        /// <summary>
        /// Identificador de la contratacion.
        /// </summary>
        public int Id {get; set;}
        /// <summary>
        /// Representa la oferta laboral del contrato.
        /// </summary>
        public OfertaLaboral OfertaLaboral {get; set;}
        /// <summary>
        /// Representa el empleador que realiza la contratacion.
        /// </summary>
        public Empleador Empleador {get; set;}
        /// <summary>
        /// Representa el costo del trabajo a realizar.
        /// </summary>
        public int CostoServicio {get; set;}
        /// <summary>
        /// Representa la descripcion del contrato.
        /// </summary>
        public string Descripcion {get; set;}
        /// <summary>
        /// Representa la fecha en la que se crea el contrato.
        /// </summary>
        public string Fecha_inicio {get; set;}
        /// <summary>
        /// Representa la fecha en la que se cierra el contrato.
        /// </summary>
        public string Fecha_final {get; set;}
        /// <summary>
        /// Representa el estado de la contratacion
        /// True = Activo, False = inactivo.
        /// </summary>
        public bool Estado {get; set;}
        /// <summary>
        /// Almacena la calificacion del empleador.
        /// </summary>
        public Calificacion CalificacionEmpleador {get; set;}
        /// <summary>
        /// Almacena la califiacion del trabajador por el trabajo realizado.
        /// </summary>
        public Calificacion CalificacionTrabajador {get; set;}

        /// <summary>
        /// Initializes a new instance of the <see cref="Contratacion"/> class.
        /// </summary>
        /// <param name="id">id que se le asigna a la oferta laboral.</param>
        /// <param name="ofertaLaboral">Almacena la oferta laboral que se le asigna a la contratacion.</param>
        /// <param name="empleador">Almacena el empleador que contrata el servicio que se le asigna a la contratacion.</param>
        /// <param name="descripcion">Almacena la descripcion del contrato que se le asigna a la contratacion.</param>
        [JsonConstructor]
        public Contratacion(int id, OfertaLaboral ofertaLaboral, Empleador empleador, string descripcion)
        {
            if (ofertaLaboral != null && empleador != null)
            {
                this.Id = id;
                this.OfertaLaboral = ofertaLaboral;
                this.Empleador = empleador;
                this.CostoServicio = ofertaLaboral.Precio;
                this.Descripcion = descripcion;
                this.Estado = true; 
                this.Fecha_inicio = DateTime.UtcNow.ToString("dd-MM-yy");
            }
            else
            {
                Console.WriteLine("Valores incorrectos.");
            }
        }
        /// <summary>
        /// Métodos para calificar empleadores y trabajadores. Método desemplear.
        /// </summary>
        /// <param name="descripcion">Descripción de la califiación.</param>
        /// <param name="puntaje">puntaje de la calificación.</param>
        public void CalificarEmpleador(string descripcion, double puntaje)
        {
            this.CalificacionEmpleador = new Calificacion(descripcion, puntaje);
        }
        /// <summary>
        /// Metodo para calificar al trabajador.
        /// </summary>
        /// <param name="descripcion">almacena la descripcion de la calificacion.</param>
        /// <param name="puntaje">es el puntaje de la calificacion.</param>
        public void CalificarTrabajador(string descripcion, double puntaje)
        {
            this.CalificacionTrabajador = new Calificacion(descripcion, puntaje);
        }
        /// <summary>
        /// Método para agregar una calificación neutral al empleador si el trabajador demora más de un mes 
        /// en calificarlo. Este método forma parte de las USER STORIES.
        /// "Como empleador, quiero poder calificar a un trabajador; el trabajador me tiene que calificar a mi también, 
        /// si no me califica en un mes, la calificación será neutral, para que de esa forma, pueda definir la reputaión del trabajador".
        /// </summary>
        public void CalificarNeutral()
        {
            string hoy = DateTime.UtcNow.ToString("dd-MM-yy");
            DateTime ho = Convert.ToDateTime(DateTime.UtcNow.ToString("dd-MM-yy"));
            DateTime si = Convert.ToDateTime(this.Fecha_inicio);
            int difFechas = (ho - si).Days;
            if (difFechas > 31)
            {
                if (this.CalificacionEmpleador == null)
                {
                    this.CalificarEmpleador("Calificacion neutral", 2.5);
                }
                if (this.CalificacionTrabajador == null)
                {
                    this.CalificarTrabajador("Calificacion neutral", 2.5);
                }
            }
        }
        /// <summary>
        /// Metodo para desemplear a un trabajador, este pone la fecha cuando se cierra el contrato.
        /// </summary>
        public void Desemplear()
        {
            this.Estado = false;
            this.Fecha_final = DateTime.UtcNow.ToString("dd-MM-yy");
        }
        /// <summary>
        /// Convierte a json los datos de la clase.
        /// </summary>
        /// <returns>devuelve los datos convertidos.</returns>
        public string ConvertToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        /// <summary>
        /// Carga los datos json a la clase.
        /// </summary>
        /// <param name="json">json a deserializar.</param>
        public void LoadFromJson(string json)
        {
            Contratacion deserialized = JsonSerializer.Deserialize<Contratacion>(json);
            this.Id = deserialized.Id;
            this.OfertaLaboral = deserialized.OfertaLaboral;
            this.Empleador = deserialized.Empleador;
            this.CostoServicio = deserialized.CostoServicio;
            this.Descripcion = deserialized.Descripcion;
            this.Estado = deserialized.Estado;
            this.Fecha_inicio = deserialized.Fecha_inicio;
            this.Fecha_final = deserialized.Fecha_final;
        }
    }
}