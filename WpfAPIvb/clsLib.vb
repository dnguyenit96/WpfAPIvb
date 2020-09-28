Imports System.Data
Imports System.Text
Imports System.Web.Script.Serialization

Public Class clsLib

    Public Shared Function Decode64(ByVal s As String) As String
        s = Encoding.UTF8.GetString(Convert.FromBase64String(s))
        Return s
    End Function

    Public Shared Function Encode64(ByVal str As String) As String
        Dim bt() As Byte = Encoding.UTF8.GetBytes(str)
        str = Convert.ToBase64String(bt)
        Return str
    End Function

    ' Private Shared sTmp As String = "abcdefghijklmnopqrstuvwcyz0123456789ABCDEFGHIJKLMNOPQRSTUVWCYZ"
    'Private Shared privateKey As String = "", nPosition() As Integer

    Public Shared Function genKeyFromData(listPosition As String, privateKey As String, sData As String) As String
        Dim arr() As String = listPosition.Replace(" ", "").Split(",")
        Dim nPosition(arr.Length - 1) As Integer
        For i As Integer = 0 To arr.Length - 1
            nPosition(i) = Integer.Parse(arr(i))
        Next

        Dim sMd5 As String = md5(privateKey + sData)

        Dim sKey As String = ""
        For i As Integer = 0 To nPosition.Length - 1
            sKey += sMd5(nPosition(i) - 1)
        Next
        Return sKey
    End Function

    Public Shared Function EncodeData(sData As String) As String
        sData = Encode64(sData)
        Return sData
    End Function


    Private Shared sTimeOld As String = ""
    Public Shared Function DecodeData(sData As String) As Object

        sData = Decode64(sData)
        Return ConvertJsonToObject(sData)
    End Function

    Public Shared Function ConvertJsonToObject(sData As String) As Object
        'Dim jss As New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim jss As New JavaScriptSerializer()
        Return jss.DeserializeObject(sData)
    End Function

    Public Shared Function convertDataTableToJson(dt As DataTable) As String
        Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer

        Dim rows As New List(Of Dictionary(Of String, Object))
        Dim row As Dictionary(Of String, Object)

        For Each dr As DataRow In dt.Rows
            row = New Dictionary(Of String, Object)
            For Each c As DataColumn In dt.Columns
                row.Add(c.ColumnName, trimValue(dr(c.ColumnName)))
            Next
            rows.Add(row)
        Next


        Return serializer.Serialize(rows)
    End Function

    Private Shared Function trimValue(val As Object) As Object
        If val Is Nothing Then
            Return Nothing
        End If

        Select Case val.GetType.Name
            Case "String"
                Return val.ToString.Trim
            Case "Date" : Case "DateTime"
                Return CType(val, DateTime).ToString("dd/MM/yyyy HH:mm:ss").Replace(" 00:00:00", "")
            Case Else
                Return val
        End Select
        Return val
    End Function

#Region "Security"

#Region "Var"
    Private Shared sKey As String = "", nDay As Integer = 0, nowDay As String
#End Region

    Private Shared Function encryptData(data As String) As Byte()
        Dim md5Hasher As New System.Security.Cryptography.MD5CryptoServiceProvider()
        Dim hashedBytes As Byte()
        Dim encoder As New System.Text.UTF8Encoding()
        hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data))
        Return hashedBytes
    End Function

    Public Shared Function md5(data As String) As String
        Return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower()
    End Function

#End Region

End Class

Public Enum clsDataType
    NumberType
    DateTimeType
    StringType
    BooleanType
End Enum

