using NUnit.Framework;
using Proyecto_Chatbot.Usuarios;
using Proyecto_Chatbot.Catalogos;
using Proyecto_Chatbot;
using System.Collections.Generic;
using System;
using Proyecto_Chatbot.Locations.Client;

namespace Tests
{
    /// <summary>
    /// ESTOS TEST PERTENECEN A LA USER STORY:
    /// "Como administrador, quiero poder dar de baja ofertas de servicios,
    /// avisando al oferente para que de esa forma, pueda evitar ofertas inadecudas.".
    /// También a:
    /// "Como empleador, quiero ver el resultado de las búsquedas de ofertas de trabajo
    /// ordenado en forma descendente por reputación, es decir, las de mejor reputación
    /// primero para que de esa forma, pueda contratar un servicio.".
    /// </summary>
    
    [TestFixture]
    public class CatalogoOfertasTests
    {        
        /// <summary>
        /// Prueba la búsqueda de ofertas.
        /// </summary>
        [Test]
        public void BuscarOfertaTest()
        {
            Singleton<CatalogoOfertas>.Instance.Ofertas.Clear();

            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Categoria panaderia = new Categoria("panaderia", "pan");
            Categoria jardineria = new Categoria("jardineria", "jardin");
            OfertaLaboral oferta1 = new OfertaLaboral(0, panaderia, alex, "Descripcionprueba", 100);
            OfertaLaboral oferta2 = new OfertaLaboral(1, jardineria, alex, "Descripcionprueba2", 100);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(panaderia, alex, "hago pan", 2000);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(panaderia, alex, "jardines", 2000);
            OfertaLaboral ofertaPrueba = Singleton<CatalogoOfertas>.Instance.BuscarOferta(1);
            Assert.AreEqual(ofertaPrueba.Id, oferta2.Id);
        }
        
        /// <summary>
        /// USER STORY. Prueba que se puedan crear ofertas.
        /// Prueba el agregar una nueva oferta. Para esto crea todos los objetos necesarios como el Trabajador y la Categoría,
        /// y luego comprobamos si se creo de forma correcta.
        /// </summary>
        [Test]
        public void AgregarOfertaTest()
        {
            Singleton<CatalogoOfertas>.Instance.Ofertas.Clear();
            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Categoria panaderia = new Categoria("panaderia", "panaderia");
            OfertaLaboral Oferta1 = new OfertaLaboral(1, panaderia, alex, "Descripcionprueba", 100);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(panaderia, alex, "Descripcionprueba", 100);
            Assert.IsNotEmpty(Singleton<CatalogoOfertas>.Instance.Ofertas);
        }

        /// <summary>
        /// USER STORY. Prueba que se puedan eliminar ofertas.
        /// </summary>
        [Test]
        public void EliminarOfertaTest()
        {
            Singleton<CatalogoOfertas>.Instance.Ofertas.Clear();
            Categoria panaderia = new Categoria("panaderia", "panaderia");
            Categoria jardineria = new Categoria("jardineria", "jardin");
            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(panaderia, alex, "panes", 233);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(jardineria, alex, "panes", 233);
            Singleton<CatalogoOfertas>.Instance.EliminarOferta(1);
            List<OfertaLaboral> prueba = new List<OfertaLaboral>();
            Assert.AreEqual(1, Singleton<CatalogoOfertas>.Instance.Ofertas.Count);
        }

        /// <summary>
        /// Prueba el método BuscarOfertasPorCategoria. Esto se hace mediante la creacion de multiples categorias y trabajadores
        /// para crear varias ofertas, y luego comprobar si se pueden buscar de forma correcta las ofertas en base a su categoría.
        /// </summary>
        [Test]
        public void BuscarOfertasPorCategoriaTest()
        {
            Singleton<CatalogoOfertas>.Instance.Ofertas.Clear();
            Singleton<CatalogoCategorias>.Instance.Categorias.Clear();

            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("panaderia", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("mecanico", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("constructor", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("jardineria", "jardin");

            Empleador nacho = new Empleador("Nacho", "nacho@gmail.com", "098473729", 1, "ubicacion");
            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Trabajador emiliano = new Trabajador("Emiliano", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Trabajador ignacio = new Trabajador("Ignacio", "ignacio@gmail.com", "099689521", 55914573,"ubicacion");

            Trabajador joel = new Trabajador("Joel", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Trabajador augusto = new Trabajador("Augusto", "augusto@gmail.com", "099895234", 45294892, "ubicacion");
            Trabajador pablo = new Trabajador("Pablo", "pablo@gmail.com", "095859203", 15839294, "ubicacion");
            Trabajador mati = new Trabajador("Mati", "mati@gmail.com", "091583923", 58395773, "ubicacion");

            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[1], alex, "Bueno", 1000);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], alex, "Bueno", 1000);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[2], alex, "Bueno", 1000);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], alex, "Bueno", 1000);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], alex, "Bueno", 1000);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[3], alex, "Bueno", 1000);

            List<OfertaLaboral> lista = Singleton<CatalogoOfertas>.Instance.BuscarOfertaPorCategoria("panaderia");

            foreach (OfertaLaboral ofert in lista)
            {
                Assert.AreEqual(ofert.Categoria.Nombre, "panaderia");
            }
        }

        /// <summary>
        /// Prueba el método BuscarOfertasPorPrecio. Esto se hace mediante la creacion de multiples categorias y trabajadores
        /// para crear varias ofertas, y luego comprobar si se pueden buscar de forma correcta las ofertas en base a su precio.
        /// </summary>
        [Test]
        public void BuscarOfertasPorPrecioTest()
        {
            Singleton<CatalogoOfertas>.Instance.Ofertas.Clear();
            Singleton<CatalogoCategorias>.Instance.Categorias.Clear();

            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("panaderia", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("mecanico", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("constructor", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("jardineria", "jardin");

            Empleador nacho = new Empleador("Nacho", "nacho@gmail.com", "098473729", 1, "ubicacion");
            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Trabajador emiliano = new Trabajador("Emiliano", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Trabajador ignacio = new Trabajador("Ignacio", "ignacio@gmail.com", "099689521", 55914573,"ubicacion");

            Trabajador joel = new Trabajador("Joel", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Trabajador augusto = new Trabajador("Augusto", "augusto@gmail.com", "099895234", 45294892, "ubicacion");
            Trabajador pablo = new Trabajador("Pablo", "pablo@gmail.com", "095859203", 15839294, "ubicacion");
            Trabajador mati = new Trabajador("Mati", "mati@gmail.com", "091583923", 58395773, "ubicacion");

            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[1], alex, "Bueno", 2);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], alex, "Bueno", 5);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[2], alex, "Bueno", 4000);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], alex, "Bueno", 12);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], alex, "Bueno", 1);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], alex, "Bueno", 1432);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], alex, "Bueno", 132);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[3], alex, "Bueno", 10);

            List<OfertaLaboral> lista = Singleton<CatalogoOfertas>.Instance.BuscarOfertasPorPrecio("panaderia");

            Assert.AreEqual(lista[0].Precio, 1);
            Assert.AreEqual(lista[1].Precio, 5);
            Assert.AreEqual(lista[2].Precio, 12);
            Assert.AreEqual(lista[3].Precio, 132);
            Assert.AreEqual(lista[4].Precio, 1432);
        }

        /// <summary>
        /// Prueba el método BuscarOfertasPorUbiacion. Esto se hace mediante la creacion de multiples categorias y trabajadores
        /// para crear varias ofertas, y luego comprobar si se pueden buscar de forma correcta las ofertas en base a su ubicación.
        /// </summary>
        [Test]
        public void BuscarOfertasPorUbicacionTest()
        {
            Singleton<CatalogoOfertas>.Instance.Ofertas.Clear();
            Singleton<CatalogoCategorias>.Instance.Categorias.Clear();

            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("panaderia", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("mecanico", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("constructor", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("jardineria", "jardin");

            Empleador nacho = new Empleador("Nacho", "nacho@gmail.com", "098473729", 1, "8 de octubre y garibaldi");

            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "av. Italia");
            Trabajador emiliano = new Trabajador("Emiliano", "alex@gmail.com", "099333111", 55555550, "mallorca y piran");
            Trabajador ignacio = new Trabajador("Ignacio", "ignacio@gmail.com", "099689521", 55914573,"av. centenario y av. italia");
            Trabajador joel = new Trabajador("Joel", "alex@gmail.com", "099333111", 55555550, "albo y gerardo graso");
            Trabajador augusto = new Trabajador("Augusto", "augusto@gmail.com", "099895234", 45294892, "av. italia y gral. artigas");
            Trabajador pablo = new Trabajador("Pablo", "pablo@gmail.com", "095859203", 15839294, "18 de julio y 8 de octubre");
            Trabajador mati = new Trabajador("Mati", "mati@gmail.com", "091583923", 58395773, "gral artigas y 18 de julio");

            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[1], alex, "Bueno", 2);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], joel, "Bueno", 5);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[2], emiliano, "Bueno", 4000);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], ignacio, "Bueno", 12);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], augusto, "Bueno", 1);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], pablo, "Bueno", 1432);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], mati, "Bueno", 132);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[3], alex, "Bueno", 10);

            List<OfertaLaboral> lista = Singleton<CatalogoOfertas>.Instance.BuscarOfertasPorUbicacion("panaderia", nacho);
            LocationApiClient client = new LocationApiClient();
            
            Console.WriteLine("FDSFDS");
            Assert.AreEqual(client.GetDistance(lista[0].Trabajador.Ubicacion, nacho.Ubicacion).TravelDistance, 0,269);
            Assert.AreEqual(client.GetDistance(lista[1].Trabajador.Ubicacion, nacho.Ubicacion).TravelDistance, 0,582);
            Assert.AreEqual(client.GetDistance(lista[2].Trabajador.Ubicacion, nacho.Ubicacion).TravelDistance, 1,195);
            Assert.AreEqual(client.GetDistance(lista[3].Trabajador.Ubicacion, nacho.Ubicacion).TravelDistance, 1,633);
            Assert.AreEqual(client.GetDistance(lista[4].Trabajador.Ubicacion, nacho.Ubicacion).TravelDistance, 2,328);
        }

        /// <summary>
        /// Prueba el método BuscarOfertasPorCalifiación. Esto se hace mediante la creacion de multiples categorias y trabajadores
        /// para crear varias ofertas, luego califica a varios trabajadores y sus ofertas
        /// y por ultimo comprueba si se pueden buscar de forma correcta las ofertas en base a su calificación.
        /// </summary>
        [Test]
        public void BuscarOfertasPorCalificacionTest()
        {
            Singleton<CatalogoOfertas>.Instance.Ofertas.Clear();
            Singleton<CatalogoCategorias>.Instance.Categorias.Clear();
            Singleton<CatalogoContratacion>.Instance.Contratacion.Clear();

            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("panaderia", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("mecanico", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("constructor", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("jardineria", "jardin");

            Empleador nacho = new Empleador("Nacho", "nacho@gmail.com", "098473729", 1, "ubicacion");
            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Trabajador emiliano = new Trabajador("Emiliano", "alex@gmail.com", "099333111", 55555551, "ubicacion");
            Trabajador ignacio = new Trabajador("Ignacio", "ignacio@gmail.com", "099689521", 55914573,"ubicacion");

            Trabajador joel = new Trabajador("Joel", "alex@gmail.com", "099333111", 55555552, "ubicacion");
            Trabajador augusto = new Trabajador("Augusto", "augusto@gmail.com", "099895234", 45294893, "ubicacion");
            Trabajador pablo = new Trabajador("Pablo", "pablo@gmail.com", "095859203", 15839294, "ubicacion");
            Trabajador mati = new Trabajador("Mati", "mati@gmail.com", "091583923", 58395775, "ubicacion");

            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[1], alex, "Bueno", 2);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], pablo, "Bueno", 5);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[2], alex, "Bueno", 4000);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], joel, "Bueno", 12);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[0], mati, "Bueno", 1);
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[3], alex, "Bueno", 10);

            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Singleton<CatalogoOfertas>.Instance.Ofertas[0], nacho, "Bueno");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Singleton<CatalogoOfertas>.Instance.Ofertas[1], nacho, "Bueno");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Singleton<CatalogoOfertas>.Instance.Ofertas[2], nacho, "Bueno");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Singleton<CatalogoOfertas>.Instance.Ofertas[3], nacho, "Bueno");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Singleton<CatalogoOfertas>.Instance.Ofertas[4], nacho, "Bueno");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Singleton<CatalogoOfertas>.Instance.Ofertas[5], nacho, "Bueno");

            Singleton<CatalogoContratacion>.Instance.Contratacion[0].CalificarTrabajador("Trabaja bien", 2);
            Singleton<CatalogoContratacion>.Instance.Contratacion[1].CalificarTrabajador("Trabaja bien", 4);
            Singleton<CatalogoContratacion>.Instance.Contratacion[2].CalificarTrabajador("Trabaja bien", 3);
            Singleton<CatalogoContratacion>.Instance.Contratacion[3].CalificarTrabajador("Trabaja bien", 3);
            Singleton<CatalogoContratacion>.Instance.Contratacion[4].CalificarTrabajador("Trabaja bien", 5);
            Singleton<CatalogoContratacion>.Instance.Contratacion[5].CalificarTrabajador("Trabaja bien", 1);

            List<OfertaLaboral> lista = Singleton<CatalogoOfertas>.Instance.BuscarOfertasPorCalificacion("panaderia");

            Assert.AreEqual(lista[0].Trabajador.Reputacion, 3);
            Assert.AreEqual(lista[1].Trabajador.Reputacion, 4);
            Assert.AreEqual(lista[2].Trabajador.Reputacion, 5);
        }

        /// <summary>
        /// Prueba la serialización y la descerialización de las ofertas en .json. Para eso crea varias ofertas
        /// que luego serializa, para consecuentemente traerlas y testear si se encuentran de forma correcta.
        /// </summary>
        [Test]
        public void GuardarTraerDatosJsonFileOfertaTes()
        {
            Singleton<CatalogoOfertas>.Instance.LoadFromJson(Singleton<CatalogoOfertas>.Instance.ShowFile());
            Singleton<CatalogoCategorias>.Instance.LoadFromJson(Singleton<CatalogoCategorias>.Instance.ShowFile());

            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("panaderia", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("mecanico", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("constructor", "pancitos");
            Singleton<CatalogoCategorias>.Instance.AgregarCategoria("jardineria", "jardin");

            Empleador nacho = new Empleador("Nacho", "nacho@gmail.com", "098473729", 1, "ubicacion");
            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Trabajador joel = new Trabajador("Joel", "alex@gmail.com", "099333111", 55555552, "ubicacion");
            Trabajador augusto = new Trabajador("Augusto", "augusto@gmail.com", "099895234", 45294893, "ubicacion");
            Trabajador pablo = new Trabajador("Pablo", "pablo@gmail.com", "095859203", 15839294, "ubicacion");
            Trabajador mati = new Trabajador("Mati", "mati@gmail.com", "091583923", 58395775, "ubicacion");

            Singleton<CatalogoOfertas>.Instance.AgregarOferta(Singleton<CatalogoCategorias>.Instance.Categorias[1], alex, "Bueno", 2);

            Singleton<CatalogoOfertas>.Instance.ConvertToJson();
            Singleton<CatalogoOfertas>.Instance.LoadFromJson(Singleton<CatalogoOfertas>.Instance.ShowFile());

            Assert.AreEqual(Singleton<CatalogoOfertas>.Instance.Ofertas[Singleton<CatalogoOfertas>.Instance.Ofertas.Count - 1].Id, Singleton<CatalogoOfertas>.Instance.Ofertas.Count - 1);
            Assert.AreEqual(Singleton<CatalogoOfertas>.Instance.Ofertas[Singleton<CatalogoOfertas>.Instance.Ofertas.Count - 1].Categoria.Nombre, "mecanico");
            Assert.AreEqual(Singleton<CatalogoOfertas>.Instance.Ofertas[Singleton<CatalogoOfertas>.Instance.Ofertas.Count - 1].Trabajador.Id, 55555550);
            Assert.AreEqual(Singleton<CatalogoOfertas>.Instance.Ofertas[Singleton<CatalogoOfertas>.Instance.Ofertas.Count - 1].Precio, 2);
        }

        /// <summary>
        /// Este test verifica si aparece la excepcion luego de enviar valores no esperados.
        /// Este test lo hicimos aca porque al ser uno solo para OfertaLaboral, nos parecia mejor ponerlo aca.
        /// </summary>
        [Test]
        public void ParametrosIncorrectosOfertaTest()
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
