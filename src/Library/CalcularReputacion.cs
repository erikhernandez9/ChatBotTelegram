using System;
using Proyecto_Chatbot.Usuarios;
using Proyecto_Chatbot.Catalogos;

namespace Proyecto_Chatbot;

/// <summary>
/// Clase que calcula la reputacion del usuario.
/// No creamos una nueva interfaz para cumplir con el patron DIP
/// porque nos parecio que el algoritmo de esta clase es muy simple
/// y no va a cambiar en cuanto al calculo.
/// 
/// EXPERT: el usuario no puede ser el experto en informacion y calcular su propia reputación,
/// por lo que se crea una clase con los métodos para calcular la reputación de los empleadores
/// y de los trabajadores.
/// </summary>
public static class CalcularReputacion
{
    /// <summary>
    /// Calcula la reputacion del empleador. 
    /// DEFENSA:
    /// Como ya tenemos una
    /// </summary>
    /// <param name="empleador">Seria el usuario que se calcula la reputacion.</param>
    /// <returns>Devuelve un double con la reputación del empleador.</returns>
    public static double Calcular(Empleador empleador)
    {
        double promedio = 0;
        int cont = 0;
        foreach(Contratacion element in Singleton<CatalogoContratacion>.Instance.Contratacion)
        {
            if (element.Empleador.Id == empleador.Id && element.CalificacionEmpleador != null)
            {
                promedio += element.CalificacionEmpleador.Puntaje;
                cont += 1;
            }
        }

        if (cont > 0)
        {
            promedio = promedio / cont;
            return promedio;
        }

        return 0;
    }

    /// <summary>
    /// Calcula la reputación del trabajador.
    /// </summary>
    /// <param name="trabajador">Seria el usuario al que se calcula la reputacion.</param>
    /// <returns>Devuelve un double con la reputación del trabajador.</returns>
    public static double Calcular(Trabajador trabajador)
    {
        double promedio = 0;
        int cont = 0;
        foreach(Contratacion element in Singleton<CatalogoContratacion>.Instance.Contratacion)
        {
            if (element.OfertaLaboral.Trabajador.Id == trabajador.Id && element.CalificacionTrabajador != null)
            {
                promedio += element.CalificacionTrabajador.Puntaje;
                cont += 1;
            }
        }

        if (cont > 0)
        {
            promedio = promedio / cont;
            return promedio;
        }

        return 0;
    }
}

