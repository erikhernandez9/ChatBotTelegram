using System;
using System.Text.RegularExpressions;

namespace Proyecto_Chatbot;
/// <summary>
/// Clase que calcula la reputacion del usuario.
/// No creamos una nueva interfaz para cumplir con los patrones
/// y principios DIP ni ISP porque nos pareció que el algoritmo
/// de esta clase es muy simple y no va a cambiar en cuanto al cálculo.
/// Se utilizó el patrón EXPERT. Se creó una clase individual para
/// la verificación ya que el usuario no es el experto en información,
/// por lo que no puede verificar si un string tiene números.
/// </summary>
public static class VerificarString
{
    /// <summary>
    /// Verifica si el string solo tiene numeros.
    /// </summary>
    /// <param name="palabra">palabra cualquiera.</param>
    /// <returns>devuelve true si tiene numeros y no letras.</returns>
    public static bool TieneSoloNumeros(string palabra)
    {
        int num;
        return int.TryParse(palabra, out num);
    }
}