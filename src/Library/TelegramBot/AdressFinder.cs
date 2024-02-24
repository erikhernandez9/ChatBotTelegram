//--------------------------------------------------------------------------------
// <copyright file="Distance.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//--------------------------------------------------------------------------------
using Proyecto_Chatbot.Locations.Client;
using Nito.AsyncEx;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Un buscador de direcciones concreto que usa una API de localización.
    /// </summary>
    public class AddressFinder : IAddressFinder
    {
        private LocationApiClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressFinder"/> class.
        /// </summary>
        /// <param name="client">El cliente de la API de localización.</param>
        public AddressFinder(LocationApiClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Determina si existe una dirección.
        /// </summary>
        /// <param name="address">La dirección a buscar.</param>
        /// <returns>Una instancia de AddressResult con el resultado de la búsqueda, que incluye si la dirección se
        /// encontró o no, y si se encontró, la latitud y la longitud de la dirección.</returns>
        public IAddressResult GetLocation(string address)
        {
            Location location = AsyncContext.Run(() => this.client.GetLocationAsync(address));
            AddressResult result = new AddressResult(location);

            return result;
        }
    }  
}