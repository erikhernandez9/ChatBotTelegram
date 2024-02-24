using System;
using Proyecto_Chatbot.Usuarios;
namespace Proyecto_Chatbot;

/// <summary>
/// Clase que se encarga de crear la notificación a enviar a un trabajador cuando se da de baja una oferta que ha creado.
/// </summary>
public class Notificaciones
{
    /// <summary>
    /// Método que crea el string de notificación para el trabajador.
    /// </summary>
    /// <param name="oferta">Representa la oferta eliminada.</param>
    /// <returns>Retorna un string de la notificación del trabajador.</returns>
    public static void NotificacionTrabajador(OfertaLaboral oferta)
    {
         Singleton<ListaTrabajadores>.Instance.NotificarTrabajador(oferta.Trabajador, $"Un administrador ha eliminado una oferta que te pertenece: {oferta.Categoria}, {oferta.Descripcion}, {oferta.Precio}");
    }
}