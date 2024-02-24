using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Proyecto_Chatbot
{
    /// <summary>
    /// Clase que representa la calificacion del trabajador y empleador.
    /// Implementa la interfaz IJsonConvertible, por lo que se utiliza el principio DIP.
    /// Se utilizó el principio SRP ya que la clase solo se centra en contener la
    /// calificación y lanzar una excepción en caso de que se ingrese una calificación inválida.
    /// </summary>
    public class Calificacion : IJsonConvertible
    {
        /// <summary>
        /// Representa la descripcion de la calificacion.
        /// </summary>
        public string Descripcion{ get; set; }

        /// <summary>
        /// Representa el puntaje de la calificacion
        /// La calificacion va de 0 a 5 puntos.
        /// </summary>
        public double Puntaje { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Calificacion"/> class.
        /// </summary>
        /// <param name="descripcion">Almacena la descripcion que se le asigna a la calificacion.</param>
        /// <param name="puntaje">Almacena el puntaje que se le asigna a la calificacion.</param>
        
        [JsonConstructor]

        public Calificacion(string descripcion, double puntaje)
        {
            if (puntaje >= 0 && puntaje <= 5)
            {
                this.Descripcion = descripcion;
                this.Puntaje = puntaje;
            }
            else
            {
                throw new ExcepcionConstructor("Parametros incorrectos");
            }
        }
        /// <summary>
        /// Convierte a json los datos de la clase.
        /// </summary>
        /// <returns>devuelve los datos convertidos.</returns>
        public string ConvertToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        /// <summary>
        /// Carga los datos json a la clase.
        /// </summary>
        /// <param name="json">json a deserializar.</param>
        public void LoadFromJson(string json)
        {
            Calificacion deserialized = JsonSerializer.Deserialize<Calificacion>(json);
            this.Descripcion = deserialized.Descripcion;
            this.Puntaje = deserialized.Puntaje;
        }
    }
}