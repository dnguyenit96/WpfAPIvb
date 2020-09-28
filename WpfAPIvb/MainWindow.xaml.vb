Imports System.Collections.Specialized
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions

Class MainWindow
    Dim Position As String = "2,9,30,18,23,16,12,17,22,1"
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        txtLinkAPI.Focus()

        txtTime.IsReadOnly = chkTime.IsChecked
        If IO.File.Exists("token.txt") Then
            Dim sT As New IO.StreamReader("token.txt")
            txtToken.Text = sT.ReadLine()
            sT.Close()
        End If

        If IO.File.Exists("link.txt") Then
            Dim sT As New IO.StreamReader("link.txt")
            txtLinkAPI.Text = sT.ReadLine()
            sT.Close()
        End If

        If IO.File.Exists("source.txt") Then
            Dim sT As New IO.StreamReader("source.txt")
            txtSource.Text = sT.ReadLine()
            sT.Close()
        End If

        If IO.File.Exists("privatekey.txt") Then
            Dim sT As New IO.StreamReader("privatekey.txt")
            txtPrivateKey.Text = sT.ReadLine()
            sT.Close()
        End If

        If IO.File.Exists("userid.txt") Then
            Dim sT As New IO.StreamReader("userid.txt")
            txtUserID.Text = sT.ReadLine()
            sT.Close()
        End If
    End Sub

    Private Sub btnMinimizeForm_Click(sender As Object, e As RoutedEventArgs)
        WindowState = WindowState.Minimized
    End Sub

    Private Sub btnCloseForm_Click(sender As Object, e As RoutedEventArgs)
        Application.Current.Shutdown()
    End Sub
    Private Sub gridTitleBarLeft_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        DragMove()
    End Sub
    Private Sub Window_KeyDown(sender As Object, e As KeyEventArgs)
        Dim [next] As Boolean = True

        If e.Key = Key.Tab Then
            e.Handled = True
            Return
        End If

        If e.Key = Key.Enter Then
            Dim tRequest As TraversalRequest = New TraversalRequest(FocusNavigationDirection.[Next])
            Dim keyboardFocus As UIElement = TryCast(Keyboard.FocusedElement, UIElement)

            If CType(sender, Control) Is txtSource Then
                [next] = getDirectory()
            End If

            If keyboardFocus IsNot Nothing AndAlso [next] Then
                keyboardFocus.MoveFocus(tRequest)
            End If

            e.Handled = True
        End If

    End Sub

    Private Function getDirectory() As Boolean
        Dim _clsDirectoryXML As clsDirectory = New clsDirectory()
        Dim path As String = txtSource.Text

        If path.Length > 0 Then

            If Directory.Exists(path) Then
                ' This path is a directory
                Dim directoryFiles As List(Of clsDirectory.DirectoryXML) = _clsDirectoryXML.ProcessDirectory(path)
                cbxController.ItemsSource = directoryFiles
                cbxController.SelectedIndex = 0
                Return True
            Else
                Dim dialogWindow As DialogWindow = New DialogWindow()
                dialogWindow._DialogMessage = String.Format("{0} is not a valid file or directory.", path)
                dialogWindow.ShowDialog()
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub txtSource_TextChanged(sender As Object, e As TextChangedEventArgs)
        Dim path As String = txtSource.Text

        If path.Length > 0 Then

            If Directory.Exists(path) Then
                getDirectory()
            End If
        End If
    End Sub

    Private Sub txtUserID_PreviewTextInput(sender As Object, e As TextCompositionEventArgs)
        Dim regex As Regex = New Regex("[^0-9]+")
        e.Handled = regex.IsMatch(e.Text)
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As RoutedEventArgs)
        Dim sTime As String = DateTime.Now.ToString("yyyyMMddHHmmssffffff")
        txtKey.Text = clsLib.genKeyFromData(Position, txtPrivateKey.Text, txtUserID.Text.Trim + sTime)

        txtMD5.Text = clsLib.md5(txtPrivateKey.Text + txtUserID.Text.Trim + sTime)
        txtTime.Text = sTime

        ' rtbUrl.Text = "tp=" + txtBase64.Text + "&ac=1&data=" + rtbResult.Text + "&key=" + rtbKey.Text

        Dim by() As Byte
        Dim net As New Net.WebClient
        Try
            '  net.Headers.Add("Content-Type", "text/plain")
            Dim vals As New NameValueCollection()
            vals.Add("userID", txtUserID.Text)
            vals.Add("time", sTime)
            vals.Add("key", txtKey.Text)

            txtPost.Text = "userID=" + txtUserID.Text + vbNewLine + "time=" + sTime + vbNewLine + "key=" + txtKey.Text
            'txtLink.Text = txturl.Text + "APIlogin.ashx"

            Dim sss As String = ""
            Try
                by = net.UploadValues(txtLinkAPI.Text + "APIlogin.ashx", vals)
                sss = Encoding.UTF8.GetString(by)
                txtResult.Text = sss
                'MessageBox.Show(sss)
                Dim dialogWindow As DialogWindow = New DialogWindow()
                dialogWindow._DialogMessage = sss
                dialogWindow.ShowDialog()
            Catch ex1 As WebException
                Dim stre As New StreamReader(ex1.Response.GetResponseStream)
                sss = stre.ReadToEnd
                txtResult.Text = sss
                stre.Close()
            End Try

            Dim o = clsLib.ConvertJsonToObject(sss)
            If o(0)("type") = "1" Then
                txtToken.Text = o(0)("token")
                Dim sT As New IO.StreamWriter("token.txt")
                sT.WriteLine(txtToken.Text)
                sT.Close()

                Dim sL As New IO.StreamWriter("link.txt")
                sL.WriteLine(txtLinkAPI.Text)
                sL.Close()

                Dim sS As New IO.StreamWriter("source.txt")
                sS.WriteLine(txtSource.Text)
                sS.Close()

                Dim sP As New IO.StreamWriter("privatekey.txt")
                sP.WriteLine(txtPrivateKey.Text)
                sP.Close()

                Dim sU As New IO.StreamWriter("userid.txt")
                sU.WriteLine(txtUserID.Text)
                sU.Close()

            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message + vbNewLine + ex.StackTrace)
            Dim dialogWindow As DialogWindow = New DialogWindow()
            dialogWindow._DialogMessage = ex.Message + vbNewLine + ex.StackTrace
            dialogWindow.ShowDialog()
            net.Dispose()
            Return
        End Try
    End Sub

    Private Sub btnPost_Click(sender As Object, e As RoutedEventArgs)
        Dim ControllerSelect As String = cbxController.SelectedValue.ToString()

        txtForm.Text = clsLib.Encode64(ControllerSelect)

        Dim dialogWindow As DialogWindow = New DialogWindow()


        Dim sTime As String = DateTime.Now.ToString("yyyyMMddHHmmssffffff")
        If chkTime.IsChecked = False Then
            sTime = txtTime.Text
        Else
            txtTime.Text = sTime
        End If

        txtResult.Text = clsLib.EncodeData(txtJson.Text)
        txtKey.Text = clsLib.genKeyFromData(Position, txtPrivateKey.Text, sTime + txtResult.Text)
        txtMD5.Text = clsLib.md5(txtPrivateKey.Text + sTime + txtResult.Text)

        ' rtbUrl.Text = "tp=" + txtBase64.Text + "&ac=1&data=" + rtbResult.Text + "&key=" + rtbKey.Text

        Dim by() As Byte
        Dim net As New Net.WebClient
        Try
            '  net.Headers.Add("Content-Type", "text/plain")
            Dim vals As New NameValueCollection()

            vals.Add("token", txtToken.Text)
            vals.Add("userid", txtUserID.Text)
            vals.Add("form", txtForm.Text)
            vals.Add("key", txtKey.Text)
            vals.Add("time", sTime)
            vals.Add("data", txtResult.Text)

            txtPost.Text = "token=" + txtToken.Text + vbNewLine + "userid=" + txtUserID.Text + vbNewLine + "form=" + txtForm.Text + vbNewLine + "key=" + txtKey.Text + vbNewLine + "time=" + sTime + vbNewLine + "data=" + txtResult.Text
            Dim sss As String = ""
            'txtLink.Text = txturl.Text + "APIQuery.ashx"
            Try
                by = net.UploadValues(txtLinkAPI.Text + "APIQuery.ashx", vals)
                sss = Encoding.UTF8.GetString(by)
            Catch ex1 As WebException
                Dim stre As New StreamReader(ex1.Response.GetResponseStream)
                sss = stre.ReadToEnd
                'MessageBox.Show(sss)
                dialogWindow._DialogMessage = sss
                dialogWindow.ShowDialog()
                stre.Close()
            End Try

            net.Dispose()
            txtResult.Text = sss
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
            dialogWindow._DialogMessage = ex.Message
            dialogWindow.ShowDialog()
            net.Dispose()
            Return
        End Try
    End Sub

    Private Sub btnDescrypt_Click(sender As Object, e As RoutedEventArgs)
        Dim dialogWindow As DialogWindow = New DialogWindow()

        Try
            txtJson.Text = ""
            txtResult.Text = ""
            txtKey.Text = txtKey.Text.Trim
            txtTime.Text = txtTime.Text.Trim

            If txtTime.Text.Trim = "" Then
                dialogWindow._DialogMessage = "Vui lòng nhập time!"
                dialogWindow.ShowDialog()
                'MessageBox.Show("Vui lòng nhập time!")
                txtTime.Focus()
                Return
            End If


            If txtKey.Text.Trim = "" Then
                dialogWindow._DialogMessage = "Vui lòng nhập key!"
                dialogWindow.ShowDialog()
                'MessageBox.Show("Vui lòng nhập key!")
                txtKey.Focus()
                Return
            End If

            If txtPost.Text.Trim = "" Then
                dialogWindow._DialogMessage = "Vui lòng nhập chuỗi Base 64!"
                dialogWindow.ShowDialog()
                'MessageBox.Show("Vui lòng nhập chuỗi Base 64!")
                txtPost.Focus()
                Return
            End If

            txtPost.Text = txtPost.Text.Trim
            txtJson.Text = clsLib.Decode64(txtPost.Text.Trim)

            Dim o = clsLib.ConvertJsonToObject(txtJson.Text)



            Dim md5 As String = clsLib.md5(txtPrivateKey.Text + txtTime.Text + txtPost.Text)
            Dim sKey As String = clsLib.genKeyFromData(Position, txtPrivateKey.Text, txtTime.Text + txtPost.Text)

            txtResult.Text = "MD5 Private Key + Time + Data: " + md5 + vbNewLine + vbNewLine + "Your key: " + txtKey.Text + ", Fast key: " + sKey + vbNewLine + vbNewLine + IIf(txtKey.Text = sKey, "Result: The Same", "Result: Don't the same")
            dialogWindow._DialogMessage = txtResult.Text
            dialogWindow.ShowDialog()
            'MessageBox.Show(txtResult.Text)
        Catch ex As Exception
            dialogWindow._DialogMessage = "Giải mã không được!"
            dialogWindow.ShowDialog()
            'MessageBox.Show("Giải mã không được!")
        End Try
    End Sub

    Private Sub btnEtoJ_Click(sender As Object, e As RoutedEventArgs)
        Dim s As String = Clipboard.GetText(), arr() As String, sB As New StringBuilder, aHeader() As String, a() As String
        Dim dialogWindow As DialogWindow = New DialogWindow()

        arr = s.Split(vbNewLine)
        If arr.Length = 1 Then
            dialogWindow._DialogMessage = "Dữ liệu không đúng cấu trúc copy từ excel"
            dialogWindow.ShowDialog()
            'MessageBox.Show("Dữ liệu không đúng cấu trúc copy từ excel")
            Return
        End If

        aHeader = arr(0).Split("	")
        sB.Append("[")
        For i As Integer = 1 To arr.Length - 1
            If arr(i).Trim <> "" Then

                a = arr(i).Split("	")
                s = ""
                If i > 1 Then sB.Append(", " + vbNewLine)

                For j As Integer = 0 To aHeader.Length - 1
                    If aHeader(j).Trim <> "" Then
                        Dim vaj As String = a(j).Trim
                        If IsNumeric(vaj) Then
                            vaj = vaj.Replace(",", "")
                        End If
                        s += IIf(s = "", "", "," + vbNewLine) + "   " + aHeader(j).Trim + ": """ + vaj + """"
                    End If
                Next

                sB.Append("{" + vbNewLine + s + vbNewLine + "}")
            End If
        Next
        sB.Append("]")
        txtJson.Text = sB.ToString
    End Sub

    Private Sub chkTime_Checked(sender As Object, e As RoutedEventArgs)
        txtTime.IsReadOnly = chkTime.IsChecked
    End Sub
End Class
