using System;
using System.Collections.Generic;
using Proyecto_Chatbot;
using System.Text.Json;
using System.Text.Json.Serialization;
using Proyecto_Chatbot.Catalogos;

namespace Proyecto_Chatbot.Usuarios
{
    /// <summary>
    /// Clase que representa un trabajador
    /// Utilizamos HERENCIA. Trabajador es una clase hija de Usuario, hereda sus métodos.
    /// Como esta clase hereda de otra, podemos afirmar que se aplicó el principio OCP.
    /// Prepara nuestro código para que los posibles cambios en el comportamiento de una
    /// clase se puedan implementar mediante herencia y composición.
    /// </summary>
    public class Trabajador : Usuario
    {

    /// <summary>
    /// Lista que almacena las notificaciones del trabajador.
    /// </summary>      
    public List<string> Notificaciones = new List<string>();

        /// <summary>
        /// Este get llamada al metodo para calcular la reputacion del usuario.
        /// </summary>
        /// <value>retorna la reputacion.</value>
        public new double Reputacion
        {
            get
            {
                return CalcularReputacion.Calcular(this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trabajador"/> class.
        /// </summary>
        /// <param name="nombre">Es el nombre que se le asigna al trabajador.</param>
        /// <param name="email">Es el mail que se le asigna al trabajador.</param>
        /// <param name="telefono">Es el telefono que se le asigna al trabajador.</param>
        /// <param name="id">Es el id que se le asigna al trabajador.</param>
        /// <param name="ubicacion">Almacena la latitud que se le asigna al trabajador.</param>
        public Trabajador(string nombre, string email, string telefono, long id, string ubicacion) 
            : base(nombre, email, telefono, id, ubicacion)
        {
        }
    }
}