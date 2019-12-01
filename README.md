Universidad Rafael Landivar
Facultad de Ingeniería
Estructuras de Datos II

PROYECTO DE APLICACIÓN - CHAT

Descripcion:
Plataforma de mensajería instantánea
(chat) una alternativa a las soluciones en el mercado que no cumplen los estándares de seguridad
de la empresa Mensajes Relevantes S.A. 

Requerimientos funcionales:
● Creación de usuarios en donde las contraseñas deberán almacenarse cifradas.
● Login
● El envío de los mensajes con cifrado de punto a punto.
● Capacidad de envío de archivos comprimidos para agilizar la descarga.
● Capacidad de almacenar el historial de mensajes entre usuarios y poder
visualizarlos en cualquier momento.
● Búsqueda de un mensaje por palabra clave en dónde el usuario haya sido emisor
o receptor del mismo.

Implementacion:
Solución de N capas, las cuales serían:
1. Capa de diseño o presentación: Deberá ser una aplicación WEB utilizando
Microsoft MVC.
2. Capa de comunicación: Una API en .Net Core que se comunique utilizando JSON
para manejo de datos y JWT para manejo de la sesión.
3. Capa de almacenamiento de datos: Todos los mensajes y los usuarios serán
almacenados en una base de datos no relacional MongoDB utilizando un GUID
como identificador único para cada registro.
4. Capa de procesos alternos: La cual será una DLL con los procesos de compresión
y cifrado los cuales implementaron en sus laboratorios del curso Estructura de
Datos II.

Notas:

Existe comunicación entre al menos 2 computadoras.
Se ha logrado probar el servidor con la ayuda de "ngrok".

La descarga de mensajes nuevos se hace a través cada cierto tiempo en el que se mantenga la aplicación en ejecución. 
(Con una funcion de javascript)
