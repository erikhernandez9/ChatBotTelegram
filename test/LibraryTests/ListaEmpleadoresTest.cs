using NUnit.Framework;
using Proyecto_Chatbot.Usuarios;
using Proyecto_Chatbot.Catalogos;
using Proyecto_Chatbot;
using System.Collections.Generic;
using System;

namespace Tests
{
    /// <summary>
    /// Tests de la Lista de empleadores.
    /// </summary>
    [TestFixture]
    public class ListaEmpleadoresTest
    {        
        /// <summary>
        /// Prueba el agregar un empleador a la lista de empleadores.
        /// </summary>
        [Test]
        public void AgregarEmpleadorTest()
        {
            Singleton<ListaEmpleadores>.Instance.Empleadores.Clear();

            Singleton<ListaEmpleadores>.Instance.AgregarEmpleador("Juan", "juan@gmail.com", "553423678", 099123345, "ubicacion");
            Assert.AreEqual(Singleton<ListaEmpleadores>.Instance.Empleadores[0].Nombre, "Juan");
            Assert.AreEqual(Singleton<ListaEmpleadores>.Instance.Empleadores[0].Email, "juan@gmail.com");
            Assert.AreEqual(Singleton<ListaEmpleadores>.Instance.Empleadores[0].Telefono, "553423678");
            Assert.AreEqual(Singleton<ListaEmpleadores>.Instance.Empleadores[0].Id, 099123345);
            Assert.AreEqual(Singleton<ListaEmpleadores>.Instance.Empleadores[0].Ubicacion, "ubicacion");
        }

        /// <summary>
        /// Prueba el eliminar un empleador de la lista de empleadores.
        /// </summary>
        [Test]
        public void EliminarEmpleadorTest()
        {
            Singleton<ListaEmpleadores>.Instance.Empleadores.Clear();

            Singleton<ListaEmpleadores>.Instance.AgregarEmpleador("Juan", "juan@gmail.com", "553423678", 099123345, "ubicacion");
            Singleton<ListaEmpleadores>.Instance.EliminarEmpleador(099123345);
            Assert.AreEqual(Singleton<ListaEmpleadores>.Instance.Empleadores.Count, 0);
        }

        /// <summary>
        /// Prueba el seleccionar un empleador de la lista de empleadores a través de su id.
        /// </summary>
        [Test]
        public void SeleccionarEmpleadorTest()
        {
            Singleton<ListaEmpleadores>.Instance.Empleadores.Clear();

            Singleton<ListaEmpleadores>.Instance.AgregarEmpleador("Juan", "juan@gmail.com", "553423678", 099123345, "ubicacion");
            Empleador juan = Singleton<ListaEmpleadores>.Instance.SeleccionarEmpleador(099123345);
            Assert.AreEqual(juan.Nombre, "Juan");
            Assert.AreEqual(juan.Email, "juan@gmail.com");
            Assert.AreEqual(juan.Telefono, "553423678");
            Assert.AreEqual(juan.Id, 099123345);
            Assert.AreEqual(juan.Ubicacion, "ubicacion");
        }

        /// <summary>
        /// Prueba la serialización y la deserialización de la lista de empleadores en .json.
        /// </summary>
        [Test]
        public void GuardarTraerDatosJsonFileEmpleadoresTest()
        {
            Singleton<ListaEmpleadores>.Instance.LoadFromJson(Singleton<ListaEmpleadores>.Instance.ShowFile());

            Singleton<ListaEmpleadores>.Instance.AgregarEmpleador("Juan", "juan@gmail.com", "553423678", 099123345, "ubicacion");
            Singleton<ListaEmpleadores>.Instance.ConvertToJson();
            Singleton<ListaEmpleadores>.Instance.LoadFromJson(Singleton<ListaEmpleadores>.Instance.ShowFile());
            Assert.AreEqual(Singleton<ListaEmpleadores>.Instance.Empleadores[Singleton<ListaEmpleadores>.Instance.Empleadores.Count - 1].Nombre, "Juan");
            Assert.AreEqual(Singleton<ListaEmpleadores>.Instance.Empleadores[Singleton<ListaEmpleadores>.Instance.Empleadores.Count - 1].Email, "juan@gmail.com");
            Assert.AreEqual(Singleton<ListaEmpleadores>.Instance.Empleadores[Singleton<ListaEmpleadores>.Instance.Empleadores.Count - 1].Telefono, "553423678");
            Assert.AreEqual(Singleton<ListaEmpleadores>.Instance.Empleadores[Singleton<ListaEmpleadores>.Instance.Empleadores.Count - 1].Id, 099123345);
            Assert.AreEqual(Singleton<ListaEmpleadores>.Instance.Empleadores[Singleton<ListaEmpleadores>.Instance.Empleadores.Count - 1].Ubicacion, "ubicacion");
        }
    }
}