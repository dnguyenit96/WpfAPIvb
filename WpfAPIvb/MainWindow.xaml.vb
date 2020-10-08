Imports System.Collections.Specialized
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Configuration

Class MainWindow
    Dim Position As String = ConfigurationManager.AppSettings("Position")
    Dim flagState As Integer = 0

    Dim GlobalWidth As Double = SystemParameters.WorkArea.Width
    Dim GlobalHeight As Double = SystemParameters.WorkArea.Height
    Dim DefaultHeight As Double = 700
    Dim DefaultWidth As Double = 1152

    Private Sub getConfig()
        txtLinkAPI.Text = ConfigurationManager.AppSettings("LinkAPI")
        txtPrivateKey.Text = ConfigurationManager.AppSettings("PrivateKey")
        txtUserID.Text = ConfigurationManager.AppSettings("UserID")
        cbxController.Text = ConfigurationManager.AppSettings("Controller")
        txtTime.Text = ConfigurationManager.AppSettings("Time")
        txtForm.Text = ConfigurationManager.AppSettings("Form")
        txtMD5.Text = ConfigurationManager.AppSettings("MD5")
        txtToken.Text = ConfigurationManager.AppSettings("Token")
    End Sub
    Private Sub setConfig(linkAPI As String, privateKey As String, userID As String, controller As String, time As String, form As String, MD5 As String, token As String)
        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)

        config.AppSettings.Settings("LinkAPI").Value = linkAPI
        config.AppSettings.Settings("PrivateKey").Value = privateKey
        config.AppSettings.Settings("UserID").Value = userID
        config.AppSettings.Settings("Controller").Value = controller
        config.AppSettings.Settings("Time").Value = time
        config.AppSettings.Settings("Form").Value = form
        config.AppSettings.Settings("MD5").Value = MD5
        config.AppSettings.Settings("Token").Value = token

        config.Save(ConfigurationSaveMode.Modified)
        ConfigurationManager.RefreshSection("appSettings")
    End Sub
    Private Sub DefaultForm(resize As Boolean, load As Boolean)
        Dim setWidth As Double
        Dim setHeight As Double
        If resize Then
            setWidth = GlobalWidth
            setHeight = GlobalHeight
        Else
            setWidth = DefaultWidth
            setHeight = DefaultHeight
        End If

        txtJson.Height = setHeight / 2 - 60
        txtResult.Height = setHeight / 2 - 60

        txtLinkAPI.Width = setWidth / 2 - 118
        txtPrivateKey.Width = setWidth / 2 - 278
        cbxController.Width = setWidth / 2 - 118
        txtTime.Width = setWidth / 2 - 198
        txtForm.Width = setWidth / 2 - 118
        txtMD5.Width = setWidth / 2 - 118
        txtKey.Width = setWidth / 2 - 118
        txtToken.Width = setWidth / 2 - 118

        txtPost.Height = setHeight - 400

        If load Then
            getConfig()
        End If
    End Sub

    Private Sub getController()
        Dim ControllerSelect As String
        Dim dialogWindow As DialogWindow = New DialogWindow()
        Dim sTime As String
        Dim result As String, key As String

        ControllerSelect = ConfigurationManager.AppSettings("getController")

        sTime = ConfigurationManager.AppSettings("getController")

        result = clsLib.EncodeData(txtJson.Text)
        key = clsLib.genKeyFromData(Position, txtPrivateKey.Text, sTime + result)

        Dim by() As Byte
        Dim net As New Net.WebClient
        Try
            Dim vals As New NameValueCollection()

            vals.Add("token", txtToken.Text)
            vals.Add("userid", txtUserID.Text)
            vals.Add("form", clsLib.Encode64(ControllerSelect))
            vals.Add("key", key)
            vals.Add("time", sTime)
            vals.Add("data", result)

            Dim sss As String = ""
            Try
                by = net.UploadValues(txtLinkAPI.Text + "APIQuery.ashx", vals)
                sss = Encoding.UTF8.GetString(by)
            Catch ex1 As WebException
                Dim stre As New StreamReader(ex1.Response.GetResponseStream)
                sss = stre.ReadToEnd
                dialogWindow._DialogMessage = sss
                dialogWindow.ShowDialog()
                stre.Close()
            End Try

            net.Dispose()

            Dim arrayController As String()
            arrayController = sss.Split(",")

            Dim list As List(Of DirectoryXML) = New List(Of DirectoryXML)()
            For Each fileName As String In arrayController
                list.Add(New DirectoryXML With {
                    .FileXMLName = fileName
                })
            Next

            cbxController.ItemsSource = list
            cbxController.SelectedIndex = 0
        Catch ex As Exception
            dialogWindow._DialogMessage = ex.Message
            dialogWindow.ShowDialog()
            net.Dispose()
            Return
        End Try
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        txtLinkAPI.Focus()

        txtTime.IsReadOnly = chkTime.IsChecked

        txtJson.Text = "[{" + vbNewLine + "cacheID: ""|cacheID|""" + vbNewLine + "}]"

        DefaultForm(False, True)
    End Sub

    Private Sub btnMinimizeForm_Click(sender As Object, e As RoutedEventArgs)
        WindowState = WindowState.Minimized
    End Sub
    Private Sub btnMaximizeForm_Click(sender As Object, e As RoutedEventArgs)

        'Scale
        If flagState = 1 Then
            icoMaximize.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize
            flagState = 0

            Application.Current.MainWindow.Height = DefaultHeight
            Application.Current.MainWindow.Width = DefaultWidth
            Application.Current.MainWindow.Top = GlobalHeight / 5
            Application.Current.MainWindow.Left = GlobalWidth / 5

            DefaultForm(False, False)
        Else
            icoMaximize.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore
            flagState = 1

            Application.Current.MainWindow.Height = GlobalHeight
            Application.Current.MainWindow.Width = GlobalWidth
            Application.Current.MainWindow.Top = 0
            Application.Current.MainWindow.Left = 0

            DefaultForm(True, False)
        End If
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

            If keyboardFocus IsNot Nothing AndAlso [next] Then
                keyboardFocus.MoveFocus(tRequest)
            End If

            e.Handled = True
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
        If chkTime.IsChecked = True Then
            txtTime.Text = sTime
        Else
            sTime = txtTime.Text
        End If

        Dim by() As Byte
        Dim net As New Net.WebClient
        Try
            Dim vals As New NameValueCollection()
            vals.Add("userID", txtUserID.Text)
            vals.Add("time", sTime)
            vals.Add("key", txtKey.Text)

            txtPost.Text = "userID=" + txtUserID.Text + vbNewLine + "time=" + sTime + vbNewLine + "key=" + txtKey.Text

            Dim sss As String = ""
            Try
                by = net.UploadValues(txtLinkAPI.Text + "APIlogin.ashx", vals)
                sss = Encoding.UTF8.GetString(by)
                txtResult.Text = sss
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
                getController()
            End If

            setConfig(txtLinkAPI.Text, txtPrivateKey.Text, txtUserID.Text, "", sTime, "", txtMD5.Text, txtToken.Text)
        Catch ex As Exception
            Dim dialogWindow As DialogWindow = New DialogWindow()
            dialogWindow._DialogMessage = ex.Message + vbNewLine + ex.StackTrace
            dialogWindow.ShowDialog()
            net.Dispose()
            Return
        End Try
    End Sub

    Private Sub btnPost_Click(sender As Object, e As RoutedEventArgs)
        Dim ControllerSelect As String
        Dim dialogWindow As DialogWindow = New DialogWindow()

        If cbxController.SelectedValue Is Nothing Then
            ControllerSelect = cbxController.Text
        Else
            ControllerSelect = cbxController.SelectedValue.ToString()
        End If

        If ControllerSelect = "" Then
            dialogWindow._DialogMessage = "Controller không được bỏ trống!"
            dialogWindow.ShowDialog()
            cbxController.Focus()
            Return
        End If

        txtForm.Text = clsLib.Encode64(ControllerSelect)

        Dim sTime As String = DateTime.Now.ToString("yyyyMMddHHmmssffffff")
        If chkTime.IsChecked = False Then
            sTime = txtTime.Text
        Else
            txtTime.Text = sTime
        End If

        txtResult.Text = clsLib.EncodeData(txtJson.Text)
        txtKey.Text = clsLib.genKeyFromData(Position, txtPrivateKey.Text, sTime + txtResult.Text)
        txtMD5.Text = clsLib.md5(txtPrivateKey.Text + sTime + txtResult.Text)

        setConfig(txtLinkAPI.Text, txtPrivateKey.Text, txtUserID.Text, ControllerSelect, sTime, txtForm.Text, txtMD5.Text, txtToken.Text)

        Dim by() As Byte
        Dim net As New Net.WebClient
        Try
            Dim vals As New NameValueCollection()

            vals.Add("token", txtToken.Text)
            vals.Add("userid", txtUserID.Text)
            vals.Add("form", txtForm.Text)
            vals.Add("key", txtKey.Text)
            vals.Add("time", sTime)
            vals.Add("data", txtResult.Text)

            txtPost.Text = "token=" + txtToken.Text + vbNewLine + "userid=" + txtUserID.Text + vbNewLine + "form=" + txtForm.Text + vbNewLine + "key=" + txtKey.Text + vbNewLine + "time=" + sTime + vbNewLine + "data=" + txtResult.Text
            Dim sss As String = ""
            Try
                by = net.UploadValues(txtLinkAPI.Text + "APIQuery.ashx", vals)
                sss = Encoding.UTF8.GetString(by)
            Catch ex1 As WebException
                Dim stre As New StreamReader(ex1.Response.GetResponseStream)
                sss = stre.ReadToEnd
                dialogWindow._DialogMessage = sss
                dialogWindow.ShowDialog()
                stre.Close()
            End Try

            net.Dispose()
            txtResult.Text = sss
        Catch ex As Exception
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
                txtTime.Focus()
                Return
            End If

            If txtKey.Text.Trim = "" Then
                dialogWindow._DialogMessage = "Vui lòng nhập key!"
                dialogWindow.ShowDialog()
                txtKey.Focus()
                Return
            End If

            If txtPost.Text.Trim = "" Then
                dialogWindow._DialogMessage = "Vui lòng nhập chuỗi Base 64!"
                dialogWindow.ShowDialog()
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
        Catch ex As Exception
            dialogWindow._DialogMessage = "Giải mã không được!"
            dialogWindow.ShowDialog()
        End Try
    End Sub

    Private Sub btnEtoJ_Click(sender As Object, e As RoutedEventArgs)
        Dim s As String = Clipboard.GetText(), arr() As String, sB As New StringBuilder, aHeader() As String, a() As String
        Dim dialogWindow As DialogWindow = New DialogWindow()

        arr = s.Split(vbNewLine)
        If arr.Length = 1 Then
            dialogWindow._DialogMessage = "Dữ liệu không đúng cấu trúc copy từ excel"
            dialogWindow.ShowDialog()
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
