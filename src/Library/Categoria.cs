using System;
using System.IO;
namespace Proyecto_Chatbot
{
    /// <summary>
    /// Esta clase representa al categoria de cada servicio.
    /// Se utilizó el principio SRP, ya que la clase contiene solo a la categoría,
    /// y tiene la responsabilidad de corroborar si la misma es válida cuando se crea.
    /// Además, es la experta en información, ya que solo ella tiene y conoce la información
    /// necesaria para crear una categoría.
    /// </summary>
    public class Categoria
    {
        /// <summary>
        /// Representa el nombre del servicio.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Representa la descripcion de la categoria.
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Categoria"/> class.
        /// </summary>
        /// <param name="nombre">Almacena el nombre que se le asigna a la categoria.</param>
        /// <param name="descripcion">Almacena la descripcion que se le asigna a la categoria.</param>
        public Categoria(string nombre, string descripcion)
        {
            if (nombre != null && descripcion != null && nombre != string.Empty)
            {
                this.Descripcion = descripcion;
                this.Nombre = nombre;
            }
            else
            {
                throw new ExcepcionConstructor("Valores incorrectos");
            }
        }
    }
}