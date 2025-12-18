# Personal
Hacer formularios con modelo de 3 capas para el CRUD de:

1) CREATE TABLE productos ( id INT IDENTITY(1,1) PRIMARY KEY, descripcion VARCHAR(100) );
2) CREATE TABLE precios ( id INT IDENTITY(1,1) PRIMARY KEY, fecha DATE, monto INT, idProducto INT, FOREIGN KEY (idProducto) REFERENCES productos(id) );





Se agregan las referencias a cada capa
*Datos
Referencias de proyecto
-Modelos
Referencias de Ensamblados(esto es para poder acceder a la cadena de conexion que está en Presentacion)
-System.configuration

*Negocio
Referencias de proyecto
-Datos
-Modelos

*Presentacion
Referencias de proyecto
-Modelos
-Negocio

