using NUnit.Framework;
using Proyecto_Chatbot;
using Proyecto_Chatbot.Usuarios;
using Proyecto_Chatbot.Catalogos;
using Proyecto_Chatbot.TelegramBot;
using System;
namespace ProgramTests;

    /// <summary>
    /// ESTOS TEST PERTENECEN A LA USER STORY:
    /// "Como administrador, quiero poder indicar categorías sobre
    /// las cuales se realizarán las ofertas de servicios para que de esa forma,
    /// los trabajadoras puedan clasificarlos." Se decició utilizar el patrón Creator, estas tareas
    /// de creación y eliminación de categorías se le asignaron a la clase "Catálogo Categorías".
    /// </summary>
    
    [TestFixture]
    public class HandlersTests
    {
        /// <summary>
        /// User story.
        /// Prueba que el administrador cree categorías utilizando el bot, para ello, se llama al catálogo de categorías y se utiliza el método de agregar categoría
        /// que pide como parametro en nombre y la descripción de la categoría, se fija si no existe y, de ser así, lo agrega a la
        /// lista de categorías y la serializa.
        /// </summary>
        [Test]
        public void CrearCategoriaHandlerTest()
        {
            Singleton<ListaAdministradores>.Instance.AgregarAdministrador("Coscu", "coscu@gmail.com", 5601698880);
            IHandler handler = new CrearCategoriaHandler(null);
            Mensaje mensaje = new Mensaje(5601698880, "Crear categoria");
            string response;
            
            IHandler result = handler.Handle(mensaje, out response);
            Assert.AreEqual(response, "Ingrese el nombre de la categoría");
            handler.Handle(new Mensaje(5601698880, "Tecnico"), out response);
            Assert.AreEqual(response, "Ingrese la descripcion de la categoría");
            handler.Handle(new Mensaje(5601698880, "Se realizan reparaciones"), out response);
            
            Assert.AreEqual(response, "La categoria fue creada");
            Assert.AreEqual(Singleton<CatalogoCategorias>.Instance.SeleccionarCategorias("Tecnico").Nombre, "Tecnico");
        }
        /// <summary>
        /// User story.
        /// Prueba que se creen empleadores utilizando el bot, para ello, se llama a la lista de empleadores y se utiliza el método de agregar empleador
        /// que pide como parametro el nombre, el email, el telefono y la direccion y lo agrega a la
        /// lista de empleadores y la serializa.
        /// </summary>
        [Test]
        public void AgregarEmpleadorHandlerTest()
        {
            IHandler handler = new AgregarEmpleadorHandler(null);
            Mensaje mensaje = new Mensaje(4324324, "Quiero registrarme como empleador");
            string response;
            
            IHandler result = handler.Handle(mensaje, out response);
            Assert.AreEqual(response, "Bueno.. Ingrese tu nombre");
            handler.Handle(new Mensaje(4324324, "Fernando"), out response);
            Assert.AreEqual(response, "Ingrese tu email");
            handler.Handle(new Mensaje(4324324, "NotaS@gmail.com"), out response);
            Assert.AreEqual(response, "Ingrese tu numero de telefono");
            handler.Handle(new Mensaje(4324324, "099233123"), out response);
            Assert.AreEqual(response, "Ingrese tu dirección");
            handler.Handle(new Mensaje(4324324, "tres cruces"), out response);
            Assert.AreEqual(response, "Genial! Te has registrado como empleador");

            Assert.AreEqual(Singleton<ListaEmpleadores>.Instance.SeleccionarEmpleador(4324324).Nombre, "Fernando");
        }
        /// <summary>
        /// User story.
        /// Prueba que se creen trabajadores utilizando el bot, para ello, se llama a la lista de trabajadores y se utiliza el método de agregar trabajador
        /// que pide como parametro el nombre, el email, el telefono y la direccion y lo agrega a la
        /// lista de trabajadores y la serializa.
        /// </summary>
        [Test]
        public void AgregarTrabajadorHandlerTest()
        {
            Singleton<ListaAdministradores>.Instance.AgregarAdministrador("Coscu", "coscu@gmail.com", 5601698880);
            IHandler handler = new AgregarTrabajadorHandler(null);
            Mensaje mensaje = new Mensaje(432432, "Quiero registrarme como trabajador");
            string response;
            
            IHandler result = handler.Handle(mensaje, out response);
            Assert.AreEqual(response, "Bueno.. Ingrese tu nombre");
            handler.Handle(new Mensaje(432432, "Martin"), out response);
            Assert.AreEqual(response, "Ingrese tu email");
            handler.Handle(new Mensaje(432432, "proyectoS@gmail.com"), out response);
            Assert.AreEqual(response, "Ingrese tu numero de telefono");
            handler.Handle(new Mensaje(432432, "099432643"), out response);
            Assert.AreEqual(response, "Ingrese tu dirección");
            handler.Handle(new Mensaje(432432, "montevideo shopping"), out response);
            Assert.AreEqual(response, "Genial! Te has registrado como trabajador");

            Assert.AreEqual(Singleton<ListaTrabajadores>.Instance.SeleccionarTrabajador(432432).Nombre, "Martin");
        }
        /// <summary>
        /// User story.
        /// Prueba que un trabajador pueda calificar a un empleador utilizando el bot, para ello, se selecciona la contratacion 
        /// en la que desea calificar al empleador, y para cumplir con dicha tarea se utiliza el método calificar empleador, 
        /// que solicita el puntaje y una descripcion, y por último te guarda un promedio con las calificaciones que ya tiene (en su reputacion)
        /// </summary>
        [Test]
        public void CalificarEmpleadorHandlerTest()
        {
            Singleton<ListaTrabajadores>.Instance.LoadFromJson(Singleton<ListaTrabajadores>.Instance.ShowFile());
            Singleton<CatalogoContratacion>.Instance.LoadFromJson(Singleton<CatalogoContratacion>.Instance.ShowFile());
            IHandler handler = new CalificarEmpleadorHandler(null);
            Mensaje mensaje = new Mensaje(5601698880, "Calificar empleador");
            string response;
            
            IHandler result = handler.Handle(mensaje, out response); //No controlamos la respuesta porque puede ser extensa (lista todas las contrataciones que estan vinculadas con el usuario)
            Console.WriteLine(response);
            handler.Handle(new Mensaje(5601698880, "1"), out response);
            Console.WriteLine(response);
            Assert.AreEqual(response, "Ingrese el puntaje (del 0 al 5)");
            handler.Handle(new Mensaje(5601698880, "4"), out response);
            Assert.AreEqual(response, "Escribi la descripcion");
            handler.Handle(new Mensaje(5601698880, "Buen trabajo"), out response);
            Contratacion contratacion = Singleton<CatalogoContratacion>.Instance.FiltrarContratacionesPorTrabajador(5601698880)[0];
            Assert.AreEqual(response, $"Muy bien, Calificaste a {contratacion.Empleador.Nombre}" );

            Assert.AreEqual(Singleton<CatalogoContratacion>.Instance.FiltrarContratacionesPorTrabajador(5601698880)[0].CalificacionEmpleador.Puntaje, 4);
        }
        /// <summary>
        /// User story.
        /// Prueba que un empleador pueda calificar a un trabajador utilizando el bot, para ello, se selecciona la contratacion 
        /// en la que desea calificar al trabajador, y para cumplir con dicha tarea se utiliza el método calificar trabajador, 
        /// que solicita el puntaje y una descripcion, y por último te guarda un promedio con las calificaciones que ya tiene (en su reputacion)
        /// </summary>
        [Test]
        public void CalificarTrabajadorHandlerTest()
        {
            Singleton<ListaEmpleadores>.Instance.LoadFromJson(Singleton<ListaEmpleadores>.Instance.ShowFile());
            Singleton<CatalogoContratacion>.Instance.LoadFromJson(Singleton<CatalogoContratacion>.Instance.ShowFile());

            IHandler handler = new CalificarTrabajadorHandler(null);
            Mensaje mensaje = new Mensaje(5601698880, "Calificar trabajador");
            string response;
            
            IHandler result = handler.Handle(mensaje, out response); //No controlamos la respuesta porque puede ser extensa (lista todas las contrataciones que estan vinculadas con el usuario)
            handler.Handle(new Mensaje(5601698880, "1"), out response);
            Assert.AreEqual(response, "Ingrese el puntaje (del 0 al 5)");
            handler.Handle(new Mensaje(5601698880, "3"), out response);
            Assert.AreEqual(response, "Escribi la descripcion");
            handler.Handle(new Mensaje(5601698880, "Buen trabajo"), out response);
            Contratacion contratacion = Singleton<CatalogoContratacion>.Instance.FiltrarContratacionesPorEmpleador(5601698880)[0];
            Assert.AreEqual(response, $"Muy bien, Calificaste a {contratacion.OfertaLaboral.Trabajador.Nombre}" );

            Assert.AreEqual(Singleton<CatalogoContratacion>.Instance.FiltrarContratacionesPorEmpleador(5601698880)[0].CalificacionTrabajador.Puntaje, 3);
        }
        /// <summary>
        /// User story.
        /// Prueba que se creen ofertas utilizando el bot, para ello, se llama a la catalogo de ofertas y se utiliza el método de crear oferta
        /// que pide como parametro un objeto categoria, un objeto trabajador, una descripcion y un precio y lo agrega a la
        /// lista de ofertas y la serializa.
        /// </summary>
        [Test]
        public void CrearOfertaLaboralHandler()
        {
            Singleton<ListaTrabajadores>.Instance.AgregarTrabajador("Coscu", "coscu@gmail.com", "099887766", 5601698880, "Carrasco");
            Singleton<CatalogoCategorias>.Instance.LoadFromJson(Singleton<CatalogoCategorias>.Instance.ShowFile());
            IHandler handler = new CrearOfertaLaboralHandler(null);
            Mensaje mensaje = new Mensaje(5601698880, "Crear oferta laboral");
            string response;
            
            IHandler result = handler.Handle(mensaje, out response); //No controlamos la respuesta porque puede ser extensa (lista todas las contrataciones que estan vinculadas con el usuario)
            handler.Handle(new Mensaje(5601698880, "Jardineria"), out response);
            Assert.AreEqual(response, "Ingrese precio de la oferta en pesos");
            handler.Handle(new Mensaje(5601698880, "800"), out response);
            Assert.AreEqual(response, "Ingrese la descripción para la oferta");
            handler.Handle(new Mensaje(5601698880, "flores"), out response);
            Assert.AreEqual(response, "La oferta fue creada con éxito");

            Assert.AreEqual(Singleton<CatalogoOfertas>.Instance.BuscarOferta(Singleton<CatalogoOfertas>.Instance.Ofertas.Count - 1).Descripcion, "flores");
        }
        /// <summary>
        /// User story.
        /// Prueba que se creen contrataciones utilizando el bot, para ello, se llama a la catalogo de contrataciones y se utiliza el método de crear contratacion
        /// que pide como parametro un objeto oferta laboral, un objeto empleador y una descripcion y lo agrega a la
        /// lista de contrataciones y la serializa. Aclaración: a la hor de seleccionar la oferta laboral, te da la opcion de ordenarlas por: calificacion, ubicacion, precio, categoria.
        /// </summary>
        [Test]
        public void CrearContratacionHandler()
        {
            Singleton<ListaEmpleadores>.Instance.AgregarEmpleador("Coscu", "coscu@gmail.com", "099887766", 5601698880, "Carrasco");
            Singleton<CatalogoContratacion>.Instance.LoadFromJson(Singleton<CatalogoContratacion>.Instance.ShowFile());
            IHandler handler = new CrearContratacionHandler(null);
            Mensaje mensaje = new Mensaje(5601698880, "Crear una contratacion");
            string response;
            
            IHandler result = handler.Handle(mensaje, out response); //No controlamos la respuesta porque puede ser extensa (lista todas las contrataciones que estan vinculadas con el usuario)

            handler.Handle(new Mensaje(5601698880, "Jardineria"), out response);
            Assert.AreEqual(response, "Indique cómo quiere ordenar las ofertas:\n1. Ordenar por calificacion\n2. Ordenar por precio\n3. Ordenar por ubicación\n4. Ordenar por categoria\nPor favor, indique el número correspondiente de la opción deseada");
            handler.Handle(new Mensaje(5601698880, "1"), out response);
            handler.Handle(new Mensaje(5601698880, "1"), out response);
            Assert.AreEqual(response, "Ingrese la descripcion de la contratacion");
            handler.Handle(new Mensaje(5601698880, "trabaja dale"), out response);

            Assert.AreEqual(response, "La contratacion fue creada con éxito");
            Assert.AreEqual(Singleton<CatalogoContratacion>.Instance.BuscarContratacion(Singleton<CatalogoContratacion>.Instance.Contratacion.Count - 1).Descripcion, "trabaja dale");//Compruebo la descripción ya que se trata de algo que se puede ver que se creó
        }
        /// <summary>
        /// Prueba que se eliminen categorias utilizando el bot, para ello, se llama al catalogo de categorias y se utiliza el método de eliminar categoria
        /// que pide como parametro el nombre de una categoria, lo selecciona de la lista de categorias, y , de existir, lo elimina y serializa la lista.
        /// </summary>
        [Test]
        public void EliminarCategoriaHandler()
        {
            Singleton<ListaAdministradores>.Instance.AgregarAdministrador("Coscu", "coscu@gmail.com", 432434);
            Singleton<CatalogoCategorias>.Instance.LoadFromJson(Singleton<CatalogoCategorias>.Instance.ShowFile());
            IHandler handler = new EliminarCategoriaHandler(null);
            Mensaje mensaje = new Mensaje(432434, "Eliminar una categoria");
            string response;
            
            IHandler result = handler.Handle(mensaje, out response); //No controlamos la respuesta porque puede ser extensa (lista todas las contrataciones que estan vinculadas con el usuario)
            Assert.AreEqual(response, "Ingrese el nombre de la categoría");
            handler.Handle(new Mensaje(432434, "clouse"), out response);
            Assert.AreEqual(response, "La categoria fue eliminada");

            Assert.IsNull(Singleton<CatalogoCategorias>.Instance.SeleccionarCategorias("clouse"));//Compruebo la descripción ya que se trata de algo que se puede ver que se creó
        }
        /// <summary>
        /// Prueba que se eliminen empleadores utilizando el bot, para ello, se llama al lista de empleadores y se utiliza el método de eliminar empleador
        /// que pide como parametro el id del empleador, lo selecciona de la lista de empleadores, y , de existir, lo elimina y serializa la lista.
        /// </summary>
        [Test]
        public void EliminarEmpleadorHandler()
        {
            Singleton<ListaEmpleadores>.Instance.LoadFromJson(Singleton<ListaEmpleadores>.Instance.ShowFile());
            Singleton<ListaAdministradores>.Instance.AgregarAdministrador("Coscu", "coscu@gmail.com", 5601698880);
            Singleton<ListaEmpleadores>.Instance.AgregarEmpleador("Juan", "juan@gmail.com", "553423678", 77323456, "ubicacion");
            IHandler handler = new EliminarEmpleadorHandler(null);
            Mensaje mensaje = new Mensaje(5601698880, "Eliminar empleador");
            string response;
            
            IHandler result = handler.Handle(mensaje, out response); 
            Console.WriteLine(response);
            Assert.AreEqual(response, "Ingrese el id del empleador");
            handler.Handle(new Mensaje(5601698880, "77323456"), out response);
            //Console.WriteLine(empleador.Nombre);
            Console.WriteLine(response);
            Assert.AreEqual(response, "El empleador fue eliminado");
            Empleador empleador = Singleton<ListaEmpleadores>.Instance.SeleccionarEmpleador(77323456);
            Console.WriteLine(empleador.Nombre);
            Assert.IsNull(empleador);//Compruebo la descripción ya que se trata de algo que se puede ver que se creó
        }/// <summary>
        /// Prueba que se eliminen ofertas utilizando el bot, para ello, se llama al catalogo de ofertas y se utiliza el método de eliminar oferta
        /// que pide como parametro el id, lo selecciona de la lista de ofertas, y , de existir, lo elimina y serializa la lista.
        /// </summary>
        [Test]
        public void EliminarOfertaHandler()
        {
            Singleton<ListaAdministradores>.Instance.AgregarAdministrador("Coscu", "coscu@gmail.com", 432434);
            Singleton<CatalogoOfertas>.Instance.LoadFromJson(Singleton<CatalogoOfertas>.Instance.ShowFile());
            IHandler handler = new EliminarOfertaHandler(null);
            Mensaje mensaje = new Mensaje(432434, "Eliminar oferta");
            string response;
            
            IHandler result = handler.Handle(mensaje, out response); //No controlamos la respuesta porque puede ser extensa (lista todas las contrataciones que estan vinculadas con el usuario)
            Assert.AreEqual(response, "Ingrese el ID de la oferta");
            handler.Handle(new Mensaje(432434, "1"), out response);
            Assert.AreEqual(response, "La oferta fue eliminada");

            Assert.IsNull(Singleton<CatalogoOfertas>.Instance.BuscarOferta(1));//Compruebo la descripción ya que se trata de algo que se puede ver que se creó
        }
        /// <summary>
        /// Prueba que se eliminen trabajadores utilizando el bot, para ello, se llama al lista de trabajadores y se utiliza el método de eliminar trabajador
        /// que pide como parametro el id del trabajador, lo selecciona de la lista de trabajadores, y , de existir, lo elimina y serializa la lista.
        /// </summary>
        [Test]
        public void EliminarTrabajadorHandler()
        {
            Singleton<ListaTrabajadores>.Instance.LoadFromJson(Singleton<ListaTrabajadores>.Instance.ShowFile());
            Singleton<ListaAdministradores>.Instance.AgregarAdministrador("Coscu", "coscu@gmail.com", 5601698880);
            Singleton<ListaTrabajadores>.Instance.AgregarTrabajador("Juan", "juan@gmail.com", "553423678", 77323456, "ubicacion");
            IHandler handler = new EliminarTrabajadorHandler(null);
            Mensaje mensaje = new Mensaje(5601698880, "Eliminar trabajador");
            string response;
            
            IHandler result = handler.Handle(mensaje, out response); 
            Console.WriteLine(response);
            Assert.AreEqual(response, "Ingrese el id del trabajador");
            handler.Handle(new Mensaje(5601698880, "77323456"), out response);
            Console.WriteLine(response);
            Assert.AreEqual(response, "El trabajador fue eliminado");
            Trabajador trabajador = Singleton<ListaTrabajadores>.Instance.SeleccionarTrabajador(77323456);
            Console.WriteLine(trabajador.Nombre);
            Assert.IsNull(trabajador);//Compruebo la descripción ya que se trata de algo que se puede ver que se creó
        }
    }