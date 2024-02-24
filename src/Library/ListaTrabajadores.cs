using System.Collections;
using System.Runtime.Serialization.Json;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Proyecto_Chatbot.Usuarios;
using System.Collections.Generic;
namespace Proyecto_Chatbot;

/// <summary>
/// Clase que almacena todos los trabajadores del programa, permitiendo agregarlos, eliminarlos y seleccionarlos.
/// Aplicamos el patrón Creator y el patrón Expert de forma que esta clase es la responsable de crear, eliminar y manipular trabajadores. 
/// Esto es porque utiliza instancias de la clase Trabajador, por lo tanto, podemos decir que la contiene o la agrega,
/// además de ser la clase experta, es decir, tiene la información necesaria para realizar la creación del objeto.
/// </summary>
public class ListaTrabajadores : IJsonConvertible

{
    /// <summary>
    /// Lista que almacena todos los trabajadores.
    /// </summary>        
    [JsonInclude]
    public List<Trabajador> Trabajadores = new List<Trabajador>();


    /// <summary>
    /// Método que permite crear un Trabajador, serializarlo y agregarlo a la lista.
    /// </summary>
    /// <param name="nombre">Representa el nombre del trabajador a agregar.</param>
    /// <param name="email">Representa el email del trabajador a agregar.</param>
    /// <param name="telefono">Representa el telefono del trabajador a agregar.</param>
    /// <param name="id">Representa el id del trabajador a agregar.</param>
    /// <param name="ubicacion">Representa la ubicacion del trabajador a agregar.</param>
    public void AgregarTrabajador(string nombre, string email, string telefono, long id, string ubicacion)
    {
        Trabajador trabajador = new Trabajador(nombre, email, telefono, id, ubicacion);
        this.Trabajadores.Add(trabajador);
        this.ConvertToJson();
    }
    /// <summary>
    /// Método que notifica un trabajador cuando se elimina una oferta.
    /// </summary>
    /// <param name="trabajador">Representa el trabajador que identifica al trabajador para eliminarla.</param>
    /// <param name="mensaje">Representa el trabajador que identifica al trabajador para eliminarla.</param>
    public void NotificarTrabajador (Trabajador trabajador, string mensaje)
    {
        trabajador.Notificaciones.Add(mensaje);
    }


    /// <summary>
    /// Método que elimina un trabajador.
    /// </summary>
    /// <param name="id">Representa el id que identifica al trabajador para eliminarla.</param>
    public void EliminarTrabajador(long id)
    {
        Singleton<ListaTrabajadores>.Instance.LoadFromJson(Singleton<ListaTrabajadores>.Instance.ShowFile());
        Trabajador trabajadorEliminado = this.SeleccionarTrabajador(id);
        this.Trabajadores.Remove(trabajadorEliminado); 
    }
    /// <summary>
    /// Método que permite seleccionar un trabajador.
    /// </summary>
    /// <param name="idTrabajador">Id del trabajador que se quiere seleccionar.</param>
    /// <returns>Retorna el trabajador seleccionado.</returns>
    public Trabajador SeleccionarTrabajador(long idTrabajador)
    {
        foreach(Trabajador element in this.Trabajadores)
        {
            if (element.Id == idTrabajador)
            {
                return element;
            }
        }  
        return null; 
    }

    /// <summary>
    /// Serializa (convierte) valores a .json.
    /// </summary>
    public string ConvertToJson()
    {
        string jsonString = JsonSerializer.Serialize(this.Trabajadores);
        try
        {
            File.WriteAllText("../../src/Program/Trabajadores.json", jsonString);
        }
        catch(System.IO.DirectoryNotFoundException)
        {
            File.WriteAllText("../../../../../src/Program/Trabajadores.json", jsonString);
        }
        return jsonString;
    }
    
    /// <summary>       
    /// Muestra el contenido del .json.
    /// </summary>
    /// <returns>devuelve el json como string.</returns>
    public string ShowFile()
    {
        string json;
        try
        {
            json = File.ReadAllText("../../src/Program/Trabajadores.json");
            return json;
        }
        catch(System.IO.DirectoryNotFoundException)
        {
            json = File.ReadAllText("../../../../../src/Program/Trabajadores.json");
            return json;
        }
        
    }
    
    /// <summary>       
    /// Carga el contenido del .json.
    /// </summary>
    /// <param name="json">el json a deserializar.</param>
    public void LoadFromJson(string json)
    {
        List<Trabajador> deserialized = JsonSerializer.Deserialize<List<Trabajador>>(json);
        this.Trabajadores = deserialized;
    }
}