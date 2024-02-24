using NUnit.Framework;
using Proyecto_Chatbot.Usuarios;
using Proyecto_Chatbot.Catalogos;
using Proyecto_Chatbot;
using System.Collections.Generic;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Tests
{
    /// <summary>
    /// Tests del catálogo de contrataciones.
    /// </summary>
    [TestFixture]
    public class CatalogoContratacionesTests
    {
        /// <summary>
        /// Prueba que se creen contrataciones.
        /// </summary>
        [Test]
        public void AgregarContratacionTest()
        {
            Singleton<CatalogoContratacion>.Instance.Contratacion.Clear();
            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Empleador juan = new Empleador("Juan", "juan@gmail.com","099123345", 553423678, "ubicacion");
            OfertaLaboral Oferta1 = new OfertaLaboral(1, new Categoria("Panaderia", "prueba"), alex, "Descripcionprueba", 100);
            Contratacion Contratacion1 = new Contratacion(1, Oferta1, juan, "contratado");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Oferta1, juan, "panes");
            Assert.IsNotEmpty(Singleton<CatalogoContratacion>.Instance.Contratacion);
        }
        /// <summary>
        /// Prueba que se eliminen contrataciones.
        /// </summary>
        [Test]
        public void EliminarContratacionTest()
        {
            Singleton<CatalogoContratacion>.Instance.Contratacion.Clear();
            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Empleador juan = new Empleador("Juan", "juan@gmail.com","099123345", 553423678, "ubicacion");
            OfertaLaboral ofertaPrueba = new OfertaLaboral(4, new Categoria("Panaderia", "prueba"), alex, "Descripcionprueba", 100);
            Contratacion Contratacion1 = new Contratacion(1, ofertaPrueba, juan, "contratado");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(ofertaPrueba, juan, "panes");
            Singleton<CatalogoContratacion>.Instance.EliminarContratacion(0);
            List<Contratacion> prueba = new List<Contratacion>();
            Assert.AreEqual(prueba, Singleton<CatalogoContratacion>.Instance.Contratacion);
        }
        /// <summary>
        /// Prueba que la búsqueda de contrataciones.
        /// </summary>
        [Test]
        public void BuscarContratacionTest()
        {
            Singleton<CatalogoContratacion>.Instance.Contratacion.Clear();
            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Empleador juan = new Empleador("Juan", "juan@gmail.com", "553423678", 099123345, "ubicacion");
            OfertaLaboral Oferta1 = new OfertaLaboral(1, new Categoria("Panaderia", "prueba"), alex, "Descripcionprueba", 100);
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Oferta1, juan, "contratado");
            Contratacion guardado = Singleton<CatalogoContratacion>.Instance.BuscarContratacion(3);
            Assert.AreEqual(guardado, null);
        }

        /// <summary>
        /// Prueba la serialización y la descerialización de las contrataciones en .json. Para eso creamos varias contrataciones
        /// que luego serializamos, para consecuentemente traerlas y testear si se encuentran de forma correcta.
        /// </summary>
        [Test]
        public void GuardarTraerDatosJsonFileContratacionesTest()
        {
            Singleton<CatalogoCategorias>.Instance.LoadFromJson(Singleton<CatalogoCategorias>.Instance.ShowFile());
            Singleton<ListaTrabajadores>.Instance.LoadFromJson(Singleton<ListaTrabajadores>.Instance.ShowFile());
            Singleton<ListaEmpleadores>.Instance.LoadFromJson(Singleton<ListaEmpleadores>.Instance.ShowFile());
            Singleton<CatalogoContratacion>.Instance.LoadFromJson(Singleton<CatalogoContratacion>.Instance.ShowFile());
            
            Singleton<ListaEmpleadores>.Instance.AgregarEmpleador("Juan", "juan@gmail.com", "553423678", 099123344, "ubicacion");
            Singleton<ListaTrabajadores>.Instance.AgregarTrabajador("Juan", "juan@gmail.com", "553423678", 099123345, "ubicacion");
            Empleador juan = new Empleador("Juan", "juan@gmail.com", "553423678", 599123345, "ubicacion");
            
            Singleton<CatalogoOfertas>.Instance.AgregarOferta(new Categoria("Panaderia", "prueba"), Singleton<ListaTrabajadores>.Instance.SeleccionarTrabajador(099123345), "Descripcionprueba", 100);
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Singleton<CatalogoOfertas>.Instance.BuscarOferta(0), Singleton<ListaEmpleadores>.Instance.SeleccionarEmpleador(099123344), "contratado");
            Singleton<CatalogoContratacion>.Instance.ConvertToJson();
            Singleton<CatalogoContratacion>.Instance.LoadFromJson(Singleton<CatalogoContratacion>.Instance.ShowFile());

            Assert.AreEqual(Singleton<CatalogoContratacion>.Instance.Contratacion[Singleton<CatalogoContratacion>.Instance.Contratacion.Count - 1].OfertaLaboral.Id, 0);
            Assert.AreEqual(Singleton<CatalogoContratacion>.Instance.Contratacion[Singleton<CatalogoContratacion>.Instance.Contratacion.Count - 1].Empleador.Id, 099123344);
            Assert.AreEqual(Singleton<CatalogoContratacion>.Instance.Contratacion[Singleton<CatalogoContratacion>.Instance.Contratacion.Count - 1].Descripcion, "contratado");
        }
    }
}