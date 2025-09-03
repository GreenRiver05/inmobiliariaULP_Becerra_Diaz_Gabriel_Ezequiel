Durante el comienzo inicial del proyecto, use prefijos numéricos en los archivos de Models (ej. 00_Irepositorio.cs, 02_RepositorioPropietario.cs) para facilitar la lectura y evitar confundirme entre las clases con nombres similares (IRepositorio, RepositorioPropietario, etc.).


⚠️ Nota técnica: Por lo que anduve leyendo y consultando a la IA tambien, en C#, el nombre del archivo no afecta la clase. Lo que importa es su declaración (public class Nombre) y el namespace. El compilador procesa todos los .cs incluidos, sin importar cómo se llamen, siempre que estén bien referenciados. Incluso pueden convivir varias clases en un mismo archivo o tener nombres distintos al del archivo, y todo compila sin problema.


Esta convención la mantengo para ordenar mentalmente el flujos. Luego cambabiare a nombres más convencionales.
