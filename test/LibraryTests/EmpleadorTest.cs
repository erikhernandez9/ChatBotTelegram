using NUnit.Framework;
using Proyecto_Chatbot.Usuarios;
using Proyecto_Chatbot.Catalogos;
using Proyecto_Chatbot;
using System.Collections.Generic;
using System;

namespace Tests
{
    /// <summary>
    /// Tests del empleador
    /// </summary>
    [TestFixture]
    public class EmpleadorTests
    {
        /// <summary>
        /// Prueba el cálculo de la reputación del empleador.
        /// </summary>
        [Test]
        public void CalcularReputacionEmpleadorTest()
        {
            Singleton<CatalogoContratacion>.Instance.Contratacion.Clear();

            Trabajador pepe = new Trabajador("Pepe", "pepe@gmail.com","022336699", 1, "ubicacion");
            Empleador juan = new Empleador("Juan", "juan@gmail.com","099123345", 553423678, "ubicacion");
            OfertaLaboral oferta1 = new OfertaLaboral(1, new Categoria("Panaderia", "prueba"), pepe, "Descripcionprueba", 100);
            OfertaLaboral oferta3 = new OfertaLaboral(2, new Categoria("AAAA", "4"), pepe, "Descripcionprueba", 100);
            OfertaLaboral oferta2 = new OfertaLaboral(3, new Categoria("bbbb", "2"), pepe, "Descripcionprueba", 100);

            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(oferta1, juan, "contratado");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(oferta2, juan, "contratado");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(oferta3, juan, "contratado");
            Singleton<CatalogoContratacion>.Instance.Contratacion[0].CalificarEmpleador("a", 4.0);
            Singleton<CatalogoContratacion>.Instance.Contratacion[1].CalificarEmpleador("b", 4.0);
            Singleton<CatalogoContratacion>.Instance.Contratacion[2].CalificarEmpleador("c", 4.0);
            
            Assert.AreEqual(4, juan.Reputacion);
        }

        /// <summary>
        /// Prueba el crear un empleador. Para esto crea el objeto empleador y comprueba si el nombre es relacionado
        /// al objeto es correcto.
        /// </summary>
        [Test]
        public void CrearEmpleador()
        {
            Empleador juan = new Empleador("Juan", "juan@gmail.com","099123345", 553423678, "ubicacion");
            Assert.AreEqual("Juan", juan.Nombre);
        }

        /// <summary>
        /// Prueba el caso de crear un empleador pero con parámetros incorrectos, y tira una excepción en caso de
        /// ser erroneos.
        /// </summary>
        [Test]
        public void CrearEmpleadorParametrosIncorrectos()
        {
            try
            {
                Empleador juan = new Empleador("", "juan@gmail.com",null, 553423678, "ubicacion");
                Assert.Throws<ExcepcionConstructor>(delegate { throw new ExcepcionConstructor(); });
            }
            catch (ExcepcionConstructor e)
            {
            }
        }
    }
}