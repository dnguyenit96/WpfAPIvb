Imports System.IO

Public Class clsDirectory
    Public Function ProcessDirectory(ByVal targetDirectory As String) As List(Of DirectoryXML)
        Dim list As List(Of DirectoryXML) = New List(Of DirectoryXML)()
        ' Process the list of files found in the directory.

        Dim fileEntries As String() = Directory.GetFiles(targetDirectory)

        For Each fileName As String In fileEntries

            If Path.GetExtension(fileName).IndexOf("xml") > 0 Then
                list.Add(New DirectoryXML With {
                    .FileXMLName = Path.GetFileNameWithoutExtension(fileName)
                })
            End If
        Next

        Return list
    End Function

    Public Class DirectoryXML
        Private fileXMLNameField As String

        Public Property FileXMLName As String
            Get
                Return fileXMLNameField
            End Get
            Set(ByVal value As String)
                fileXMLNameField = value
            End Set
        End Property
    End Class
End Class
