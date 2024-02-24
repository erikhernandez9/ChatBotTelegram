using System;

namespace Proyecto_Chatbot
{
    /// <summary>
    /// SINGLETON:
    /// Garantiza que una clase solo tenga una instancia.
    /// Se utilizó el patrón de diseño Singleton, para restringir la creación de objetos pertenecientes a
    /// una clase, para asegurar que la clase no puede ser instanciada nuevamente.
    /// Lo aplicamos en:
    ///     - Catálogo Categorías.
    ///     - Catálogo Contrataciones.
    ///     - Catálogo Ofertas.
    ///     - Lista Empleadores.
    ///     - Lista Trabajadores.
    /// Porque consideramos que solo debe haber una instancia de los catálogos y las listas. Por ejemplo, no
    /// pueden existir dos catálogos de categorías, solo hay uno. Entonces, CatálogoCategorías es un Singleton.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : new()
    {
        private Singleton()
        {
        }
        private static T instance;
        /// <summary>
        /// Instancia de clase singleton.
        /// </summary>
        /// <value></value>
        public static T Instance 
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }

                return instance;
            }
        }
    }
}