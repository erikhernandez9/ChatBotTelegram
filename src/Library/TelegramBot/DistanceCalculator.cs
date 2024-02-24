using Proyecto_Chatbot.Locations.Client;
using Nito.AsyncEx;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Un calculador de distancias concreto que utiliza una API de localización para calcular la distancia entre dos
    /// direcciones.
    /// </summary>
    public class DistanceCalculator : IDistanceCalculator
    {
        private LocationApiClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceCalculator"/> class.
        /// </summary>
        /// <param name="client">El cliente de la API de localización.</param>
        public DistanceCalculator(LocationApiClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Método para calcular la distancia entre dos ubicaciones.
        /// </summary>
        /// <return>Retorna IDistanceResult.</return>
        /// <param name="fromAddress">primera ubicacion.</param>
        /// <param name="toAddress">segunda ubicacion.</param>
        public IDistanceResult CalculateDistance(string fromAddress, string toAddress)
        {
            Location fromLocation = AsyncContext. Run(() => this.client.GetLocationAsync(fromAddress));
            Location toLocation = AsyncContext. Run(() => this.client.GetLocationAsync(toAddress));
            Distance distance = AsyncContext. Run(() => this.client.GetDistanceAsync(fromLocation, toLocation));

            DistanceResult result = new DistanceResult(fromLocation, toLocation, distance.TravelDistance, distance.TravelDuration);

            return result;
        }
    }

    /// <summary>
    /// Una implementación concreta del resutlado de calcular distancias. Además de las propiedades definidas en
    /// IDistanceResult esta clase agrega propiedades para acceder a las coordenadas de las direcciones buscadas.
    /// </summary>
    public class DistanceResult : IDistanceResult
    {
        private Location from;
        private Location to;
        private double distance;
        private double time;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceResult"/> class.
        /// Inicializa una nueva instancia de DistanceResult a partir de dos coordenadas, la distancia y el tiempo
        /// entre ellas.
        /// </summary>
        /// <param name="from">Las coordenadas de origen.</param>
        /// <param name="to">Las coordenadas de destino.</param>
        /// <param name="distance">La distancia entre las coordenadas.</param>
        /// <param name="time">El tiempo que se demora en llegar del origen al destino.</param>
        public DistanceResult(Location from, Location to, double distance, double time)
        {
            this.from = from;
            this.to = to;
            this.distance = distance;
            this.time = time;
        }
        /// <summary>
        /// Si la ubicación inicial existe devuelve que la encontró.
        /// </summary>
        public bool FromExists
        {
            get
            {
                return this.from.Found;
            }
        }
        /// <summary>
        /// Si la ubicación final existe devuelve que la encontró.
        /// </summary>
        public bool ToExists
        {
            get
            {
                return this.to.Found;
            }
        }
        /// <summary>
        /// Get distancia.
        /// </summary>
        /// <value></value>
        public double Distance
        {
            get
            {
                return this.distance;
            }
        }
        /// <summary>
        /// Get time.
        /// </summary>
        /// <value></value>
        public double Time
        {
            get
            {
                return this.time;
            }
        }
        /// <summary>
        /// Get locacion inicial.
        /// </summary>
        /// <value></value>
        public Location From
        {
            get
            {
                return this.from;
            }
        }

        /// <summary>
        /// Devuelve el location.
        /// </summary>
        /// <value></value>
        public Location To
        {
            get
            {
                return this.to;
            }
        }
    }
}