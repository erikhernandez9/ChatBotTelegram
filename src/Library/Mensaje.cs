using System;
namespace Proyecto_Chatbot;

    /// <summary>
    /// Esta clase representa al mensaje que envía el usuario.
    /// Creada para disminuir el acoplamiento y aumentar la coesión. (Patrones Low coupling y high cohesion)
    /// </summary>
public class Mensaje
{
    /// <summary>
    /// Representa el id del usuario que envía el mensaje.
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Representa el mensaje que envía el usuario.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Mensaje"/> class.
    /// </summary>
    /// <param name="id">Almacena el id del usuario.</param>
    /// <param name="text">Almacena el mensaje enviado.</param>
    public Mensaje(long id, string text)
    {
        this.Id = id;
        this.Text = text;
    }
}