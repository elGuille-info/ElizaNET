'------------------------------------------------------------------------------
' Clasificar                                                        (22/ene/23)
' Clase para clasificar los elementos de un array
'
' ©Guillermo Som (Guille), 2023
'------------------------------------------------------------------------------
Option Strict On
Option Infer On
Option Explicit On

Imports System
Imports System.Collections
Imports System.Collections.Generic

Public Class Clasificar
    Implements IComparer(Of String)

    Public Sub New(Optional deMayorAMenor As Boolean = True)
        MayorAMenor = deMayorAMenor
    End Sub
    Private ReadOnly MayorAMenor As Boolean

    Public Function Compare(x As String, y As String) As Integer Implements IComparer(Of String).Compare
        If MayorAMenor Then
            Return (New CaseInsensitiveComparer()).Compare(y, x)
        Else
            Return (New CaseInsensitiveComparer()).Compare(x, y)
        End If
    End Function
End Class
