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
/// Clase que almacena todos los administradores del programa, permitiendo agregarlos, eliminarlos y seleccionarlos.
/// Aplicamos el patrón Creator y el principio Expert de forma que esta clase es la responsable de crear, eliminar y manipular administradores. 
/// Esto es porque utiliza instancias de la clase Administrador, por lo tanto, podemos decir que la contiene o la agrega,
/// además de ser la clase experta, es decir, tiene la información necesaria para realizar la creación del objeto.
/// </summary>
public class ListaAdministradores : IJsonConvertible
{
    /// <summary>
    /// Lista para almacenar los administradores.
    /// </summary>
    [JsonInclude]
    public List<Administrador> Administradores = new List<Administrador>();

    /// <summary>
    /// Método para agregar administrador a la lista de administradores y guardarlos.
    /// </summary>
    /// <param name="nombre">nombre del administrador.</param>
    /// <param name="email">email del administrador.</param>
    /// <param name="id">id del administrador.</param>
    public void AgregarAdministrador(string nombre, string email, long id)
    {
        Administrador administrador = new Administrador(nombre, email, id);
        this.Administradores.Add(administrador);
    }

    /// <summary>
    /// Método para eliminar administradores de la lista.
    /// </summary>
    /// <param name="id">id del administrador a eliminar.</param>
    public void EliminarAdministrador(long id)
    {
        Administrador administradorEliminado = this.SeleccionarAdministrador(id);
        this.Administradores.Remove(administradorEliminado); 
    }

    /// <summary>
    /// Mpetodo para seleccionar un administrador.
    /// </summary>
    /// <param name="idAdministrador">id del administrador a seleccionar.</param>
    /// <returns>Retorna el objeto administrador en base a su id.</returns>
    public Administrador SeleccionarAdministrador(long idAdministrador)
    {
        foreach(Administrador element in this.Administradores)
        {
            if (element.Id == idAdministrador)
            {
                return element;
            }
        }  
        return null; 
    }

    /// <summary>
    /// Serializa (convierte) valores a .json.
    /// </summary>
    /// <returns>Devuelve el json en forma de string.</returns>
    public string ConvertToJson()
    {
        string jsonString = JsonSerializer.Serialize(this.Administradores);
        try
        {
            File.WriteAllText("../../src/Program/Administradores.json", jsonString);
        }
        catch(System.IO.DirectoryNotFoundException)
        {
            File.WriteAllText("../../../../../src/Program/Administradores.json", jsonString);
        }
        return jsonString;
    }
    
    /// <summary>       
    /// Carga el contenido del .json.
    /// </summary>
    public void LoadFromJson(string json)
    {
        List<Administrador> deserialized = JsonSerializer.Deserialize<List<Administrador>>(json);
        this.Administradores = deserialized;
    }

    /// <summary>       
    /// Muestra el contenido del .json.
    /// </summary>
    public string ShowFile()
    {
        string json;
        try
        {
            json = File.ReadAllText("../../src/Program/Administradores.json");
            return json;
        }
        catch(System.IO.DirectoryNotFoundException)
        {
            json = File.ReadAllText("../../../../../src/Program/Administradores.json");
            return json;
        }
    }
}
