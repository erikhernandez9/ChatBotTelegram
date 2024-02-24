# PROYECTO PROGRAMACIÓN 2

- USER STORIES:

Cómo administrador, quiero poder indicar categorías sobre las cuales se realizarán las ofertas de servicios para que de esa forma, los trabajadoras puedan clasificarlos. [CUMPLE]

Como administrador, quiero poder dar de baja ofertas de servicios, {avisando al oferente para que de esa forma, pueda evitar ofertas inadecudas}. [CUMPLE]

Como trabajador, quiero registrarme en la plataforma, indicando mis datos personales e información de contacto para que de esa forma, pueda proveer información de contacto a quienes quieran contratar mis servicios. [CUMPLE]

Como trabajador, quiero poder hacer ofertas de servicios; mi oferta indicará en qué categoría quiero publicar, tendrá una descripción del servicio ofertado, y un precio para que de esa forma, mis ofertas sean ofrecidas a quienes quieren contratar servicios. [CUMPLE]

Como empleador, quiero registrarme en la plataforma, indicando mis datos personales e información de contacto para que de esa forma, pueda proveer información de contacto a los trabajadores que quiero contratar. [CUMPLE]

Como empleador, quiero buscar ofertas de trabajo, opcionalmente filtrando por categoría para que de esa forma, pueda contratar un servicio. [CUMPLE]

Como empleador, quiero ver el resultado de las búsquedas de ofertas de trabajo ordenado en forma *ascendente* de *distancia a mi ubicación*, es decir, las más cercanas primero para que de esa forma, pueda poder contratar un servicio. [CUMPLE]

Como empleador, quiero ver el resultado de las búsquedas de ofertas de trabajo ordenado en forma *descendente* por reputación, es decir, las *de mejor reputación primero* para que de esa forma, pueda contratar un servicio. [CUMPLE]

Como empleador, quiero poder contactar a un trabajador para que de esa forma pueda, contratar una oferta de servicios determinada. [CUMPLE]

Como *trabajador*, quiero poder calificar a un empleador; el empleador me tiene que calificar a mi también, {si no me califica en un mes, la calificación será neutral, para que de esa forma pueda definir la reputación de mi empleador.} [CUMPLE]

Como *empleador*, quiero poder calificar a un trabajador; el trabajador me tiene que calificar a mi también, {si no me califica en un mes, la calificación será neutral, para que de esa forma, pueda definir la reputación del trabajador.} [CUMPLE]

- Como trabajador, quiero poder saber la reputación de un empleador que me contacte para que de esa forma, poder decidir sobre su solicitud de contratación. [CUMPLE]

//________________________________________________________________________________________________//

MATERIAL:

recopilamos información de patrones y design patterns de:
- https://refactoring.guru/es
- https://en.wikipedia.org/wiki/Plain_old_CLR_object-
- https://jbravomontero.files.wordpress.com/2012/12/solid-y-grasp-buenas-practicas-hacia-el-exito-en-el-desarrollo-de-software.pdf
...
y materiales del curso.


//__________________________________________________//

*GRASP Y SOLID: PATRONES Y PRINCIPIOS.*

- GRASP:

        - Expert: Experto en información nos dice que la responsabilidad de la creación de un objeto o la implementación de un método, debe recaer sobre la clase que conoce toda la información necesaria para crearlo o ejecutarlo.
        - Creator: nos ayuda a identificar quién debe ser el responsable de la creación o instanciación de nuevos objetos o clases. La nueva instancia podrá ser creada si: Contiene o agrega la clase, tiene la información necesaria para realizar la creación del objeto, o usa directamente las instancias creadas del objeto.
        - Polimorfismo:  polimorfismo es permitir que varias clases se comporten de manera distinta dependiendo del tipo que sean. Siempre que se haga una responsabilidad que dependa de un tipo, utilizaremos polimorfismo.
        - Low coupling: Tener las clases lo menos "conectadas" entre sí que se pueda, para tener la mínima repercusión posible en el resto de clases por si se modifica alguna. Potenciando la reutilización y disminuyendo la dependencia.
        - High Cohesion: Nos dice que la información que almacena una clase debe de ser coherente y debe estar, en la medida de lo posible,          relacionada con la clase.  

- SOLID:

        - SRP:  Una clase debería concentrarse sólo en hacer una cosa para que cuando cambie algo dicho cambio sólo afecte a esa clase por una razón. (single responsability)
        - DIP: para conseguir robustez y flexibilidad y para posibilitar la reutilización el código depende de abstracciones y no de concreciones, utilizar muchas interfaces y muchas clases abstractas.
        - ISP: Interface segregation principle. Mantener las interfaces pequeñas y cohesivas para que puedan coexistir unas con otras. (no depender de interfaces con cosas que no utilicen) 
        - OCP: Cambia el comportamiento de una clase mediante herencia, polimorfismo y composición. Los objetos o entidades deben estar abiertos para la extensión pero cerrados para la modificación.
        - LSP: Las subclasses deben comportarse adecuadamente cuando sean usadas en lugar de sus clases base. 

- OTROS PATRONES:

                - Controlador: sirve como intermediario entre una determinada interfaz y el algoritmo que la implementa, de tal forma que es el controlador quien recibe los datos del usuario y quien los envía a las distintas clases según el método llamado.
                - Chain of responsability: evita acoplar el emisor de una petición a su receptor dando a más de un objeto la posibilidad de responder a una petición.
ACLARACIÓN SOBRE LOS TESTS: Al momento de correr los siguientes tests simultáneamente da error por motivos de que al hacer los tests se elimino mucha informacion de los json y no podemos testearlo para un correcto funcionamiento ya que no tenemos los datos, dichos tests son: EliminarOfertaHandlerTest, EliminarEmpleadorHandlerTest, CrearOfertaLaboralHandlerTest, CrearContratacionHandlerTest, CalificarTrabajadorHandlerTest, CalificarEmpleadorHandlerTest, EliminarTrabajadorHandlerTest.
