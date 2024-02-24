using System;
namespace Proyecto_Chatbot
{
    /// <summary>
    /// Interfaz que representa el tipo User.
    /// Se utilizó el patrón ISP y DIP, para conseguir robustez y flexibilidad, y posibilitar la
    /// reutilización, con estas interfaces. IUser es implementada por Persona, facilita la creación
    /// de todos los usuarios (admin, trabajador, empleador)
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Representa el nombre del usuario
        /// </summary>
        string Nombre {get; set;}
        /// <summary>
        /// Representa el mail del usuario
        /// </summary>
        string Email {get; set;}
    }
}