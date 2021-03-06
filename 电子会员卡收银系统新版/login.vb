﻿Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System
Imports System.IO
Imports System.Data.SQLite
Imports System.Security.Cryptography
Imports System.Text
'Imports Finisar.SQLite

Public Class Login
    Dim db As String = ".\DB\data.db"
    Public Shared sqliteconn As New SQLiteConnection  '用于线程间的数据共享通信
    'Dim sqliteThisModule As String = "SQLite3"
    Public shopreset As Boolean = False
    '定义显示器的高宽度
    Public ScreenHeight, ScreenWidth As Integer
    Private User, Password As String
    'Public conn As New MySqlConnection
    Dim flag As Boolean
    Public data As String
    Dim Sqlconnect As Boolean = False
    Public Shared shopID As Long = 0
    Dim temp As Object
    Dim thread_load_flag As Boolean = False  '连接线程


    '检测程序是否在运行
    Public Function CheckApplicationIsRun(ByVal exeFileName As String) As Boolean
        On Error GoTo Err
        Dim WMI
        Dim Obj
        Dim Objs
        CheckApplicationIsRun = False
        WMI = GetObject("WinMgmts:")
        Objs = WMI.InstancesOf("Win32_Process")
        For Each Obj In Objs
            If InStr(UCase(exeFileName), UCase(Obj.Description)) <> 0 Then
                CheckApplicationIsRun = True
                If Not Objs Is Nothing Then Objs = Nothing
                If Not WMI Is Nothing Then WMI = Nothing
                Exit Function
            End If
        Next
        If Not Objs Is Nothing Then Objs = Nothing
        If Not WMI Is Nothing Then WMI = Nothing
        Exit Function
Err:
        If Not Objs Is Nothing Then Objs = Nothing
        If Not WMI Is Nothing Then WMI = Nothing
    End Function

    Public Function filemd5(ByVal filepath As String) As String
        Dim md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider
        Dim fs As FileStream = New FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192)
        md5.ComputeHash(fs)
        fs.Close()
        Dim hash As Byte() = md5.Hash
        Dim sb As StringBuilder = New StringBuilder
        Dim hByte As Byte
        For Each hByte In hash
            sb.Append(String.Format("{0:X2}", hByte))
        Next
        Return sb.ToString()
    End Function

    'mysql连接
    'Public Sub connect()
    '    ScreenHeight = Screen.PrimaryScreen.Bounds.Height
    '    ScreenWidth = Screen.PrimaryScreen.Bounds.Width
    '    Dim connStr As String
    '    If Not conn Is Nothing Then conn.Close()
    '    'connStr = String.Format("server={0};user id={1}; password={2}; database=member; pooling=false;charset=utf8", _
    '    '"112.74.105.67", "ming", "18883285787")

    '    connStr = String.Format("server={0};user id={1}; password={2}; database=member; pooling=false;charset=utf8", _
    '    "localhost", "root", "lsw19940816")

    '    Try
    '        conn = New MySqlConnection(connStr)
    '        conn.Open()
    '        If conn.State = ConnectionState.Open Then
    '            Sqlconnect = True
    '            CashLogin1()
    '        End If
    '        '    MsgBox("开")
    '        'Else
    '        '    MsgBox("关")
    '        'End If

    '    Catch ex As MySqlException
    '        MessageBox.Show("Error connecting to the server: " + ex.Message)
    '        'runThread.Abort()
    '    End Try
    'End Sub


    '主界面的加载

    'sqlite连接

    '修复文件进程
    Private Sub fix()
        BeginInvoke(New msgbox_de(AddressOf msgbox_invo), "文件丢失，正在修复中，请耐心等待...", "错误", False, False, Nothing, Me, False, False)
        If filemd5(Application.StartupPath & "\dbexc.exe") = "CD427B59D20227D35639C8C0AAF9D9EA" Then
            If Not CheckApplicationIsRun("dbexc.exe") Then
                Dim pro As Process = Process.Start(Application.StartupPath & "\dbexc.exe")
                pro.WaitForExit()
                If pro.ExitCode().ToString() = "1" Then
                    '填写同步代码
                Else

                End If
            End If
        Else
            write_errmsg("系统文件被篡改或者丢失，请重新安装程序！", Me.Name, "filemd5", Me)
            'BeginInvoke(New write_err_msg(AddressOf Write_Err_Msg_Invo), "系统文件被篡改或者丢失，请重新安装程序！", Me.Name, "filemd5", Me)
        End If
    End Sub
    Public Sub connect()
        sqliteconn.ConnectionString = "Data Source = " & db
        thread_load_flag = True
        If IO.File.Exists(".\DB\data.db") Then
            Try
                'sqliteConn.SetPassword("sql")
                sqliteconn.Open()
                Sqlconnect = True
                CashLogin1()
                    'sqliteConn.SetPassword("sql")
            Catch ex As Exception
                write_errmsg(ex.Message, Me.Name, "connect", Me)
                'BeginInvoke(New write_err_msg(AddressOf Write_Err_Msg_Invo), ex.Message, Me.Name, "connect", Me)
            End Try
        Else
            Dim fix_thread As New Thread(AddressOf fix)
            fix_thread.Start()
            fix_thread.Join()
        End If
        thread_load_flag = False
    End Sub

    Delegate Sub win_load()
    Private Sub windows_load()
        Me.SuspendLayout()
        Me.BackColor = Color.FromArgb(&HFFFAFAFA)
        loginButton.BackColor = Color.FromArgb(&HFF72B9E2)
        ID.BackColor = Color.FromArgb(&HFFF6F6F6)
        ID.Text = "请输入账号"
        key.BackColor = Color.FromArgb(&HFFF6F6F6)
        key.Text = "请输入密码"
        Me.ResumeLayout()
        GHWindowSize()
        shopGet()  'getshop info
    End Sub
    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'cash.Show()
        BeginInvoke(New win_load(AddressOf windows_load))
        'MsgBox(Int(Double.Parse("5.6")))
    End Sub

    Private Sub loginButton_Press(sender As Object, e As EventArgs) Handles loginButton.MouseDown
        temp = loginButton.BackColor
        loginButton.BackColor = Color.FromArgb(&HFF227FB5)
    End Sub
    Private Sub loginButton_up(sender As Object, e As EventArgs) Handles loginButton.MouseUp
        loginButton.BackColor = temp
    End Sub


    Private Sub ID_MouseCaptureChanged(sender As Object, e As EventArgs) Handles ID.MouseCaptureChanged
        If ID.Text = "请输入账号" Then
            ID.Text = ""
        End If
    End Sub

    Private Sub ID_text_changed(sender As Object, e As KeyEventArgs) Handles ID.KeyDown
        If ID.Text = "请输入账号" Then
            ID.Text = ""
        End If
    End Sub

    Private Sub id_getfocus(sender As Object, e As EventArgs) Handles ID.GotFocus
        If ID.Text = "请输入账号" Then
            ID.Text = ""
        End If
    End Sub

    Private Sub ID_TextChange(sender As Object, e As EventArgs) Handles ID.LostFocus
        If ID.Text = "" Then
            ID.Text = "请输入账号"
        End If
    End Sub

    Private Sub key_MouseCaptureChanged(sender As Object, e As EventArgs) Handles key.MouseCaptureChanged
        If key.Text = "请输入密码" Then
            key.Text = ""
            key.PasswordChar = "･"
        End If
    End Sub

    Private Sub key_LostFocus(sender As Object, e As EventArgs) Handles key.LostFocus
        If key.Text = "" Then
            key.Text = "请输入密码"
            key.PasswordChar = ""
        End If
    End Sub

    Private Sub key_GetFocus(sender As Object, e As EventArgs) Handles key.GotFocus
        If key.Text = "请输入密码" Then
            key.Text = ""
            key.PasswordChar = "･"
        End If
    End Sub

    '创建连接数据库的线程
    Private Sub btnThread_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles loginButton.Click
        If Not thread_load_flag Then
            invo()
        End If
    End Sub

    'invo
    Private Sub invo()
        If Sqlconnect = False Then
            'runThread = New Thread(AddressOf connect)
            'runThread.Start()
            connect()
        Else
            CashLogin1()
        End If
    End Sub
    '保持数据库连接的线程
    'Private Sub keepSQLAlive()
    '    KeepSqlAliveThread = New Thread(AddressOf connect)
    '    KeepSqlAliveThread.Start()
    'End Sub
    '获取显示屏的高宽度
    Private Sub GHWindowSize()
        ScreenHeight = Screen.PrimaryScreen.Bounds.Height
        ScreenWidth = Screen.PrimaryScreen.Bounds.Width
    End Sub
    '
    Private Function CasherLogin()
        If ID.Text <> "请输入账号" And key.Text <> "请输入密码" Then
            Try
                Dim sqlstr = "select id from casher where password = '" + key.Text.ToString() + "' and id = " + ID.Text.ToString + " and shop_id = " + shopID.ToString + ";"
                Dim sqlda As SQLite.SQLiteDataAdapter
                sqlda = New SQLite.SQLiteDataAdapter(sqlstr, sqliteconn)
                Dim tableData As New DataTable
                sqlda.Fill(tableData)
                If tableData.Rows.Count() Then
                    CasherLogin = True
                    Exit Function
                End If
                CasherLogin = False
            Catch ex As Exception
                CasherLogin = False
                write_errmsg(ex.Message, Me.Name, "CasherLogin", Me)
                'BeginInvoke(New write_err_msg(AddressOf Write_Err_Msg_Invo), ex.Message, Me.Name, "CasherLogin", Me)
            End Try
        Else
            CasherLogin = False
        End If
    End Function

    Private Sub CashLogin1()
        If CasherLogin() = True Then
            Me.Hide()
            cash.Show()
        Else  'when login error
            MsgboxNotice("请检查你的用户名密码", "登录失败", False, False, Nothing, Me, True, False)
            'BeginInvoke(New msgbox_de(AddressOf msgbox_invo), "请检查你的用户名密码", "登录失败", False, False, Nothing, Me, True, False)
        End If
    End Sub


    Private Sub key_Keypress(sender As Object, e As KeyPressEventArgs) Handles key.KeyPress
        If e.KeyChar = ChrW(13) And Not thread_load_flag Then
            invo()
        End If
    End Sub

    'Public Function ConcectDataIfBreak()
    '    Dim connStr As String
    '    If Not conn Is Nothing Then conn.Close()
    '    'connStr = String.Format("server={0};user id={1}; password={2}; database=member; pooling=false;charset=utf8", _
    '    '"112.74.105.67", "ming", "18883285787")

    '    connStr = String.Format("server={0};user id={1}; password={2}; database=member; pooling=false;charset=utf8", _
    '    "localhost", "root", "lsw19940816")

    '    Try
    '        conn = New MySqlConnection(connStr)
    '        conn.Open()
    '        If conn.State = ConnectionState.Open Then
    '            ConcectDataIfBreak = True
    '            Exit Function
    '        End If
    '        '    MsgBox("开")
    '        'Else
    '        '    MsgBox("关")
    '        'End If

    '    Catch ex As MySqlException
    '        MessageBox.Show("Error connecting to the server: " + ex.Message)
    '        'runThread.Abort()
    '        ConcectDataIfBreak = False
    '    End Try
    '    ConcectDataIfBreak = False
    'End Function

    'when esc pressed,windows app close the sql connenction and close itself
    Private Sub form_key(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            sqliteconn.Close()
            End
        End If
    End Sub

    'shopid base64 encryption

    'this function design to get shop id and name and save it into config data
    Private Sub shopGet()
        Dim fristrunflag As Boolean = False
        If My.Computer.FileSystem.DirectoryExists(".\config") Then

            If Not IO.File.Exists(".\config\data.ini") Then

            End If
            config()

            If IO.File.Exists(".\config\readme.txt") Then
            Else
                IO.File.Create(".\config\readme.txt").Close()
                Dim fsw As New StreamWriter(".\config\readme.txt")
                fsw.WriteLine("this folder for the application configuration,don't move or delete or rewite")
                fsw.Close()
                'save config date into file
            End If
        Else
            My.Computer.FileSystem.CreateDirectory(".\config")
            IO.File.Create(".\config\data.ini").Close()
            IO.File.Create(".\config\readme.txt").Close()
            Dim fsw As New StreamWriter(".\config\readme.txt")
            fsw.WriteLine("this folder for the application configuration,don't move or delete or rewite")
            fsw.Close()
            config()
        End If
    End Sub

    'config config files(also get shop id)
    Private Sub config()
        If IO.File.Exists(".\config\fristrun.ini") Then
            Dim fristrunfl As New StreamReader(".\config\fristrun.ini")
            Dim flag As String = ""
            flag = fristrunfl.ReadLine()
            fristrunfl.Close()

            Dim fsr As New StreamReader(".\config\data.ini")
            Dim IDtemp As String = ""
            Dim Nametemp As String = ""
            IDtemp = fsr.ReadLine()
            Nametemp = fsr.ReadLine()
            fsr.Close()

            If IDtemp = "" Or Nametemp = "" Then
                Dim fristrunWindow As New fristrun
                If Not flag = "1" Then
                    Dim Msgform As New MSG
                    Msgform.head.Text = "错误"
                    Msgform.msgP.Text = "配置文件丢失，请重新配置"
                    fristrunWindow.notice.Text = "您好，请选择店铺或者输入店铺编号："
                    Msgform.ShowDialog(Me)
                    'BeginInvoke(New msgbox_de(AddressOf msgbox_invo), "配置文件丢失，请重新配置", "错误", False, True, Nothing, Me, True, False)
                    'BeginInvoke(New Label_text(AddressOf Label_text_invo), fristrunWindow.notice, "您好，请选择店铺或者输入店铺编号：")
                Else
                    fristrunWindow.notice.Text = "您好，这是您第一次登录，请选择店铺或者输入店铺编号："
                    'BeginInvoke(New Label_text(AddressOf Label_text_invo), fristrunWindow.notice, "您好，这是您第一次登录，请选择店铺或者输入店铺编号：")
                End If
                fristrunWindow.Show(Me)
                'BeginInvoke(New windows_show(AddressOf win_show), fristrunWindow, Me)
            Else
                shopID = Long.Parse(IDtemp)
                'shopname
            End If
        Else
            IO.File.Create(".\config\fristrun.ini").Close()
            Dim rewrite As New StreamWriter(".\config\fristrun.ini") '重写配置文件
            rewrite.WriteLine("0")
            rewrite.Close()
            Dim fristrun As New fristrun
            fristrun.notice.Text = "您好，请选择店铺或者输入店铺编号："
            fristrun.Show(Me)
            'BeginInvoke(New Label_text(AddressOf Label_text_invo), fristrun.notice, "您好，请选择店铺或者输入店铺编号：")
            'BeginInvoke(New windows_show(AddressOf win_show), Me, Nothing)
        End If
    End Sub

    Private Sub logo_Click(sender As Object, e As EventArgs) Handles logo.Click
        If MsgboxNotice("继续以重新设置店铺ID及名称", "提示", True, True, "取消", Me, True, False) = DialogResult.OK Then
            shopreset = True
            Dim reset As New fristrun
            reset.notice.Text = "您好，请重新选择店铺或者输入店铺编号："
            reset.Show(Me)
        End If
    End Sub

    Public Function MsgboxNotice(ByVal msgp As String, ByVal head As String, ByVal No_B_visible As Boolean, ByVal Yes_B_visible As Boolean, ByVal canceltext As String, e As Form, ByVal info As Boolean, ByVal factoy As Boolean)
        Dim formmsg As New MSG
        formmsg.yes.Visible = Yes_B_visible
        formmsg.yes_button.Visible = Yes_B_visible
        formmsg.no.Visible = No_B_visible
        formmsg.no_button.Visible = No_B_visible
        formmsg.no_button.Text = canceltext
        formmsg.msgP.Text = msgp
        formmsg.head.Text = head
        If Not No_B_visible And Yes_B_visible Then
            formmsg.yes.Location = New Point(281, 160)
        End If
        If info Then
            formmsg.warn.Image = Image.FromFile(".\source\info.png")
        End If

        If factoy Then
            formmsg.msgP.TextAlign = ContentAlignment.MiddleLeft
        End If
        Return formmsg.ShowDialog(e)
    End Function

    '向文件写错误日志
    Public Sub write_errmsg(ByVal errmsg As String, ByVal window As String, ByVal func As String, e As Object)
        MsgboxNotice(errmsg, "发生了一个错误", False, True, Nothing, e, False, False)
        Dim time As String = Format(Now, "yyyy_MM_dd hh:mm:ss") '获得系统时间
        If IO.File.Exists(".\logs\exception.log") Then
        Else
            If Not My.Computer.FileSystem.DirectoryExists(".\logs") Then
                My.Computer.FileSystem.CreateDirectory(".\logs")
            End If
            IO.File.Create(".\logs\exception.log").Close()
        End If
        Dim fsw As New StreamWriter(".\logs\exception.log", True) '向日志中添加错误信息
        fsw.WriteLine(time & ":(window_" & window & ",func_" & func & ")" & errmsg)
        fsw.Close()
    End Sub


    'invo_msgbox
    Public Delegate Sub msgbox_de(meg As String, content As String, No_visible As Boolean, Yes_visible As Boolean, cancelText As String, e As Form, info As Boolean, factory As Boolean)
    Public Sub msgbox_invo(meg As String, content As String, No_visible As Boolean, Yes_visible As Boolean, cancelText As String, e As Form, info As Boolean, factory As Boolean)
        MsgboxNotice(meg, content, No_visible, Yes_visible, cancelText, e, info, factory)
    End Sub

    'invoke writeerror_msg
    Public Delegate Sub write_err_msg(str As String, strName As String, method As String, e As Form)
    Public Sub Write_Err_Msg_Invo(str As String, strName As String, method As String, e As Form)
        write_errmsg(str, strName, method, e)
    End Sub

    '重写  label 值
    Public Delegate Sub Label_text(mycontrol As Label, str As String)
    Public Sub Label_text_invo(mycontrol As Label, str As String)
        mycontrol.Text = str
    End Sub

    '重写 textbox 值
    Public Delegate Sub TextBox_text(mycontrol As TextBox, str As String)
    Public Sub TextBox_text_invo(mycontrol As TextBox, str As String)
        mycontrol.Text = str
    End Sub
    '在cla窗体上显示e  invo方法
    Delegate Sub windows_show(e As Form, cla As Form)
    Public Sub win_show(e As Form, cla As Form)
        e.Show(cla)
    End Sub
    '关闭e 窗体 invo方法
    Delegate Sub windows_close(e As Form)
    Public Sub win_close(e As Form)
        e.Close()
    End Sub

    '隐藏 窗体 invo方法
    Delegate Sub windows_hide(e As Form)
    Public Sub win_hide(e As Form)
        e.Hide()
    End Sub

End Class
