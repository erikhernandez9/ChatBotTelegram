using System.Threading;
using System;
namespace Proyecto_Chatbot;
/// <summary>
/// Representa a todos los usuarios que utilizan la aplicacion (incluyendo al administrador).
/// Es la clase padre de Usuario, por lo que se usó HERENCIA. El principio OCP es utilizado
/// ya que esta clase se implementa mediante herencia en Usuario.
/// Además se utilizaron los patrones y principios de DIP e ISP, ya que esta clase implementa
/// la interfaz de IUser, esto para darle robustez al código.
/// </summary>
public abstract class Persona : IUser
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Persona"/> class.
    /// </summary>
    /// <param name="nombre">Almacena el nombre que se le asigna al usuario.</param>
    /// <param name="email">Almacena el mail que se le asigna al usuario.</param>
    /// <param name="id">Almacena la cedula que se le asigna al usuario.</param>
    public Persona(string nombre, string email, long id)
    {
        if (nombre != null && email != null && nombre != string.Empty && email != string.Empty)
        {
            this.Nombre = nombre;
            this.Id = id;
            this.Email = email;
        }
        else
        {
            throw new ExcepcionConstructor("Parametros incorrectos");
        }
    }
    /// <summary>
    /// Representa el nombre de la persona.
    /// </summary>
    public string Nombre {get; set;}
    /// <summary>
    /// Representa el mail de la persona.
    /// </summary>
    public string Email {get; set;}
    /// <summary>
    /// Representa la cedula de la persona.
    /// </summary>
    public long Id {get; set;}
}