'--------------------------------------------------------------------------------
' Punto de entrada de ElizaNETVB.                               (26/ene/23 17.21)
'
' (c) Guillermo Som (Guille), 2023
'--------------------------------------------------------------------------------
Option Strict On
Option Infer On

Imports System
Imports System.Windows.Forms

Friend Module Program

    <STAThread()>
    Friend Sub Main(args As String())
        Application.SetHighDpiMode(HighDpiMode.SystemAware)
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        Application.Run(New fEliza())

    End Sub

End Module
