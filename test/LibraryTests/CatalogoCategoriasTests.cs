using NUnit.Framework;
using Proyecto_Chatbot;
using Proyecto_Chatbot.Usuarios;
using Proyecto_Chatbot.Catalogos;
using System;
namespace Tests
{
    /// <summary>
    /// ESTOS TEST PERTENECEN A LA USER STORY:
    /// "Como administrador, quiero poder indicar categorías sobre
    /// las cuales se realizarán las ofertas de servicios para que de esa forma,
    /// los trabajadoras puedan clasificarlos." Se decició utilizar el patrón Creator, estas tareas
    /// de creación y eliminación de categorías se le asignaron a la clase "Catálogo Categorías".
    /// </summary>
    
    [TestFixture]
    public class CatalogoCategoriasTests
    {
        /// <summary>
        /// User story.
        /// Prueba que se creen categorías, para ello, se llama al catálogo de categorías y se utiliza el método de agregar categoría
        /// que pide como parametro en nombre y la descripción de la categoría, se fija si no existe y, de ser así, lo agrega a la
        /// lista de categorías y la serializa.
        /// </summary>
        [Test]
        public void CrearCategoriaTest()
        {
            Singleton<CatalogoCategorias>.Instance.Categorias.Clear();
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("Panaderia", "Descripcion1");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("Carnicería", "Descripcion2");
            Assert.IsNotEmpty(Singleton<CatalogoCategorias>.Instance.Categorias);
        }
        
        /// <summary>
        /// Prueba que se eliminen categorías, para ello creamos una categoria con el fin de eliminarla más adelante.
        /// Una vez creada, llamamos al método eliminar categoría del catálogo categorías, que se encarga de
        /// seleccionar la categoría enviada por parámetro y luego eliminarla.
        /// </summary>
        [Test]
        public void EliminarCategoriaTest()
        {
            Singleton<CatalogoCategorias>.Instance.Categorias.Clear();
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("Panaderia", "Descripcion1");
            Singleton<CatalogoCategorias>.Instance.EliminarCategoria("Panaderia");
            Assert.IsEmpty(Singleton<CatalogoCategorias>.Instance.Categorias);
        }

        /// <summary>
        /// Prueba la búsqueda de categorías, se selecciona de qué área se quiere buscar. Para realizar esta tarea, se
        /// envía por parámetro el nombre de la categoría deseada.
        /// Para hacer uso de este método, creamos dos categorías de la forma previamente explicada.
        /// </summary>
        [Test]
        public void SeleccionarCategoriaTest()
        {
            Singleton<CatalogoCategorias>.Instance.Categorias.Clear();

            Categoria Categoria1 = new Categoria("Panaderia", "Descripcion1");
            Categoria Categoria2 = new Categoria("Lavanderia", "Descripcion2");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("Panaderia", "Descripcion1");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("Lavanderia", "Descripcion2");
            Categoria guardado = Singleton<CatalogoCategorias>.Instance.SeleccionarCategorias("Panaderia");
            Assert.AreEqual(guardado.Nombre, Categoria1.Nombre);
        }

        /// <summary>
        /// Prueba la creación de una categoría ya creada anteriormente. Para esto, creamos una categoría con una nombre
        /// y luego creamos otra con el mismo nombre para comprobar si se ejecuta una excepción.
        /// </summary>
        [Test]
        public void CategoriaRepetidaTest()
        {
            try
            {
                Singleton<CatalogoCategorias>.Instance.AgregarCategoria("Panaderia", "Descripcion1");
                Singleton<CatalogoCategorias>.Instance.AgregarCategoria("Panaderia", "Descripcion1");
                Assert.Throws<ExcepcionConstructor>(delegate { throw new ExcepcionConstructor(); });
            }
            catch (ExcepcionConstructor e)
            {
            }
        }

        /// <summary>
        /// Prueba el guardado de datos de categoria en un archivo .json. Para esto, creamos categorías y las guardamos en un archivo .json
        /// luego las traemos y procedemos a comprobar si son correctas.
        /// </summary>
        [Test]
        public void GuardarTraerDatosJsonFileTest()
        {
            Singleton<CatalogoCategorias>.Instance.LoadFromJson(Singleton<CatalogoCategorias>.Instance.ShowFile());
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("Lavador", "Descripcion1");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("Polleria", "Descripcion2");
            Singleton<CatalogoCategorias>.Instance.ConvertToJson();
            Singleton<CatalogoCategorias>.Instance.LoadFromJson(Singleton<CatalogoCategorias>.Instance.ShowFile());

            Assert.AreEqual(Singleton<CatalogoCategorias>.Instance.Categorias[Singleton<CatalogoCategorias>.Instance.Categorias.Count - 1].Nombre, "Polleria");
            Assert.AreEqual(Singleton<CatalogoCategorias>.Instance.Categorias[Singleton<CatalogoCategorias>.Instance.Categorias.Count - 1].Descripcion, "Descripcion2");
        }
        
        /// <summary>
        /// Este test verifica si aparece la excepcion luego de enviar valores no esperados.
        /// Este test lo hicimos aca porque al ser uno solo para OfertaLaboral, nos parecia mejor ponerlo aca.
        /// </summary>
        [Test]
        public void ParametrosIncorrectosCategoriaTest()
        {
            Singleton<CatalogoOfertas>.Instance.Ofertas.Clear();
            Singleton<CatalogoCategorias>.Instance.Categorias.Clear();

            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("panaderia", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("mecanico", "pancitos");

            Empleador nacho = new Empleador("Nacho", "nacho@gmail.com", "098473729", 1, "ubicacion");
            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            try
            {
                Singleton<CatalogoOfertas>.Instance.AgregarOferta(null, null, "Bueno", 2);
                Assert.Throws<ExcepcionConstructor>(delegate { throw new ExcepcionConstructor(); });
            } 
            catch (ExcepcionConstructor e)
            {
            }
        }
    }
}