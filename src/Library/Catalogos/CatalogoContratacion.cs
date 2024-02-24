using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Proyecto_Chatbot.Usuarios;
using Proyecto_Chatbot;
using System.Text;

namespace Proyecto_Chatbot.Catalogos
{
    /// <summary>
    /// Clase que almacena todas las contrataciones, permitiendo agregar, eliminar y buscar contrataciones.
    /// Aplicamos el patrón Creator y el principio Expert de forma que esta clase es la responsable de crear, eliminar y manipular contrataciones. 
    /// Esto es porque utiliza instancias de la clase Contratacion, por lo tanto, podemos decir que la contiene o la agrega,
    /// además de ser la clase experta, es decir, tiene la información necesaria para realizar la creación del objeto.
    /// </summary>
    public class CatalogoContratacion : IJsonConvertible
    {
        /// <summary>
        /// Lista que almacena todas las contrataciones.
        /// </summary>        
        [JsonInclude]
        public List<Contratacion> Contratacion = new List<Contratacion>();

        /// <summary>
        /// Método que crea una contratación y la agrega al católogo.
        /// </summary>
        /// <param name="ofertaLaboral">representa la oferta laboral de la contratación a agregar.</param>
        /// <param name="empleador">representa al empleador relacionado a la contratación a agregar.</param>
        /// <param name="descripcion">representa la descripción de la contratación a agregar.</param>
        public void AgregarContratacion(OfertaLaboral ofertaLaboral, Empleador empleador, string descripcion)
        {
            try
            {
                Contratacion contratacion = new Contratacion(this.Contratacion.Count, ofertaLaboral, empleador, descripcion);
                this.Contratacion.Add(contratacion);
                this.ConvertToJson();
            }
            catch (ExcepcionConstructor)
            {
                Console.WriteLine("Valores nulos");
            }
        }

        /// <summary>
        /// Método que elimina una contratación del catálogo.
        /// </summary>
        /// <param name="id">id de la contratación.</param>
        public void EliminarContratacion(long id)
        {
            Singleton<CatalogoContratacion>.Instance.LoadFromJson(Singleton<CatalogoContratacion>.Instance.ShowFile());
            Contratacion contratacionSeleccionada = this.BuscarContratacion(id);
            this.Contratacion.Remove(contratacionSeleccionada); 
        }
        
        /// <summary>
        /// Lista que almacena las contrataciones filtradas por la ID del empleador.
        /// </summary>
        /// <param name="id">id del empleador.</param>
        /// <returns> devuelve la lista de las contrataciones activas que tiene un empleador. </returns>
        public List<Contratacion> FiltrarContratacionesPorEmpleador(long id)
        {
            List<Contratacion> lista = new List<Contratacion>();
            foreach (Contratacion element in Singleton<CatalogoContratacion>.Instance.Contratacion)
            {
                if (element.Empleador.Id == id)
                {
                    lista.Add(element);
                }
            }
            return lista;
        }

        /// <summary>
        /// Lista que almacena las contrataciones (servicios contratados) que tiene el trabajador.
        /// </summary>
        /// <param name="id">id del trabajador.</param>
        /// <returns>devuelve una lista que toma todas las contrataciones que tiene el trabajador.</returns>
        public List<Contratacion> FiltrarContratacionesPorTrabajador(long id)
        {
            List<Contratacion> lista = new List<Contratacion>();
            foreach (Contratacion element in Singleton<CatalogoContratacion>.Instance.Contratacion)
            {
                if (element.OfertaLaboral.Trabajador.Id == id)
                {
                    lista.Add(element);
                }
            }
            return lista;
        }

        /// <summary>
        /// Método que busca una contratación en específico.
        /// </summary>
        /// <param name="id">Representa el id de la contratacion.</param>
        /// <returns>Retorna una contratación en base a su id.</returns>
        public Contratacion BuscarContratacion(long id)
        {
            foreach(Contratacion contratacion in this.Contratacion)
            {
                if (contratacion.Id == id)
                {
                    return contratacion;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Recorre todas las contrataciones para verificar si 
        /// no se calificaron en un mes, les asigna una calificacion
        /// neutral.
        /// </summary>
        public void ContratacionesNeutras()
        {
            foreach(Contratacion c in this.Contratacion)
            {
                c.CalificarNeutral();
            }
        }

        /// <summary>
        /// Serializa (convierte) valores a .json.
        /// </summary>
        public string ConvertToJson()
        {
            string jsonString = JsonSerializer.Serialize(this.Contratacion);
            try
            {
                File.WriteAllText("../../src/Program/Contrataciones.json", jsonString);
            }
            catch(System.IO.DirectoryNotFoundException)
            {
                File.WriteAllText("../../../../../src/Program/Contrataciones.json", jsonString);
            }
            return jsonString;
        }

        /// <summary>       
        /// Carga el contenido del .json.
        /// </summary>
        public void LoadFromJson(string json)
        {
            List<Contratacion> deserialized = JsonSerializer.Deserialize<List<Contratacion>>(json);
            this.Contratacion = deserialized;
        }

        /// <summary>       
        /// Muestra el contenido del .json.
        /// </summary>
        /// <returns>retorna el json de contrataciones en formato string.</returns>
        public string ShowFile()
        {
            string json;
            try
            {
                json = File.ReadAllText("../../src/Program/Contrataciones.json");
                return json;
            }
            catch(System.IO.DirectoryNotFoundException)
            {
                json = File.ReadAllText("../../../../../src/Program/Contrataciones.json");
                return json;
            }
        }

        /// <summary>
        /// Método imprimir para que se muestre en el chat la lista de contrataciones guardadas en el .json.
        /// Se utilizó un StringBuilder para tomar las contrataciones de donde se indicó anteriormente y 
        /// construir un string que muestre, en forma de lista, qué contrataciones vigentes existen.
        /// </summary>
        /// <returns>Un string que enlista todos los nombres de las contrataciones.</returns>
        public StringBuilder Imprimir()
        {
            StringBuilder s = new StringBuilder();
            foreach(Contratacion contrato in this.Contratacion)
            {
                if (contrato.Estado == true)
                {
                    s.Append(contrato.Descripcion);
                }
            }
            return s;
        }
    }
}
