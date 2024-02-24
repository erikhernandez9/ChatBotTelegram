using NUnit.Framework;
using Proyecto_Chatbot.Usuarios;
using Proyecto_Chatbot.Catalogos;
using Proyecto_Chatbot;
using System.Collections.Generic;
using System;

namespace Tests
{
    /// <summary>
    /// Tests de la lista de trabajadores.
    /// </summary>
    [TestFixture]
    public class ListaTrabajadoresTest
    {
        /// <summary>
        /// Prueba la búsqueda de categorías.
        /// </summary>
        [Test]
        public void AgregarTrabajadorTest()
        {
            Singleton<ListaTrabajadores>.Instance.Trabajadores.Clear();

            Singleton<ListaTrabajadores>.Instance.AgregarTrabajador("Juan", "juan@gmail.com", "553423678", 099123345, "ubicacion");
            Assert.AreEqual(Singleton<ListaTrabajadores>.Instance.Trabajadores[0].Nombre, "Juan");
            Assert.AreEqual(Singleton<ListaTrabajadores>.Instance.Trabajadores[0].Email, "juan@gmail.com");
            Assert.AreEqual(Singleton<ListaTrabajadores>.Instance.Trabajadores[0].Telefono, "553423678");
            Assert.AreEqual(Singleton<ListaTrabajadores>.Instance.Trabajadores[0].Id, 099123345);
            Assert.AreEqual(Singleton<ListaTrabajadores>.Instance.Trabajadores[0].Ubicacion, "ubicacion");
        }

        /// <summary>
        /// Prueba el eliminar un trabajador de la lista de empleadores.
        /// </summary>
        [Test]
        public void EliminarTrabajadorTest()
        {
            Singleton<ListaTrabajadores>.Instance.Trabajadores.Clear();

            Singleton<ListaTrabajadores>.Instance.AgregarTrabajador("Juan", "juan@gmail.com", "553423678", 099123345, "ubicacion");
            Singleton<ListaTrabajadores>.Instance.EliminarTrabajador(099123345);
            Assert.AreEqual(Singleton<ListaTrabajadores>.Instance.Trabajadores.Count, 0);
        }

        /// <summary>
        /// Prueba el seleccionar un trabajador de la lista de empleadores a través de su id.
        /// </summary>
        [Test]
        public void SeleccionarTrabajadorTest()
        {
            Singleton<ListaTrabajadores>.Instance.Trabajadores.Clear();

            Singleton<ListaTrabajadores>.Instance.AgregarTrabajador("Juan", "juan@gmail.com", "553423678", 099123345, "ubicacion");
            Trabajador juan = Singleton<ListaTrabajadores>.Instance.SeleccionarTrabajador(099123345);
            Assert.AreEqual(juan.Nombre, "Juan");
            Assert.AreEqual(juan.Email, "juan@gmail.com");
            Assert.AreEqual(juan.Telefono, "553423678");
            Assert.AreEqual(juan.Id, 099123345);
            Assert.AreEqual(juan.Ubicacion, "ubicacion");
        }

        /// <summary>
        /// Prueba la serialización y la deserialización de la lista de trabajadores en .json.
        /// </summary>
        [Test]
        public void GuardarTraerDatosJsonFileTrabajadoresTest()
        {
            Singleton<ListaTrabajadores>.Instance.LoadFromJson(Singleton<ListaTrabajadores>.Instance.ShowFile());
            Singleton<ListaTrabajadores>.Instance.AgregarTrabajador("Juan", "juan@gmail.com", "553423678", 099123345, "ubicacion");
            Singleton<ListaTrabajadores>.Instance.ConvertToJson();
            Singleton<ListaTrabajadores>.Instance.LoadFromJson(Singleton<ListaTrabajadores>.Instance.ShowFile());
            Assert.AreEqual(Singleton<ListaTrabajadores>.Instance.Trabajadores[Singleton<ListaTrabajadores>.Instance.Trabajadores.Count - 1].Nombre, "Juan");
            Assert.AreEqual(Singleton<ListaTrabajadores>.Instance.Trabajadores[Singleton<ListaTrabajadores>.Instance.Trabajadores.Count - 1].Email, "juan@gmail.com");
            Assert.AreEqual(Singleton<ListaTrabajadores>.Instance.Trabajadores[Singleton<ListaTrabajadores>.Instance.Trabajadores.Count - 1].Telefono, "553423678");
            Assert.AreEqual(Singleton<ListaTrabajadores>.Instance.Trabajadores[Singleton<ListaTrabajadores>.Instance.Trabajadores.Count - 1].Id, 099123345);
            Assert.AreEqual(Singleton<ListaTrabajadores>.Instance.Trabajadores[Singleton<ListaTrabajadores>.Instance.Trabajadores.Count - 1].Ubicacion, "ubicacion");
        }
    }
}