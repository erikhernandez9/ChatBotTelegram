using NUnit.Framework;
using Proyecto_Chatbot.Usuarios;
using Proyecto_Chatbot.Catalogos;
using Proyecto_Chatbot;
using System.Collections.Generic;

namespace Tests
{
    /// <summary>
    /// TESTS de la clase CONTRATACION.
    /// 
    /// ESTOS TESTS PERTENECEN A LA USER STORY:
    /// "Como empleador, quiero poder contactar a un trabajador para que de esa forma pueda 
    /// contratar una oferta de servicios determinada." ya que se prueba que una contratación
    /// se agregue de forma erronea.
    /// 
    /// También pertenecen a las user story:
    /// "Como trabajador, quiero poder calificar a un empleador; el empleador me tiene que calificar a mi también, 
    /// si no me califica en un mes, la calificación será neutral, para que de esa forma pueda definir 
    /// la reputación de mi empleador."
    /// 
    /// "Como empleador, quiero poder calificar a un trabajador; el trabajador me tiene que calificar a mi también, 
    /// si no me califica en un mes, la calificación será neutral, para que de esa forma, pueda definir la reputaión del trabajador."
    /// </summary>
    [TestFixture]
    public class ContratacionTests
    {
        /// <summary>
        /// Prueba la acción de contratar.
        /// </summary>
        [Test]
        public void AgregarContratacionErroneaTest()
        {
            Trabajador alex = new Trabajador("Juan", "juan@gmail.com", "099123345", 553423678, "ubicacion");
            Empleador juan = null;
            OfertaLaboral oferta1 = new OfertaLaboral(1, new Categoria("Panaderia", "prueba"), alex, "Descripcionprueba", 100);
            Contratacion contratacion1 = new Contratacion(1, oferta1, juan, "contratado"); 
            Assert.IsNull(contratacion1.Descripcion);
        }

        /// <summary>
        /// Prueba que se puedan calificar empleadores.
        /// </summary>
        [Test]
        public void CalificacionEmpleadorTest()
        {
            Singleton<CatalogoContratacion>.Instance.Contratacion.Clear();
            
            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Empleador juan = new Empleador("Juan", "juan@gmail.com","099123345", 553423678, "ubicacion");
            OfertaLaboral oferta1 = new OfertaLaboral(1, new Categoria("Panaderia", "prueba"), alex, "Descripcionprueba", 100);
            Contratacion contratacion1 = new Contratacion(1, oferta1, juan, "contratado");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(oferta1, juan, "contratado");
            contratacion1.CalificarEmpleador("Muy bueno", 5);
            Assert.AreEqual(5, contratacion1.CalificacionEmpleador.Puntaje);
        }

        /// <summary>
        /// Prueba que se puedan calificar trabajadores.
        /// </summary>
        [Test]
        public void CalificacionTrabajadorTest()
        {
            Singleton<CatalogoContratacion>.Instance.Contratacion.Clear();

            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Empleador juan = new Empleador("Juan", "juan@gmail.com", "099123345", 553423678, "ubicacion");
            OfertaLaboral Oferta1 = new OfertaLaboral(1, new Categoria("Panaderia", "prueba"), alex, "Descripcionprueba", 100);
            Contratacion Contratacion1 = new Contratacion(1, Oferta1, juan, "contratado");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Oferta1, juan, "contratacion");
            Contratacion1.CalificarTrabajador("Muy bueno", 3);
            Assert.AreEqual(3, Contratacion1.CalificacionTrabajador.Puntaje);
        }

        /// <summary>
        /// Prueba hacer una calificación erronea para así tirar una excepción.
        /// </summary>
        [Test]
        public void CalificacionIncorrecta1Test()
        {
            Singleton<CatalogoContratacion>.Instance.Contratacion.Clear();

            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Empleador juan = new Empleador("Juan", "juan@gmail.com", "099123345", 553423678, "ubicacion");
            OfertaLaboral Oferta1 = new OfertaLaboral(1, new Categoria("Panaderia", "prueba"), alex, "Descripcionprueba", 100);
            Contratacion Contratacion1 = new Contratacion(1, Oferta1, juan, "contratado");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Oferta1, juan, "contratacion");
            try
            {
                Contratacion1.CalificarTrabajador("Muy bueno", 7);
                Assert.Throws<ExcepcionConstructor>(delegate { throw new ExcepcionConstructor(); });
            } catch (ExcepcionConstructor e)
            {
            }
        }

        /// <summary>
        /// Prueba hacer una califiación negativa, y si esto tira una excepción como es esperado.
        /// </summary>
        [Test]
        public void CalificacionIncorrecta2Test()
        {
            Singleton<CatalogoContratacion>.Instance.Contratacion.Clear();

            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Empleador juan = new Empleador("Juan", "juan@gmail.com", "099123345", 553423678, "ubicacion");
            OfertaLaboral Oferta1 = new OfertaLaboral(1, new Categoria("Panaderia", "prueba"), alex, "Descripcionprueba", 100);
            Contratacion Contratacion1 = new Contratacion(1, Oferta1, juan, "contratado");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Oferta1, juan, "contratacion");
            try
            {
                Contratacion1.CalificarTrabajador("Muy bueno", -1);
                Assert.Throws<ExcepcionConstructor>(delegate { throw new ExcepcionConstructor(); });
            } catch (ExcepcionConstructor e)
            {
            }
        }
        /// <summary>
        /// Prueba que se puedan despedir trabajadores.
        /// </summary>
        [Test]
        public void DesemplearTest()
        {
            Singleton<CatalogoContratacion>.Instance.Contratacion.Clear();

            Trabajador alex = new Trabajador("Alex", "alex@gmail.com", "099333111", 55555550, "ubicacion");
            Empleador juan = new Empleador("Juan", "juan@gmail.com", "099123345", 553423678, "ubicacion");
            OfertaLaboral Oferta1 = new OfertaLaboral(1, new Categoria("Panaderia", "prueba"), alex, "Descripcionprueba", 100);
            Contratacion Contratacion1 = new Contratacion(1, Oferta1, juan, "contratado");
            Singleton<CatalogoContratacion>.Instance.AgregarContratacion(Oferta1, juan, "despedido");
            Contratacion1.Desemplear();
            Assert.AreEqual(false, Contratacion1.Estado);
        }
    }
}