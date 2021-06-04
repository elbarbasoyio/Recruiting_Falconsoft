# Recruiting Exercise A 
# Objetivo
El ejercicio apunta a demostrar algunas de las capacidades esenciales para el desarrollo de
aplicaciones de gestión e inteligencia de negocio. 
Entre ellas:
* Diseño y estructuración de modulos y clases.
* Resolución de lògica de negocio.
* Aplicación de procedimientos y componentes de seguridad.
* Claridad en código fuente.
* Optimización de performance y manejo de recursos.

# Descripción
Crear una WebAPI que permita operar una colección paginada con un total de 50 registros
(pedidos, información dummy).
Registros (pedido)
Debe permitir:
* Obtener 10 registros por página.
* Ordenar y filtrar por cualquier propiedad del pedido.
* Editar algún campo de los productos del pedido y guardarlo.
El service debe consistir en un .NET WebAPI en C# (solo Back End).

# Requerimientos
* Implementar arquitectura en capas y orientada a servicios.
* Implementar comunicación Http REST.
* Token based security: la aplicación debe poder ser accedida solamente por usuarios
autenticados.
* La lectura sobre DB debe ser no bloqueante.

# Tecnologías a utilizar
* .NET Core WebAPI
* JSON Web Tokens (JWT)
* Base de datos: SQL Server Express local DB, SQLite o cualquier DB local.
* Linq - Entity Framework

# Entregables
* Còdigo fuente compilable
* Proyecto unit testing con comprobación de funcionamiento de la Web.API,
realizando llamadas Http (webclient o httpclient).
