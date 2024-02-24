using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Proyecto_Chatbot.Usuarios
{
    /// <summary>
    /// Representa el administrador de la aplicacion.
    /// Se utilizó HERENCIA para la creación de la clase, ya que Administrador
    /// es una clase hija de Persona. Por lo tanto podemos decir que utilizamos el
    /// principio OCP.
    /// </summary>
    public class Administrador : Persona
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Administrador"/> class.
        /// </summary>
        /// <param name="nombre">Es el nombre que se le asigna al administrador.</param>
        /// <param name="email">Es el mail que se le asigna al administrador.</param>
        /// <param name="id">Es el id que se le asigna al administrador.</param>
        public Administrador(string nombre, string email, long id) 
            : base(nombre, email, id)
        {
        }
    }
}