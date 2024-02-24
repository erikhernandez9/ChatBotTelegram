using System.Net;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Proyecto_Chatbot.Usuarios;
using System.Linq;
using System.Text;
using Proyecto_Chatbot.Locations.Client;

namespace Proyecto_Chatbot.Catalogos
{
    /// <summary>
    /// Clase que representa el catalogo de ofertas, en esta se almacenan todas las ofertas,
    /// permitiendo agregarlas, eliminarlas y buscarlas segun algunos filtros.
    /// Aplicamos el patrón Creator y el principio Expert de forma que esta clase es la responsable de crear, eliminar y manipular ofertas. 
    /// Esto es porque utiliza instancias de la clase OfertaLaboral, por lo tanto, podemos decir que la contiene o la agrega,
    /// además de ser la clase experta, es decir, tiene la información necesaria para realizar la creación del objeto.
    /// </summary>
    public class CatalogoOfertas : IJsonConvertible
    {
        /// <summary>
        /// Almacena todas las ofertas laborales creadas en el programa.
        /// </summary>
        public List<OfertaLaboral> Ofertas = new List<OfertaLaboral>();

        /// <summary>
        /// Metodo que crea la oferta laboral y la agrega al catalogo.
        /// </summary>
        /// <param name="categoria">Es la categoria asociada a la oferta a agregar.</param>
        /// <param name="trabajador">Es el trabajador asociado a la oferta a agregar.</param>
        /// <param name="descripcion">Es la descripcion de la oferta a agregar.</param>
        /// <param name="precio">Es el precio de la oferta a agregar.</param>
        public void AgregarOferta(Categoria categoria, Trabajador trabajador, string descripcion, int precio)
        {
            OfertaLaboral oferta = new OfertaLaboral(this.Ofertas.Count, categoria, trabajador, descripcion, precio);
            this.Ofertas.Add(oferta);
            this.ConvertToJson();
        }

        /// <summary>
        /// Metodo que elimina la oferta de la lista.
        /// </summary>
        /// <param name="id">id de la oferta a eliminar.</param>
        public void EliminarOferta(int id)
        {
            Singleton<CatalogoOfertas>.Instance.LoadFromJson(Singleton<CatalogoOfertas>.Instance.ShowFile());
            OfertaLaboral ofertaLaboral = this.BuscarOferta(id);
            this.Ofertas.Remove(ofertaLaboral);     
            Notificaciones.NotificacionTrabajador(ofertaLaboral);
        }

        /// <summary>
        /// Busca la oferta segun la reputacion que tengan los trabajadores.
        /// </summary>
        /// <returns>Devuelve la lista ordenada segun la reputacion.</returns>
        /// <param name="nombreCategoria">el nombre de la categoría de las ofertas.</param>
        public List<OfertaLaboral> BuscarOfertasPorCalificacion (string nombreCategoria)
        {
            List<OfertaLaboral> resultado = this.BuscarOfertaPorCategoria(nombreCategoria);
            resultado = resultado.OrderByDescending(resultado => resultado.Trabajador.Reputacion).ToList();
            resultado.Reverse();
            return resultado;
        }

        /// <summary>
        /// Un string builder que construye un string de una lista con todas las ofertas laborales en una categoría,
        /// filtradas por la calificación más alta, para que el bot lo muestre en pantalla de forma "amigable". Se indica la id del servicio, 
        /// el trabajador y la descripción de la oferta.
        /// Esto forma parte de las USER STORIES.
        /// "Como empleador, quiero ver el resultado de las búsquedas de ofertas de trabajo ordenado en forma descendente 
        ///  por reputación, es decir, las de mejor reputación primero para que de esa forma, pueda contratar un servicio".
        /// </summary>
        /// <param name="nombreCategoria">el nombre de la categoría donde se buscan las ofertas para agregarlas.</param>
        /// <returns>devuelve un string con apariencia de listado conteniendo todas las ofertas laborales en una categoría.</returns>
        public StringBuilder ImprimirFiltradoCalificacion(string nombreCategoria)
        {
            StringBuilder s = new StringBuilder();
            int cont = 0;
            foreach(OfertaLaboral servicio in this.BuscarOfertasPorCalificacion(nombreCategoria))
            { 
                cont++; 
                s.Append($"\n{cont}. Trabajador asignado: {servicio.Trabajador.Nombre}, Reputacion: {servicio.Trabajador.Reputacion}, Descripción: {servicio.Descripcion}");
            }
            return s;
        }

        /// <summary>
        /// Busca la oferta segun la distancia que tengan los trabajadores y el empleador.
        /// El más cercano es el que aparece primero. Este es un método de filtrado que se pide en las USER STORIES.
        /// </summary>
        /// <param name="nombreCategoria">La categoria que quiere buscar.</param>
        /// <param name="empleador">Es el empleador que busca a los trabajadores.</param>
        /// <returns>Devuelve la lista ordenada segun la distancia.</returns>
        public List<OfertaLaboral> BuscarOfertasPorUbicacion (string nombreCategoria, Empleador empleador)
        {
            List<OfertaLaboral> resultado = this.BuscarOfertaPorCategoria(nombreCategoria);
            LocationApiClient client = new LocationApiClient();
            resultado = resultado.OrderByDescending(resultado => client.GetDistance(resultado.Trabajador.Ubicacion, empleador.Ubicacion).TravelDistance).ToList();
            resultado.Reverse();
            return resultado;
        }

        /// <summary>
        /// Método para obtener un string de la lista de Ofertas por ubicación.
        /// </summary>
        /// <param name="nombreCategoria">La categoria que quiere buscar.</param>
        /// <param name="idEmpleador">Es el empleador que busca a los trabajadores.</param>
        /// <returns>Devuelve la lista ordenada segun la distancia en forma de string.</returns>
        public StringBuilder ImprimirFiltradoUbicacion(string nombreCategoria, string idEmpleador)
        {
            StringBuilder s = new StringBuilder();
            LocationApiClient client = new LocationApiClient();
            Empleador empleador = Singleton<ListaEmpleadores>.Instance.SeleccionarEmpleador(long.Parse(idEmpleador));
            int cont = 0;
            foreach(OfertaLaboral servicio in this.BuscarOfertasPorUbicacion(nombreCategoria, empleador))
            {
                cont++;
                s.Append($"\n{cont}. Trabajador asignado: {servicio.Trabajador.Nombre}, Distancia: {client.GetDistance(servicio.Trabajador.Ubicacion, empleador.Ubicacion).TravelDistance}, Descripción: {servicio.Descripcion}");
            }
            return s;
        }

        /// <summary>
        /// Ordena las ofertas segun el precio que tengan. Esto es un método de filtrado solicitado en las USER STORIES.
        /// </summary>
        /// <param name="nombreCategoria">La categoria filtro.</param>
        /// <returns>Devuelve la lista ordenada segun el precio de las ofertas de menor a mayor.</returns>
        public List<OfertaLaboral> BuscarOfertasPorPrecio (string nombreCategoria)
        {
            List<OfertaLaboral> resultado = this.BuscarOfertaPorCategoria(nombreCategoria);
            resultado = resultado.OrderByDescending(resultado => resultado.Precio).ToList();
            resultado.Reverse();
            return resultado;
        }

        /// <summary>
        /// Un string builder que toma la lista de ofertas dentro de una categoría determinada,
        /// filtrándolas por precio, y construye un string con las mismas, indicando la id del servicio, el trabajador y la descripción
        /// de la oferta.
        /// </summary>
        /// <param name="nombreCategoria">El nombre de la categoría solicitada.</param>
        /// <returns>Un string en forma de listado con todas las ofertas de una cierta categoría ordenadas por precio (ascendente).</returns>
        public StringBuilder ImprimirFiltradoPrecio(string nombreCategoria)
        {
            StringBuilder s = new StringBuilder();
            int cont = 0;
            foreach(OfertaLaboral servicio in this.BuscarOfertasPorPrecio(nombreCategoria))
            {
                cont++;
                s.Append($"\n{cont}. Trabajador asignado: {servicio.Trabajador.Nombre}, Descripción: {servicio.Descripcion}, Precio: {servicio.Precio}");
            }
            return s;
        }

        /// <summary>
        /// Metodo que busca una oferta en especifico.
        /// </summary>
        /// <param name="id">Representa el id de la oferta a buscar.</param>
        /// <returns>El objeto OfertaLaboral que pertenece al id correspondiente.</returns>
        public OfertaLaboral BuscarOferta(int id)
        {
            foreach(OfertaLaboral element in this.Ofertas)
            {
                if (element.Id == id)
                {
                    return element;
                }
            }

            return null;
        }

        /// <summary>
        /// Método para filtrar las ofertas por categoría. Esto es un método que se pide en las USER STORIES.
        /// "Como empleador, quiero buscar ofertas de trabajo, opcionalmente filtrando por categoría para que de esa forma, 
        /// pueda contratar un servicio".
        /// </summary>
        /// <param name="nombreCategoria">Nombre de la categoría sobre la que el usuario decide ver las categorías.</param>
        /// <returns>Una lista con todas las ofertas ubicadas en esa categoría determinada.</returns>
        public List<OfertaLaboral> BuscarOfertaPorCategoria(string nombreCategoria)
        {
            List<OfertaLaboral> lista = new List<OfertaLaboral>();
            foreach(OfertaLaboral element in this.Ofertas)
            {
                if (element.Categoria.Nombre == nombreCategoria)
                {
                    lista.Add(element);
                }
            }
            return lista;
        }

        /// <summary>
        /// Se utilizó un StringBuilder para tomar las ofertas de donde se indicó anteriormente y 
        /// construir un string que muestre, en forma de lista, qué ofertas laborales activas existen.
        /// Un string builder que toma todas las ofertas ubicadas en cierta categoría y construye un string en forma de listado
        /// para mostrarlo al usuario.
        /// Esto es parte de la misma User Story que BuscarOfertaPorCategoria.
        /// </summary>
        /// <param name="nombreCategoria">nombre de la categoría a buscar.</param>
        /// <returns>un string en forma de listado con todas las ofertas en cierta categoría.</returns>
        public StringBuilder ImprimirFiltradoCategorias(string nombreCategoria)
        {
            StringBuilder s = new StringBuilder();
            int cont = 0;
            foreach(OfertaLaboral servicio in this.BuscarOfertaPorCategoria(nombreCategoria))
            {
                cont ++; 
                s.Append($"\n{cont}. Trabajador asignado: {servicio.Trabajador.Nombre}, Descripción: {servicio.Descripcion}");
            }
            return s;
        }
        /// <summary>
        /// Un string builder para formar un string que sea el "renglón" de la lista del catálogo de ofertas;
        /// pone cada oferta indicando el trabajador asignado, la descripción y una ID.
        /// </summary>
        /// <returns></returns>
        public StringBuilder Imprimir()
        {
            StringBuilder s = new StringBuilder();
            foreach (OfertaLaboral oferta in this.Ofertas)
            {
                s.Append($"\n{oferta.Id + 1}. Trabajador asignado: {oferta.Trabajador.Nombre}, Descripción: {oferta.Descripcion}");
            }

            return s;
        }
        /// <summary>
        /// Serializa valores a .json.
        /// </summary>
        public string ConvertToJson()
        {
            string jsonString = JsonSerializer.Serialize(this.Ofertas);
            try
            {
                File.WriteAllText("../../src/Program/Ofertas.json", jsonString);
            }
            catch(System.IO.DirectoryNotFoundException)
            {
                File.WriteAllText("../../../../../src/Program/Ofertas.json", jsonString);
            }
            return jsonString;
        }

        /// <summary>
        /// Carga el contenido del .json.
        /// </summary>
        /// <param name="json">el json a deserializar.</param>
        public void LoadFromJson(string json)
        {
            List<OfertaLaboral> deserialized = JsonSerializer.Deserialize<List<OfertaLaboral>>(json);
            this.Ofertas = deserialized;
        }

        /// <summary>
        /// Muestra el contenido del .json.
        /// </summary>
        /// <return>retorna el json en formato string.</return>
        public string ShowFile()
        {
            string json;
            try
            {
                json = File.ReadAllText("../../src/Program/Ofertas.json");
                return json;
            }
            catch(System.IO.DirectoryNotFoundException)
            {
                json = File.ReadAllText("../../../../../src/Program/Ofertas.json");
                return json;
            }
        }

        /// <summary>
        /// Elimina una oferta.
        /// </summary>
        /// <param name="nombreOferta">Nombre de la oferta a eliminar.</param>
        internal void EliminarOferta(string nombreOferta)
        {
            throw new NotImplementedException();
        }
    }
}