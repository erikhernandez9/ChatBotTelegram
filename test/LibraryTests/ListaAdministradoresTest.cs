using NUnit.Framework;
using Proyecto_Chatbot.Usuarios;
using Proyecto_Chatbot.Catalogos;
using Proyecto_Chatbot;
using System.Collections.Generic;
using System;

namespace Tests
{
    /// <summary>
    /// Tests de la lista de administradores.
    /// </summary>
    [TestFixture]
    public class ListaAdministradoresTest
    {        
        /// <summary>
        /// Prueba la creación de un administrador.
        /// </summary>
        [Test]
        public void AgregarAdministradorTest()
        {
            Singleton<ListaAdministradores>.Instance.Administradores.Clear();

            Singleton<ListaAdministradores>.Instance.AgregarAdministrador("Juan", "juan@gmail.com", 099123345);
            Assert.AreEqual(Singleton<ListaAdministradores>.Instance.Administradores[0].Nombre, "Juan");
            Assert.AreEqual(Singleton<ListaAdministradores>.Instance.Administradores[0].Email, "juan@gmail.com");
            Assert.AreEqual(Singleton<ListaAdministradores>.Instance.Administradores[0].Id, 099123345);
        }

        /// <summary>
        /// Prueba eliminar un administrador.
        /// </summary>
        [Test]
        public void EliminarAgregarAdministradorTest()
        {
            Singleton<ListaAdministradores>.Instance.Administradores.Clear();

            Singleton<ListaAdministradores>.Instance.AgregarAdministrador("Juan", "juan@gmail.com", 099123345);
            Singleton<ListaAdministradores>.Instance.EliminarAdministrador(099123345);
            Assert.AreEqual(Singleton<ListaAdministradores>.Instance.Administradores.Count, 0);
        }

        /// <summary>
        /// Prueba el método SeleccionarAdministrador, mediante la creación del administrador, y luego la posterior selección de
        /// este dentro de la lista de administradores.
        /// </summary>
        [Test]
        public void SeleccionarAAgregarAdministradorTest()
        {
            Singleton<ListaAdministradores>.Instance.Administradores.Clear();

            Singleton<ListaAdministradores>.Instance.AgregarAdministrador("Juan", "juan@gmail.com", 099123345);
            Administrador juan = Singleton<ListaAdministradores>.Instance.SeleccionarAdministrador(099123345);
            Assert.AreEqual(juan.Nombre, "Juan");
            Assert.AreEqual(juan.Email, "juan@gmail.com");
            Assert.AreEqual(juan.Id, 099123345);
        }

        /// <summary>
        /// Prueba la serialización y la deserialización de la lista de administradores en .json. 
        /// </summary>
        [Test]
        public void GuardarTraerDatosJsonFileAdministradoresTest()
        {
            Singleton<ListaAdministradores>.Instance.LoadFromJson(Singleton<ListaAdministradores>.Instance.ShowFile());

            Singleton<ListaAdministradores>.Instance.AgregarAdministrador("Juan", "juan@gmail.com", 099123345);
            Singleton<ListaAdministradores>.Instance.ConvertToJson();
            Singleton<ListaAdministradores>.Instance.LoadFromJson(Singleton<ListaAdministradores>.Instance.ShowFile());
            Assert.AreEqual(Singleton<ListaAdministradores>.Instance.Administradores[Singleton<ListaAdministradores>.Instance.Administradores.Count - 1].Nombre, "Juan");
            Assert.AreEqual(Singleton<ListaAdministradores>.Instance.Administradores[Singleton<ListaAdministradores>.Instance.Administradores.Count - 1].Email, "juan@gmail.com");
            Assert.AreEqual(Singleton<ListaAdministradores>.Instance.Administradores[Singleton<ListaAdministradores>.Instance.Administradores.Count - 1].Id, 099123345);
        }
    }
}