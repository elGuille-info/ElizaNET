# ElizaNET
Eliza para .NET basado en ElizaVB


Esta versión, para Visual Basic .NET y C# usando .NET 7.0, está basado en la versión de Visual Basic 6: revisión 0.17.0.1300 del 18/Sep/2002 (29/Jun/98).<br>
El original fue creado con Visual Basic 5 el Sábado, 30/May/98 17:30.<br>

El programa ELIZA original fue creado por Joseph Weizenbaum.<br>
La idea del formato de las reglas y simplificación de entradas, están basadas en 'ELIZA in Prolog' de Viren Patel.

Agradecimiento especial a Svetlana por toda la información aportada, además de ampliar la base de conocimientos de Eliza y darle un toque más femenino del que yo jamás le hubiese podido dar... y, sobre todo, por motivarme a hacer este programa, sin su ayuda no hubiera sido posible...

<br>

> **Nota:** <br>
> Incluyo el código original para Visual Basic 6.0. <br>
> Incluyo proyectos para usar la versión de .NET tanto con Visual Basic como con C# usando .NET 7.0.

<br>

> **Nota del 6-feb-2023:**<br>
> En este repositorio incluyo el proyecto [**Eliza gcnl Library**](https://github.com/elGuille-info/ElizaNET/tree/master/Eliza%20gcnl%20Library) con el código para crear la DLL con las clases de Eliza y la clase _Frases_ para analizar textos usando Google Cloud Natural Language.<br>
> Este proyecto (creado en C# a partir de ElizaVB) sustituye, o eso pretendo, al proyecto ElizaVB.<br>
> Esa DLL se puede usar incluyendo una referencia a este paquete de NuGet: [Eliza gcnl Library](https://www.nuget.org/packages/Eliza_gcnl_Library/).<br>

<br>

> **Nota sobre el análisis de textos:** <br>
> Los proyectos **ElizaNETVB** y **ElizaNETCS** (y **AnalizarTextos**) analizan el texto que se indica.<br>
> Pero como se llama a la API de Google Cloud Natural Language, se necesita el fichero _key.json_ con las claves pertinentes.<br>
> **El fichero key.json no se incluye (o no está disponible) en estos proyectos** (ni en el de aplicación de consola).<br>
> Debes usar el tuyo propio, tal como indico en este post de mi blog: [Google Cloud Natural Language, ejemplo en Visual Basic .NET](https://www.elguillemola.com/google-cloud-natural-language-ejemplo-en-visual-basic-net/)<br>
> Ahí te explico los pasos necesarios para crear un proyecto en Google Cloud y crear el fichero key.json.<br>

