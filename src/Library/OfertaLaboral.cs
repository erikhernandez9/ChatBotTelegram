using System;
using Proyecto_Chatbot.Usuarios;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Proyecto_Chatbot
{
    /// <summary>
    /// Representa la oferta laboral que publica un trabajador.
    /// Se utilizó el principio SRP, ya que la clase contiene solo a la oferta,
    /// y tiene la responsabilidad de corroborar si la misma es válida cuando se crea.
    /// </summary>
    public class OfertaLaboral
    {
        /// <summary>
        /// Representa el identificador de la oferta laboral.
        /// </summary>
        public int Id {get; set;}
        /// <summary>
        /// Almacena la categoria de la oferta.
        /// </summary>
        public Categoria Categoria {get; set;}
        /// <summary>
        /// Almacena el trabajador ofertante.
        /// </summary>
        public Trabajador Trabajador {get; set;}
        /// <summary>
        /// Almacena la descripcion de la oferta.
        /// </summary>
        public string Descripcion {get; set;}
        /// <summary>
        /// Representa el precio del trabajo que haria el trabajador.
        /// </summary>
        public int Precio {get; set;}


        /// <summary>
        /// Initializes a new instance of the <see cref="OfertaLaboral"/> class.
        /// </summary>
        /// <param name="id">Almacena la id de usuario para identificarlo.</param>
        /// <param name="categoria">Almacena La categoria que se le asigna a la oferta.</param>
        /// <param name="trabajador">Almacena el trabajador que crea la oferta.</param>
        /// <param name="descripcion">Almacena la descripcion que se le asigna a la oferta.</param>
        /// <param name="precio">Almacena el precio que se le asigna a la oferta.</param>
        public OfertaLaboral(int id, Categoria categoria, Trabajador trabajador, string descripcion, int precio)

        {
            if (categoria != null && trabajador != null)
            {
                this.Id = id;
                this.Categoria = categoria;
                this.Trabajador = trabajador;
                this.Descripcion = descripcion;
                this.Precio = precio;
            }
            else
            {
                throw new ExcepcionConstructor("Parametros incorrectos");
            }
        }
    }
}