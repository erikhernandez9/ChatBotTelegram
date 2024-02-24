using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Proyecto_Chatbot;
using System.Text;

namespace Proyecto_Chatbot.Catalogos
{
    /// <summary>
    /// Clase que almacena todas las categorias del programa, permitiendo agregarlas, eliminarlas y seleccionarlas.
    /// Aplicamos el patrón Creator y el principio Expert de forma que esta clase es la responsable de crear, eliminar y manipular categorías.
    /// Esto es porque utiliza instancias de la clase Categoria, por lo tanto, podemos decir que la contiene o la agrega,
    /// además de ser la clase experta, es decir, tiene la información necesaria para realizar la creación del objeto.
    /// Además, esta clase implementa la interfaz IJsonConvertible, por lo que se esta usando el principio DIP, para implementar
    /// el uso del Json. Las categorías del catálogo se almacenan en un archivo .json para guardarlas.
    /// </summary>
    public class CatalogoCategorias : IJsonConvertible
    {
        /// <summary>
        /// Lista que almacena todas las categorias.
        /// </summary>        
        [JsonInclude]
        public List<Categoria> Categorias = new List<Categoria>();

        /// <summary>
        /// Metodo que permite crear una categoria, serializarla y agregarla al catalogo.
        /// </summary>
        /// <param name="nombre">Representa el nombre de la categoria a agregar.</param>
        /// <param name="descripcion">Representa la descripcion de la categoria a agregar.</param>
        public void AgregarCategoria(string nombre, string descripcion)
        {
            Categoria categoria = new Categoria(nombre, descripcion);
            if (!this.Categorias.Contains(categoria))
            {
                this.Categorias.Add(categoria);
                this.ConvertToJson();
            }
            else
            {
                throw new ExcepcionConstructor("Ya existe esta categoria");
            }
        }

        /// <summary>
        /// Metodo que elimina una categoria.
        /// </summary>
        /// <param name="nombre">Representa el nombre que identifica a la categoria para eliminarla.</param>
        public void EliminarCategoria(string nombre)
        {   
            Singleton<CatalogoCategorias>.Instance.LoadFromJson(Singleton<CatalogoCategorias>.Instance.ShowFile());
            Categoria categoriaEliminada = this.SeleccionarCategorias(nombre);
            this.Categorias.Remove(categoriaEliminada);
        }

        /// <summary>
        /// Metodo que permite seleccionar una categoria.
        /// </summary>
        /// <param name="nombreCategoria">Nombre de la categoria que se quiere seleccionar.</param>
        /// <returns>Retorna un objeto Categoría en base al string de su nombre.</returns>
        public Categoria SeleccionarCategorias(string nombreCategoria)
        {
            foreach (Categoria element in this.Categorias)
            {
                if (element.Nombre == nombreCategoria)
                {
                    return element;
                }
            }

            return null; 
        }

        /// <summary>
        /// Serializa (convierte) valores de string a .json.
        /// </summary>
        public string ConvertToJson()
        {
            string jsonString = JsonSerializer.Serialize(Categorias);
            try
            {
                File.WriteAllText("../../src/Program/Categorias.json", jsonString);
            }
            catch(System.IO.DirectoryNotFoundException)
            {
                File.WriteAllText("../../../../../src/Program/Categorias.json", jsonString);
            }
            return jsonString;
        }

        /// <summary>       
        /// Carga el contenido del .json.
        /// </summary>
        /// <param name="json">El json a serializar.</param>
        public void LoadFromJson(string json)
        {
            List<Categoria> deserialized = JsonSerializer.Deserialize<List<Categoria>>(json);
            this.Categorias = deserialized;
        }

        /// <summary>       
        /// Muestra el contenido del .json.
        /// </summary>
        /// <returns>Devuelve el .json de catgorias en formato string.</returns>
        public string ShowFile()
        {
            string json;
            try
            {
                json = File.ReadAllText("../../src/Program/Categorias.json");
                return json;
            }
            catch(System.IO.DirectoryNotFoundException)
            {
                json = File.ReadAllText("../../../../../src/Program/Categorias.json");
                return json;
            }
        }

        /// <summary>
        /// Método imprimir para que se muestre en el chat la lista de categorías marcada en el .json.
        /// Se utilizó un StringBuilder para tomar las categorías de donde se indicó anteriormente y 
        /// construir un string que muestre, en forma de lista, qué categorias hay actualmente.
        /// </summary>
        /// <returns>Un string que enlista todos los nombres de las categorías.</returns>
        public StringBuilder Imprimir()
        {
            StringBuilder s = new StringBuilder();
            foreach (Categoria cat in this.Categorias)
            {
                s.Append($"\n{cat.Nombre}");
            }

            return s;
        }
    }
}
