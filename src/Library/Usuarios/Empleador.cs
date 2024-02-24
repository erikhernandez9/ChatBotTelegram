using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Proyecto_Chatbot.Catalogos;

namespace Proyecto_Chatbot.Usuarios
{
    /// <summary>
    /// Esta clase representa al empleador.
    /// Utilizamos HERENCIA. Empleador es una clase hija de Usuario, hereda sus métodos.
    /// Como esta clase hereda de otra, podemos afirmar que se aplicó el principio OCP.
    /// Prepara nuestro código para que los posibles cambios en el comportamiento de una
    /// clase se puedan implementar mediante herencia y composición.
    /// </summary>
    public class Empleador : Usuario
    {
        /// <summary>
        /// Este get llamada al metodo para calcular la reputacion del usuario.
        /// </summary>
        /// <value>retorna la reputación.</value>
        public new double Reputacion
        {
            get
            {
                return CalcularReputacion.Calcular(this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Empleador"/> class.
        /// </summary>
        /// <param name="nombre">Es el nombre que se le asigna al Empleador.</param>
        /// <param name="email">Es el mail que se le asigna al Empleador.</param>
        /// <param name="telefono">Es el telefono que se le asigna al Empleador.</param>
        /// <param name="id">Es el id que se le asigna al Empleador.</param>
        /// <param name="ubicacion">Es la ubicación que se le asigna al Empleador.</param>
        public Empleador(string nombre, string email, string telefono, long id, string ubicacion) 
            : base(nombre, email, telefono, id, ubicacion)
        {
        }
    }
}
