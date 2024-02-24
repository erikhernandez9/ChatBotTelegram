using Proyecto_Chatbot.Locations.Client;
using Nito.AsyncEx;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Una implementación concreta del resutlado de buscar una dirección. Además de las propiedades definidas en
    /// IAddressResult esta clase agrega una propiedad Location para acceder a las coordenadas de la dirección buscada.
    /// </summary>
    public class AddressResult : IAddressResult
    {
        /// <summary>
        /// Get location (ubicacion).
        /// </summary>
        public Location Location { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressResult"/> class.
        /// </summary>
        /// <param name="location">location del usuario.</param>
        public AddressResult(Location location)
        {
            this.Location = location;
        }

        /// <summary>
        /// Cuando se encuentra una ubicación, la toma y la devuelve.
        /// </summary>
        /// <return>retorna true en caso que encuentra la ubicación, y false en caso de no encontrarla.</return>
        public bool Found
        {
            get
            {
                return this.Location.Found;
            }
        }

        /// <summary>
        /// Latitud de la locacion indicada.
        /// </summary>
        /// <return>retorna la latitud.</return>
        public double Latitude
        {
            get
            {
                return this.Location.Latitude;
            }
        }

        /// <summary>
        /// Longitud de la locacion indicada.
        /// </summary>
        /// <return>retorna la longitud.</return>
        public double Longitude
        {
            get
            {
                return this.Location.Longitude;
            }
        }
    }
}