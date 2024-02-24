using NUnit.Framework;
using Proyecto_Chatbot.Usuarios;
using Proyecto_Chatbot.Catalogos;
using Proyecto_Chatbot;
using System.Collections.Generic;
using System;

namespace Tests
{
    /// <summary>
    /// Tests sobre el trabajador.
    /// </summary>
    [TestFixture]
    public class TrabajadorTests
    {
        /// <summary>
        /// Prueba el cálculo de la reputación.
        /// </summary>
        [Test]
        public void CalcularReputacionTest()
        {
            Singleton<CatalogoContratacion>.Instance.Contratacion.Clear();

            Trabajador pepe = new Trabajador("Pepee", "pepe@gmail.com", "022336699", 1, "ubicacion");
            Empleador juan = new Empleador("Juan", "juan@gmail.com","099123345", 553423678, "ubicacion");
            OfertaLaboral Oferta1 = new OfertaLaboral(1, new Categoria("Panaderia", "prueba"), pepe, "Descripcionprueba", 100);
            OfertaLaboral Oferta3 = new OfertaLaboral(2, new Categoria("AAAA", "4"), pepe, "Descripcionprueba", 100);
            OfertaLaboral Oferta2 = new OfertaLaboral(3, new Categoria("bbbb", "2"), pepe, "Descripcionprueba", 100);

            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Oferta1, juan, "contratacion");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Oferta2, juan, "Contr");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Oferta3, juan, "contr");
            Singleton<CatalogoContratacion>.Instance.Contratacion[0].CalificarTrabajador("a", 4.0);
            Singleton<CatalogoContratacion>.Instance.Contratacion[1].CalificarTrabajador("b", 4.0);
            Singleton<CatalogoContratacion>.Instance.Contratacion[2].CalificarTrabajador("c", 4.0);
            Console.WriteLine($"Nombre {Singleton<CatalogoContratacion>.Instance.Contratacion[0].OfertaLaboral.Trabajador.Nombre}");
            Assert.AreEqual(4, pepe.Reputacion);
        }

        /// <summary>
        /// Prueba el crear un objeto trabajador.
        /// </summary>
        [Test]
        public void CrearTrabajador()
        {
            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Assert.AreEqual("Alex", alex.Nombre);
        }

        /// <summary>
        /// Prueba el crear un trabajador con parámetros incorrectos para comprobar si salta la excepción.
        /// </summary>
        [Test]
        public void CrearTrabajadorParametrosIncorrectos()
        {
            try
            {
                Trabajador alex = new Trabajador("", "alex@gmail.com", "099333111", 55555550, "ubicacion");
                Assert.Throws<ExcepcionConstructor>(delegate { throw new ExcepcionConstructor(); });
            }
            catch (ExcepcionConstructor e)
            {
            }
        }
    }
}