using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Proyecto_Chatbot.Usuarios;
using System.Collections.Generic;
namespace Proyecto_Chatbot;

/// <summary>
/// Clase que almacena todas los empleadores del programa, permitiendo agregarlos, eliminarlos y seleccionarlos.
/// Aplicamos el patrón Creator y el patrón Expert de forma que esta clase es la responsable de crear, eliminar y manipular empleadores. 
/// Esto es porque utiliza instancias de la clase Empleador, por lo tanto, podemos decir que la contiene o la agrega,
/// además de ser la clase experta, es decir, tiene la información necesaria para realizar la creación del objeto.
/// </summary>
public class ListaEmpleadores : IJsonConvertible

{
    /// <summary>
    /// Lista que almacena todos los empleadores.
    /// </summary> 
    [JsonInclude]
    public List<Empleador> Empleadores = new List<Empleador>();

    /// <summary>
    /// Método que permite crear un Empleador, serializarlo y agregarlo a la lista.
    /// </summary>
    /// <param name="nombre">Representa el nombre del empleador a agregar.</param>
    /// <param name="email">Representa el email del empleador a agregar.</param>
    /// <param name="telefono">Representa el telefono del empleador a agregar.</param>
    /// <param name="id">Representa el id del empleador a agregar.</param>
    /// <param name="ubicacion">Representa la ubicacion del empleador a agregar.</param>
    public void AgregarEmpleador(string nombre, string email, string telefono, long id, string ubicacion)
    {
        Empleador empleador = new Empleador(nombre, email, telefono, id, ubicacion);
        this.Empleadores.Add(empleador);
        this.ConvertToJson();
    }

    /// <summary>
    /// Método que elimina un empleador.
    /// </summary>
    /// <param name="id">Representa el id que identifica al empleador para eliminarla.</param>
    public void EliminarEmpleador(long id)
    {
        Empleador empleadorEliminado = this.SeleccionarEmpleador(id);
        this.Empleadores.Remove(empleadorEliminado); 
    }

    /// <summary>
    /// Método que permite seleccionar un empleador.
    /// </summary>
    /// <param name="idEmpleador">Id del empleador que se quiere seleccionar.</param>
    /// <returns>Devuelve un objeto empleador en base a si id.</returns>
    public Empleador SeleccionarEmpleador(long idEmpleador)
    {
        foreach(Empleador element in this.Empleadores)
        {
            if (element.Id == idEmpleador)
            {
                return element;
            }
        }  
        return null; 
    }

    /// <summary>
    /// Serializa valores a .json.
    /// </summary>
    public string ConvertToJson()
    {
        string jsonString = JsonSerializer.Serialize(this.Empleadores);
        try
        {
            File.WriteAllText("../../src/Program/Empleadores.json", jsonString);
        }
        catch(System.IO.DirectoryNotFoundException)
        {
            File.WriteAllText("../../../../../src/Program/Empleadores.json", jsonString);
        }
        return jsonString;
    }

    /// <summary>       
    /// Carga el contenido del .json.
    /// </summary>
    public void LoadFromJson(string json)
    {
        List<Empleador> deserialized = JsonSerializer.Deserialize<List<Empleador>>(json);
        this.Empleadores = deserialized;
    }

    /// <summary>       
    /// Muestra el contenido del .json.
    /// </summary>
    public string ShowFile()
    {
        string json;
        try
        {
            json = File.ReadAllText("../../src/Program/Empleadores.json");
            return json;
        }
        catch(System.IO.DirectoryNotFoundException)
        {
            json = File.ReadAllText("../../../../../src/Program/Empleadores.json");
            return json;
        }
    }
}