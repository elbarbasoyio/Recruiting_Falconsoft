# Recruiting Exercise B (Concepts)

# Objetivo
El ejercicio apunta a demostrar conocimiento y capacidades de creación de algoritmos
utilizando .NET Core, y configuraciones del .NET Core WebAPI.
# Descripcion
### 1.  Lists matching using Linq

Teniendo 2 listas tipo List<Sample> que contienen 500.000 elementos cada una:
```javascript
public class Sample
{
  public string ID { get; set; } // Is PK
  public string Content { get; set; }
  
public int Qty { get; set; }
}
```
Utilizando Linq, de forma performante, crear una nueva lista solo con aquellos elementos
que están incluidos en ambas listas.

Nota: La propiedad ID actúa como PK. No hay elementos repetidos dentro de cada lista.
Info tipo dummy autogenerada.

### 2. Http call & Deserialization
Usando un WebClient realizar un llamado get http que descargue un json string (400 MBs
aprox), una lista de objetos.
Guardar el json obtenido en disco, y deserializar obteniendo un nuevo objeto con la lista.
Nota: Crear un modelo o reutilizar el Sample anterior.

### 3. Parallel exception handling
Crear una rutina que ejecute un loop con 10 threads paralelas utilizando como input por
ejemplo, una lista tipo del item 1, donde cada iteración debe sumar el Qty del objeto Sample
para retornar la suma como resultado.
La iteración de dicho código en paralelo debe ser cancelable.
Se pide simular una cancelación y simular otro tipo de exceptions dentro del loop.
El llamador de la rutina debe ser capaz de recibir el resultado de la ejecución y las
excepciones generadas por la misma y poder distinguir los tipos de exception para darles
diferente tratamiento a cada uno.

# Tecnologías a utilizar
* .NET Core WebAPI
# Entregables
* Proyecto Unit testing con las soluciones con código fuente compilable
