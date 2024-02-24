/*using NUnit.Framework;
using Proyecto_Chatbot.TelegramBot;
using Proyecto_Chatbot;
using Telegram.Bot.Types;

namespace ProgramTests
{
    /// <summary>
    ///Clase de prueba de la clase AddressHandler brindados por la LocationApi y el repo del bot.
    /// </summary>
    public class AddressHandlerTests
    {
        // Resultado de las búsquedas de prueba; sólo se usa el atributo Found
        private class TestAddressResult : IAddressResult
        {
            public bool Found { get; set; }
            public double Latitude { get; }
            public double Longitude { get; }
        }

        // Buscador de direcciones de prueba
        private class TestAddressFinder : IAddressFinder
        {
            // Retorna true si la dirección es "true" y false en caso contrario.
            public IAddressResult GetLocation(string address)
            {
                return new TestAddressResult() { Found = address.Equals("true") };
            }
        }

        AddressHandler handler;
        Mensaje mensaje;
        TestAddressFinder finder = new TestAddressFinder();

        /// <summary>
        /// Setup para los Tests.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            handler = new AddressHandler(finder, null);
            mensaje = new Mensaje(mensaje.Id, mensaje.Text);
        }
        /// <summary>
        ///Prueba que el comando sea procesado.
        /// </summary>
        [Test]
        public void TestAddressHandled()
        {
            mensaje.Text = handler.Keywords[0];
            string response;

            IHandler result = handler.Handle(mensaje, out response);

            Assert.That(result, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(AddressHandler.AddressState.AddressPrompt));
        }
        /// <summary>
        ///Prueba que el comando sea procesado, pida una dirección que existe, y la encuentre.
        /// </summary>
        [Test]
        public void TestAddressFound()
        {
            mensaje.Text = this.handler.Keywords[0];
            string response;
            this.handler.Handle(mensaje, out response);

            mensaje.Text = "true";
            IHandler result = this.handler.Handle(mensaje, out response);

            Assert.That(result, Is.Not.Null);
            Assert.That(this.handler.State, Is.EqualTo(AddressHandler.AddressState.Start));
            Assert.That(this.handler.Data.Result.Found, Is.True);
        }
        /// <summary>
        /// Prueba que el comando sea procesado, pide una dirección que no existe, y no la encuentre.
        /// </summary>   
        [Test]
        public void TestAddressNotFound()
        {
            mensaje.Text = this.handler.Keywords[0];
            string response;
            this.handler.Handle(mensaje, out response);

            mensaje.Text = "false";
            IHandler result = this.handler.Handle(mensaje, out response);

            Assert.That(result, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(AddressHandler.AddressState.AddressPrompt));
            Assert.That(handler.Data.Result.Found, Is.False);

            mensaje.Text = "true";
            result = handler.Handle(mensaje, out response);

            Assert.That(result, Is.Not.Null);
            Assert.That(this.handler.State, Is.EqualTo(AddressHandler.AddressState.Start));
            Assert.That(this.handler.Data.Result.Found, Is.True);
        }

        /// <summary>
        /// Prueba que el comando sea cancelado.
        /// </summary> 
        [Test]
        public void TestCancel()
        {
            handler.Cancel();

            Assert.That(handler.State, Is.EqualTo(AddressHandler.AddressState.Start));
            Assert.That(handler.Data.Address, Is.EqualTo(default(string)));
            Assert.That(handler.Data.Result, Is.Null);
        }
        
        /// <summary>
        /// Prueba qué pasa cuando el comando se ejecuta sin que haya un buscador de direcciones asignado.
        /// </summary>
        [Test]
        public void TestNoFinder()
        {
            handler = new AddressHandler(null, null);

            mensaje.Text = handler.Keywords[0];
            string response;
            handler.Handle(mensaje, out response);

            mensaje.Text = string.Empty;
            IHandler result = handler.Handle(mensaje, out response);

            Assert.That(result, Is.Not.Null);
            Assert.That(handler.State, Is.EqualTo(AddressHandler.AddressState.Start));
        }
    }
}*/