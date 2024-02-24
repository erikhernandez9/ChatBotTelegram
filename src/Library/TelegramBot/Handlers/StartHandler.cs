using Telegram.Bot.Types;
using Proyecto_Chatbot.Usuarios;
using System;

namespace Proyecto_Chatbot.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "/start". Este handler
    /// pertenece a las USER STORIES, ya que implementa una forma de registrarse dentro del servicio
    /// de contratciones. Las user stories son:
    /// "Como trabajador, quiero registrarme en la plataforma, indicando mis datos personales e información
    /// de contacto para que de esa forma, pueda proveer información de contacto a quienes quieran contratar mis servicios."
    /// "Como empleador, quiero registrarme en la plataforma, indicando mis datos personales e información de contacto para
    /// que de esa forma, pueda proveer información de contacto a los trabajadores que quiero contratar."
    /// Este handler crea una opción de registro tanto para trabajador como para empleador, donde el usuario, si aún no
    /// está registrado, puede seleccionar el modo en el que quiere usar la plataforma, trabajador o empleador.
    /// </summary>
    public class StartHandler : BaseHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartHandler"/> class.
        /// Esta clase procesa el mensaje "/start" al inicio del bot.
        /// </summary>
        /// <param name="next">El próximo "handler".</param>
        public StartHandler(BaseHandler next) 
            : base(next)
        {
            this.Keywords = new string[] { "/start" };
        }

        /// <summary>
        /// Detecta si el usuario está registrado o no, en caso de estarlo, manda lista de comandos,
        /// en caso de no estarlo, hace que se registre.
        /// </summary>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        protected override void InternalHandle(Mensaje mensaje, out string response)
        {
            response = string.Empty;
            foreach (Empleador empleador in Singleton<ListaEmpleadores>.Instance.Empleadores)
            {
                if (mensaje.Id == empleador.Id)
                {
                    response = $"Bienvenido de nuevo {empleador.Nombre}.\nPara ver la lista de comandos, escriba ayuda";
                }
            }

            foreach (Trabajador trabajador in Singleton<ListaTrabajadores>.Instance.Trabajadores)
            {
                if (mensaje.Id == trabajador.Id)
                {
                    response = $"Bienvenido de nuevo {trabajador.Nombre}.\nPara ver la lista de comandos, escriba ayuda";
                }
            }

            foreach (Administrador administrador in Singleton<ListaAdministradores>.Instance.Administradores)
            {
                if (mensaje.Id == administrador.Id)
                {
                    response = $"Bienvenido de nuevo {administrador.Nombre}.\nPara ver la lista de comandos, escriba ayuda";
                }
            }

            if (response == string.Empty)
            {
                response = "Bienvenido al bot de Trabajo, para ingresar debe crear una cuenta, por favor, diganos de qué forma usará este programa:\nQuiero registrarme como trabajador\nQuiero registrarme como como empleador";
            }
        }
    }
}