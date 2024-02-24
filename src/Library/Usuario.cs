using System.Threading;
using System;
using Proyecto_Chatbot.Catalogos;
namespace Proyecto_Chatbot;

/// <summary>
/// Clase que representa a los usuarios de la aplicacion (Empleadores y Trabajadores).
/// Es una clase hija de Persona, y a la vez clase padre de los usuarios (trabajador, empleador).
/// El principio OCP es utilizado ya que esta clase se implementa mediante herencia en las clases
/// de Trabajador y Empleador, y está implementando la clase Persona.
/// </summary>
public abstract class Usuario : Persona
{
    /// <summary>
    /// Almacena la reputacion del usuario.
    /// </summary>
    public double Reputacion { get; protected set; }

    /// <summary>
    /// Guarda la direccion de la persona.
    /// </summary>
    public string Ubicacion { get; set; }

    /// <summary>
    /// Almacena el telefono del usuario.
    /// </summary>
    public string Telefono { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Usuario"/> class.
    /// </summary>
    /// <param name="nombre">Es el nombre que se le asigna al Usuario.</param>
    /// <param name="email">Es el mail que se le asigna al Usuario.</param>
    /// <param name="telefono">Es el telefono que se le asigna al Usuario.</param>
    /// <param name="id">Es la cedula que se le asigna al Usuario.</param>
    /// <param name="ubicacion">Almacena la direccion que se le asigna al Usuario.</param>
    public Usuario(string nombre, string email, string telefono, long id, string ubicacion) 
        : base(nombre, email, id)
    {
        if (telefono != null && telefono != string.Empty && ubicacion != null && ubicacion != string.Empty && VerificarString.TieneSoloNumeros(telefono))
        {
            this.Nombre = nombre;
            this.Email = email;
            this.Id = id;
            this.Telefono = telefono;
            this.Ubicacion = ubicacion;
        }
        else
        {
            throw new ExcepcionConstructor("Parametros incorrectos");
        }
    }
}