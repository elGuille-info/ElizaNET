# ElizaVB

Biblioteca de clases escrito en Visual Basic .NET (usando .NET 7.0) para el programa Eliza en .NET

<br>

> **Nota del 06-feb-23 21.28:**<br>
> Esta DLL ya no se utiliza en los proyectos (aplicaciones de Windows Forms), ahora se utiliza el proyecto **Eliza gcnl Library**.<br>
> Por ahora la dejo por si quieres comparar el código con la DLL compilada para .NET 7.0 y creada como paquetre de NuGet.<br>


<br>

Donde se hace todo el análisis de lo que Eliza entiende y busca las respuestas, en la carpeta 'palabras' se incluye la base de conocimiento de Eliza.

<br>

> **Nota:** <br>
> Esta biblioteca (DLL) se utiliza tanto en la aplicación principal para usar Eliza en Visual Basic .NET o en C#.

<br>

> **Nota:** <br>
> El contenido de la carpeta _palabras_, con las palabras que el programa reconocerá, ahora se copia en AppData\Local\Eliza\palabras.<br>
> Se copian todos los ficheros que cumplen este _pattern_ Eliza*.txt<br>
> <br>
> Tal como está configurado el contenido de ese directorio en este proyecto, primero se copian al path del ejecutable.<br>
> Por tanto, si se hacen cambios en algunos de los ficheros de palabras, comprobar antes si el que hay en AppDataLocal se ha modificado, ya que esos ficheros pueden ser utilizados por otros proyectos, como es el de Eliza para .NET MAUI.
